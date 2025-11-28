using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Park.Android.Services;
using Park.Android.Views;
using Park.Comun.DTOs;
using System.Collections.ObjectModel;

namespace Park.Android.ViewModels;

public partial class VisitasListViewModel : ObservableObject
{
    private readonly IVisitaService _visitaService;
    private readonly IAuthService _authService;

    [ObservableProperty]
    private ObservableCollection<VisitaDto> visitas = new();

    private List<VisitaDto> _allVisitas = new();

    [ObservableProperty]
    private string searchText = string.Empty;

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private bool isRefreshing;

    [ObservableProperty]
    private string filterStatus = "Todas";

    public VisitasListViewModel(IVisitaService visitaService, IAuthService authService)
    {
        _visitaService = visitaService;
        _authService = authService;
    }

    public async Task InitializeAsync()
    {
        await LoadVisitasAsync();
    }

    [RelayCommand]
    private async Task LoadVisitasAsync()
    {
        IsLoading = true;

        try
        {
            var currentUser = await _authService.GetCurrentUserAsync();
            if (currentUser == null)
            {
                Console.WriteLine("Error: Usuario no está cargado");
                await Application.Current!.MainPage!.DisplayAlert("Error", "No se pudo obtener información del usuario", "OK");
                return;
            }

            List<VisitaDto> visitasList;
            var hoy = DateTime.Today;

            if (currentUser.Roles.Any(r => r.Name == "Guardia") && currentUser.IdZonaAsignada.HasValue)
            {
                Console.WriteLine($"Cargando visitas por zona para guardia ID: {currentUser.Id}");
                visitasList = await _visitaService.GetVisitasDelDiaAsync(currentUser.Id);
            }
            else
            {
                Console.WriteLine($"Cargando todas las visitas del día: {hoy:dd/MM/yyyy}");
                visitasList = await _visitaService.GetVisitasByFechaAsync(hoy);
            }
            
            // Filtrar solo las de hoy y ordenar
            _allVisitas = visitasList.Where(v => v.Fecha.Date == hoy).OrderBy(v => v.Fecha).ToList();
            
            Console.WriteLine($"Visitas obtenidas: {_allVisitas.Count}");
            
            Visitas.Clear();
            foreach (var visita in _allVisitas)
            {
                Visitas.Add(visita);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error cargando visitas: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            await Application.Current!.MainPage!.DisplayAlert("Error", "No se pudieron cargar las visitas", "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task RefreshVisitasAsync()
    {
        IsRefreshing = true;
        await LoadVisitasAsync();
        IsRefreshing = false;
    }

    [RelayCommand]
    private async Task SearchVisitasAsync()
    {
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            // Restaurar lista completa
            Visitas.Clear();
            foreach (var visita in _allVisitas)
            {
                Visitas.Add(visita);
            }
            return;
        }

        IsLoading = true;

        try
        {
            var searchTerm = SearchText.Trim();
            
            // Búsqueda local en _allVisitas
            var filteredList = _allVisitas.Where(v => 
                v.NumeroSolicitud.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                v.NombreCompleto.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                v.IdentidadVisitante.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                (v.Compania?.Name?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (v.Centro?.Nombre?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ?? false)
            ).ToList();
            
            Visitas.Clear();
            foreach (var visita in filteredList)
            {
                Visitas.Add(visita);
            }
            
            await Task.CompletedTask; // Mantener async signature
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error buscando visitas: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task VisitaSelectedAsync(VisitaDto visita)
    {
        if (visita == null)
            return;

        var action = await Application.Current!.MainPage!.DisplayActionSheet(
            $"Visita: {visita.NombreCompleto}",
            "Cancelar",
            null,
            "Ver Detalles",
            "Check-In",
            "Check-Out");

        switch (action)
        {
            case "Ver Detalles":
                await ShowVisitaDetailsAsync(visita);
                break;
            case "Check-In":
                await NavigateToCheckInAsync(visita);
                break;
            case "Check-Out":
                await NavigateToCheckOutAsync(visita);
                break;
        }
    }

    private async Task ShowVisitaDetailsAsync(VisitaDto visita)
    {
        var details = $"Número: {visita.NumeroSolicitud}\n" +
                     $"Visitante: {visita.NombreCompleto}\n" +
                     $"Identidad: {visita.IdentidadVisitante}\n" +
                     $"Estado: {visita.Estado}\n" +
                     $"Fecha: {visita.Fecha:dd/MM/yyyy}\n" +
                     $"Compañía: {visita.Compania?.Name ?? "N/A"}";

        await Application.Current!.MainPage!.DisplayAlert("Detalles de Visita", details, "OK");
    }

    private async Task NavigateToCheckInAsync(VisitaDto visita)
    {
        await Shell.Current.GoToAsync($"{nameof(CheckInPage)}?VisitaId={visita.Id}");
    }

    private async Task NavigateToCheckOutAsync(VisitaDto visita)
    {
        await Shell.Current.GoToAsync($"{nameof(CheckOutPage)}?VisitaId={visita.Id}");
    }
}
