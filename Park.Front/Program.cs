using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using MudBlazor.Services;
using Park.Front;
using Park.Front.Services;
using Park.Front.Components;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configurar HttpClient para conectar con el API
builder.Services.AddScoped(sp =>
{
    var httpClient = new HttpClient();
    httpClient.BaseAddress = new Uri("https://fintotal.kattangroup.com/park/");
    //httpClient.BaseAddress = new Uri("https://localhost:7001/");
    Console.WriteLine($"HttpClient BaseAddress configurado: {httpClient.BaseAddress}");
    return httpClient;
});

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
builder.Services.AddScoped<VisitorService>();
builder.Services.AddScoped<VisitaService>();

// Registrar servicio de notificaciones
builder.Services.AddScoped<NotificationService>();

// Registrar MudBlazor
builder.Services.AddMudServices();

var app = builder.Build();

// Inicializar layout manager
await app.Services.GetRequiredService<IJSRuntime>().InvokeVoidAsync("layoutManager.init");

await app.RunAsync();