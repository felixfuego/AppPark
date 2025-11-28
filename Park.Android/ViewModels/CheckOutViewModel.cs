using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Park.Android.Services;
using Park.Comun.DTOs;

namespace Park.Android.ViewModels;

[QueryProperty(nameof(VisitaIdString), "VisitaId")]
public partial class CheckOutViewModel : ObservableObject
{
    private readonly IVisitaService _visitaService;
    private readonly IAuthService _authService;

    private int _visitaId;
    
    [ObservableProperty]
    private VisitaDto? visita;

    [ObservableProperty]
    private string observaciones = string.Empty;

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    public int VisitaId
    {
        get => _visitaId;
        set
        {
            _visitaId = value;
            Console.WriteLine($"[CheckOutViewModel] VisitaId establecido: {value}");
            // Cargar la visita cuando se establece el ID
            Task.Run(async () => await LoadVisitaAsync());
        }
    }

    // Propiedad para aceptar el parámetro de Shell como string
    public string VisitaIdString
    {
        set
        {
            Console.WriteLine($"[CheckOutViewModel] Recibido VisitaId desde navegación: {value}");
            if (int.TryParse(value, out var id))
            {
                VisitaId = id;
            }
            else
            {
                Console.WriteLine($"[CheckOutViewModel] Error: No se pudo convertir '{value}' a int");
                ErrorMessage = "ID de visita inválido";
            }
        }
    }

    public CheckOutViewModel(IVisitaService visitaService, IAuthService authService)
    {
        _visitaService = visitaService;
        _authService = authService;
        Console.WriteLine("[CheckOutViewModel] Constructor inicializado");
    }

    public async Task InitializeAsync()
    {
        Console.WriteLine($"[CheckOutViewModel] InitializeAsync - VisitaId: {VisitaId}");
        if (VisitaId > 0 && Visita == null)
        {
            await LoadVisitaAsync();
        }
    }

    [RelayCommand]
    private async Task LoadVisitaAsync()
    {
        if (VisitaId <= 0)
        {
            Console.WriteLine("[CheckOutViewModel] LoadVisitaAsync - VisitaId inválido");
            ErrorMessage = "ID de visita no válido";
            return;
        }

        IsLoading = true;
        ErrorMessage = string.Empty;

        try
        {
            Console.WriteLine($"[CheckOutViewModel] Cargando visita ID: {VisitaId}");
            Visita = await _visitaService.GetVisitaByIdAsync(VisitaId);
            
            if (Visita == null)
            {
                ErrorMessage = "No se encontró la visita";
                Console.WriteLine($"[CheckOutViewModel] Visita no encontrada para ID: {VisitaId}");
            }
            else
            {
                Console.WriteLine($"[CheckOutViewModel] Visita cargada: {Visita.NombreCompleto} - Estado: {Visita.Estado}");
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error cargando visita: {ex.Message}";
            Console.WriteLine($"[CheckOutViewModel] Error: {ex.Message}");
            Console.WriteLine($"[CheckOutViewModel] StackTrace: {ex.StackTrace}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task ConfirmCheckOutAsync()
    {
        if (Visita == null)
        {
            await Application.Current!.MainPage!.DisplayAlert("Error", "No hay visita seleccionada", "OK");
            return;
        }

        var confirm = await Application.Current!.MainPage!.DisplayAlert(
            "Confirmar Check-Out",
            $"¿Confirmar salida de {Visita.NombreCompleto}?",
            "Sí",
            "No");

        if (!confirm)
            return;

        IsLoading = true;
        ErrorMessage = string.Empty;

        try
        {
            Console.WriteLine($"[CheckOutViewModel] Iniciando check-out para visita ID: {Visita.Id}");
            
            var currentUser = await _authService.GetCurrentUserAsync();
            
            if (currentUser == null)
            {
                ErrorMessage = "Error: Usuario no autenticado";
                Console.WriteLine("[CheckOutViewModel] Usuario no autenticado");
                return;
            }

            Console.WriteLine($"[CheckOutViewModel] Usuario guardia: {currentUser.Username} (ID: {currentUser.Id})");
            Console.WriteLine($"[CheckOutViewModel] Observaciones: {Observaciones}");

            var result = await _visitaService.CheckOutAsync(
                Visita.Id,
                currentUser.Id,
                Observaciones);

            if (result != null)
            {
                Console.WriteLine($"[CheckOutViewModel] Check-out exitoso. Nuevo estado: {result.Estado}");
                
                await Application.Current!.MainPage!.DisplayAlert(
                    "Éxito",
                    "Check-out realizado correctamente",
                    "OK");

                await Shell.Current.GoToAsync("..");
            }
            else
            {
                ErrorMessage = "No se pudo realizar el check-out";
                Console.WriteLine("[CheckOutViewModel] Check-out falló - resultado nulo");
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error: {ex.Message}";
            Console.WriteLine($"[CheckOutViewModel] Error en check-out: {ex.Message}");
            Console.WriteLine($"[CheckOutViewModel] StackTrace: {ex.StackTrace}");
            await Application.Current!.MainPage!.DisplayAlert("Error", ErrorMessage, "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task CancelAsync()
    {
        Console.WriteLine("[CheckOutViewModel] Cancelando check-out");
        await Shell.Current.GoToAsync("..");
    }
}
