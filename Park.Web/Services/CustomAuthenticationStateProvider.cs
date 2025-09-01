using System.Security.Claims;
using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Park.Web.Models;

namespace Park.Web.Services;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    private readonly IAuthService _authService;
    private readonly NavigationManager _navigationManager;
    private readonly ILogger<CustomAuthenticationStateProvider> _logger;

    public CustomAuthenticationStateProvider(
        ILocalStorageService localStorage,
        IAuthService authService,
        NavigationManager navigationManager,
        ILogger<CustomAuthenticationStateProvider> logger)
    {
        _localStorage = localStorage;
        _authService = authService;
        _navigationManager = navigationManager;
        _logger = logger;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            
            if (string.IsNullOrEmpty(token))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            // Verificar si el token ha expirado
            if (IsTokenExpired(token))
            {
                _logger.LogWarning("Token expirado detectado, redirigiendo al login");
                await _authService.LogoutAsync();
                _navigationManager.NavigateTo("/login", true);
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            // Intentar obtener el usuario actual
            var user = await _authService.GetCurrentUserAsync();
            if (user == null)
            {
                _logger.LogWarning("Usuario no encontrado, limpiando sesión");
                await _authService.LogoutAsync();
                _navigationManager.NavigateTo("/login", true);
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            // Crear claims del usuario
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName)
            };

            // Agregar roles
            if (user.Roles != null)
            {
                foreach (var role in user.Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.Name));
                }
            }

            var identity = new ClaimsIdentity(claims, "jwt");
            var principal = new ClaimsPrincipal(identity);

            return new AuthenticationState(principal);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el estado de autenticación");
            await _authService.LogoutAsync();
            _navigationManager.NavigateTo("/login", true);
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }

    public void NotifyUserAuthentication(string token)
    {
        var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
        var user = new ClaimsPrincipal(identity);
        var authState = Task.FromResult(new AuthenticationState(user));
        NotifyAuthenticationStateChanged(authState);
    }

    public void NotifyUserLogout()
    {
        var identity = new ClaimsIdentity();
        var user = new ClaimsPrincipal(identity);
        var authState = Task.FromResult(new AuthenticationState(user));
        NotifyAuthenticationStateChanged(authState);
    }

    private bool IsTokenExpired(string token)
    {
        try
        {
            var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var jsonToken = handler.ReadJwtToken(token);
            
            if (jsonToken.ValidTo == DateTime.MinValue)
                return false;

            // Considerar el token expirado si falta menos de 5 minutos
            return jsonToken.ValidTo < DateTime.UtcNow.AddMinutes(5);
        }
        catch
        {
            return true;
        }
    }

    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var claims = new List<Claim>();
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

        if (keyValuePairs != null)
        {
            keyValuePairs.TryGetValue(ClaimTypes.Role, out object? roles);

            if (roles != null)
            {
                if (roles.ToString()!.Trim().StartsWith("["))
                {
                    var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString()!);
                    if (parsedRoles != null)
                    {
                        foreach (var parsedRole in parsedRoles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, parsedRole));
                        }
                    }
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, roles.ToString()!));
                }

                keyValuePairs.Remove(ClaimTypes.Role);
            }

            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()!)));
        }

        return claims;
    }

    private byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 0: return Convert.FromBase64String(base64);
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }
} 