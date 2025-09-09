using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using Park.Front;
using Park.Front.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


// Configurar HttpClient para conectar con el API
var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7001";
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseUrl) });

// Registrar servicios de autenticación
builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<RoleService>();

// Registrar servicios del parque industrial
builder.Services.AddScoped<SitioService>();
builder.Services.AddScoped<ZonaService>();
builder.Services.AddScoped<CentroService>();
builder.Services.AddScoped<CompanyService>();
builder.Services.AddScoped<ColaboradorService>();

var app = builder.Build();

// Inicializar layout manager
await app.Services.GetRequiredService<IJSRuntime>().InvokeVoidAsync("layoutManager.init");

await app.RunAsync();
