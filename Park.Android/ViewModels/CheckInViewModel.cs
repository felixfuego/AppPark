using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Park.Android.Services;
using Park.Comun.DTOs;

namespace Park.Android.ViewModels;

[QueryProperty(nameof(VisitaIdString), "VisitaId")]
public partial class CheckInViewModel : ObservableObject
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
            Console.WriteLine($"[CheckInViewModel] VisitaId establecido: {value}");
            // Cargar la visita cuando se establece el ID
            Task.Run(async () => await LoadVisitaAsync());
        }
    }

    // Propiedad para aceptar el parámetro de Shell como string
    public string VisitaIdString
    {
        set
        {
            Console.WriteLine($"[CheckInViewModel] Recibido VisitaId desde navegación: {value}");
            if (int.TryParse(value, out var id))
            {
                VisitaId = id;
            }
            else
            {
                Console.WriteLine($"[CheckInViewModel] Error: No se pudo convertir '{value}' a int");
                ErrorMessage = "ID de visita inválido";
            }
        }
    }

    public CheckInViewModel(IVisitaService visitaService, IAuthService authService)
    {
        _visitaService = visitaService;
        _authService = authService;
        Console.WriteLine("[CheckInViewModel] Constructor inicializado");
    }

    public async Task InitializeAsync()
    {
        Console.WriteLine($"[CheckInViewModel] InitializeAsync - VisitaId: {VisitaId}");
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
            Console.WriteLine("[CheckInViewModel] LoadVisitaAsync - VisitaId inválido");
            ErrorMessage = "ID de visita no válido";
            return;
        }

        IsLoading = true;
        ErrorMessage = string.Empty;

        try
        {
            Console.WriteLine($"[CheckInViewModel] Cargando visita ID: {VisitaId}");
            Visita = await _visitaService.GetVisitaByIdAsync(VisitaId);
            
            if (Visita == null)
            {
                ErrorMessage = "No se encontró la visita";
                Console.WriteLine($"[CheckInViewModel] Visita no encontrada para ID: {VisitaId}");
            }
            else
            {
                Console.WriteLine($"[CheckInViewModel] Visita cargada: {Visita.NombreCompleto} - Estado: {Visita.Estado}");
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error cargando visita: {ex.Message}";
            Console.WriteLine($"[CheckInViewModel] Error: {ex.Message}");
            Console.WriteLine($"[CheckInViewModel] StackTrace: {ex.StackTrace}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task ConfirmCheckInAsync()
    {
        if (Visita == null)
        {
            await Application.Current!.MainPage!.DisplayAlert("Error", "No hay visita seleccionada", "OK");
            return;
        }

        var confirm = await Application.Current!.MainPage!.DisplayAlert(
            "Confirmar Check-In",
            $"¿Confirmar entrada de {Visita.NombreCompleto}?",
            "Sí",
            "No");

        if (!confirm)
            return;

        IsLoading = true;
        ErrorMessage = string.Empty;

        try
        {
            Console.WriteLine($"[CheckInViewModel] Iniciando check-in para visita ID: {Visita.Id}");
            
            var currentUser = await _authService.GetCurrentUserAsync();
            
            if (currentUser == null)
            {
                ErrorMessage = "Error: Usuario no autenticado";
                Console.WriteLine("[CheckInViewModel] Usuario no autenticado");
                return;
            }

            Console.WriteLine($"[CheckInViewModel] Usuario guardia: {currentUser.Username} (ID: {currentUser.Id})");
            Console.WriteLine($"[CheckInViewModel] Observaciones: {Observaciones}");

            var result = await _visitaService.CheckInAsync(
                Visita.Id,
                currentUser.Id,
                Observaciones);

            if (result != null)
            {
                Console.WriteLine($"[CheckInViewModel] Check-in exitoso. Nuevo estado: {result.Estado}");
                
                await Application.Current!.MainPage!.DisplayAlert(
                    "Éxito",
                    "Check-in realizado correctamente",
                    "OK");

                await Shell.Current.GoToAsync("..");
            }
            else
            {
                ErrorMessage = "No se pudo realizar el check-in";
                Console.WriteLine("[CheckInViewModel] Check-in falló - resultado nulo");
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error: {ex.Message}";
            Console.WriteLine($"[CheckInViewModel] Error en check-in: {ex.Message}");
            Console.WriteLine($"[CheckInViewModel] StackTrace: {ex.StackTrace}");
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
        Console.WriteLine("[CheckInViewModel] Cancelando check-in");
        await Shell.Current.GoToAsync("..");
    }
}
