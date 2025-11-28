using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Park.Android.Services;
using Park.Android.Views;
using Park.Comun.DTOs;

namespace Park.Android.ViewModels;

public partial class DashboardViewModel : ObservableObject
{
    private readonly IAuthService _authService;
    private readonly IVisitaService _visitaService;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CurrentUserRole))]
    private UserDto? currentUser;

    public string CurrentUserRole => CurrentUser?.Roles?.FirstOrDefault()?.Name ?? "Usuario";

    [ObservableProperty]
    private int visitasPendientes;

    [ObservableProperty]
    private int visitasEnProceso;

    [ObservableProperty]
    private int visitasCompletadas;

    [ObservableProperty]
    private int totalVisitasHoy;

    [ObservableProperty]
    private bool isLoading;

    public DashboardViewModel(IAuthService authService, IVisitaService visitaService)
    {
        _authService = authService;
        _visitaService = visitaService;
    }

    public async Task InitializeAsync()
    {
        CurrentUser = await _authService.GetCurrentUserAsync();
        await LoadDashboardDataAsync();
    }

    [RelayCommand]
    private async Task LoadDashboardDataAsync()
    {
        IsLoading = true;

        try
        {
            if (CurrentUser == null)
            {
                Console.WriteLine("Error: Usuario no está cargado");
                return;
            }

            List<VisitaDto> visitas;
            var hoy = DateTime.Today;

            // Lógica de roles: Si es Guardia con Zona asignada -> GetVisitasDelDiaAsync (filtra por zona)
            // Si no -> GetVisitasByFechaAsync (todas las del día)
            if (CurrentUser.Roles.Any(r => r.Name == "Guardia") && CurrentUser.IdZonaAsignada.HasValue)
            {
                Console.WriteLine($"Cargando visitas por zona para guardia ID: {CurrentUser.Id}");
                visitas = await _visitaService.GetVisitasDelDiaAsync(CurrentUser.Id);
            }
            else
            {
                Console.WriteLine($"Cargando todas las visitas del día: {hoy:dd/MM/yyyy}");
                visitas = await _visitaService.GetVisitasByFechaAsync(hoy);
            }
            
            Console.WriteLine($"Visitas obtenidas: {visitas.Count}");
            
            // Filtrar solo las de hoy para asegurar consistencia (aunque el servicio ya debería hacerlo)
            visitas = visitas.Where(v => v.Fecha.Date == hoy).ToList();
            
            TotalVisitasHoy = visitas.Count;
            VisitasPendientes = visitas.Count(v => v.Estado == Park.Comun.Enums.VisitStatus.Programada);
            VisitasEnProceso = visitas.Count(v => v.Estado == Park.Comun.Enums.VisitStatus.EnProceso);
            VisitasCompletadas = visitas.Count(v => v.Estado == Park.Comun.Enums.VisitStatus.Terminada);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error cargando dashboard: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task NavigateToVisitasListAsync()
    {
        await Shell.Current.GoToAsync(nameof(VisitasListPage));
    }

    [RelayCommand]
    private async Task NavigateToCheckInAsync()
    {
        await Shell.Current.GoToAsync(nameof(CheckInPage));
    }

    [RelayCommand]
    private async Task LogoutAsync()
    {
        var confirm = await Application.Current!.MainPage!.DisplayAlert(
            "Cerrar Sesión",
            "¿Está seguro que desea cerrar sesión?",
            "Sí",
            "No");

        if (confirm)
        {
            await _authService.LogoutAsync();
            // Reiniciar la aplicación - MAUI manejará la recreación
            System.Environment.Exit(0);
        }
    }
}
