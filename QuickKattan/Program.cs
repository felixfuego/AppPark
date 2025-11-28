using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using QuickKattan;
using QuickKattan.Services;
using MudBlazor.Services;
using ApexCharts;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configurar HttpClient para la API(https://fintotal.kattangroup.com/park/)
//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7001/") }); // Ajustar URL de tu API
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://fintotal.kattangroup.com/park/") }); // Ajustar URL de tu API
// Agregar MudBlazor
builder.Services.AddMudServices();

// Agregar ApexCharts
builder.Services.AddApexCharts();

// Agregar servicios personalizados
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IFinancialDataService, FinancialDataService>();
builder.Services.AddScoped<ITokenService, TokenService>();

await builder.Build().RunAsync();
