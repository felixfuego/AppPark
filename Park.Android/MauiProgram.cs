using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Park.Android.Services;
using Park.Android.ViewModels;
using Park.Android.Views;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Park.Android;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // Configurar HttpClient con mejor manejo de timeouts y SSL
        builder.Services.AddHttpClient("ParkApi", client =>
        {
            client.BaseAddress = new Uri("https://fintotal.kattangroup.com/park/");
            client.Timeout = TimeSpan.FromSeconds(120); // Aumentado a 120 segundos
            client.DefaultRequestHeaders.Add("User-Agent", "ParkAndroid/1.0");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        })
        .ConfigurePrimaryHttpMessageHandler(() =>
        {
#if DEBUG
            // En modo DEBUG, permitir certificados SSL no válidos (solo para desarrollo)
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
                {
                    // En producción HTTPS, esto debería validar correctamente
                    if (sslPolicyErrors == SslPolicyErrors.None)
                        return true;

                    Console.WriteLine($"[SSL] Certificate validation warning: {sslPolicyErrors}");
                    // Aceptar certificado en desarrollo
                    return true;
                },
                AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate
            };
#else
            // En producción, usar validación SSL estricta
            var handler = new HttpClientHandler
            {
                AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate
            };
#endif
            return handler;
        });

        // Registrar servicios
        builder.Services.AddSingleton<IApiService, ApiService>();
        builder.Services.AddSingleton<IAuthService, AuthService>();
        builder.Services.AddSingleton<IStorageService, StorageService>();
        builder.Services.AddSingleton<IVisitaService, VisitaService>();

        // Registrar ViewModels
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<DashboardViewModel>();
        builder.Services.AddTransient<VisitasListViewModel>();
        builder.Services.AddTransient<CheckInViewModel>();
        builder.Services.AddTransient<CheckOutViewModel>();

        // Registrar Pages
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<DashboardPage>();
        builder.Services.AddTransient<VisitasListPage>();
        builder.Services.AddTransient<CheckInPage>();
        builder.Services.AddTransient<CheckOutPage>();

        // Registrar AppShell
        builder.Services.AddSingleton<AppShell>();

        return builder.Build();
    }
}
