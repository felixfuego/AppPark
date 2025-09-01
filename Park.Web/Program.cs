using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Blazored.LocalStorage;
using Park.Web;
using Park.Web.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Registrar servicios
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<IAuthService, AuthService>();

// Configurar autenticación
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
    provider.GetRequiredService<CustomAuthenticationStateProvider>());
builder.Services.AddAuthorizationCore();

// Configurar HttpClient
//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://fintotal.kattangroup.com/park/") });
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7001/") });
// Registrar HttpClientService
builder.Services.AddScoped<HttpClientService>();

// Registrar servicios de API
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IZoneService, ZoneService>();
builder.Services.AddScoped<IGateService, GateService>();
builder.Services.AddScoped<IVisitorService, VisitorService>();
builder.Services.AddScoped<IVisitService, VisitService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();

// Configurar SignalR HubConnection
builder.Services.AddScoped<HubConnection>(sp =>
{
    var navigationManager = sp.GetRequiredService<NavigationManager>();
    return new HubConnectionBuilder()
        .WithUrl(navigationManager.ToAbsoluteUri("/parkhub"))
        .WithAutomaticReconnect()
        .Build();
});

var app = builder.Build();

// Inicializar servicios
var authService = app.Services.GetRequiredService<IAuthService>();
await authService.InitializeAsync();

await app.RunAsync();