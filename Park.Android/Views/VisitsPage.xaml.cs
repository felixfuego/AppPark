using Park.Maui.Models;
using Park.Maui.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Park.Maui.Views
{
    public partial class VisitsPage : ContentPage
    {
        private readonly IVisitService _visitService;
        private readonly IAuthService _authService;
        private ObservableCollection<Visit> _allVisits;
        private string _currentFilter = "All";

        public ObservableCollection<Visit> Visits { get; set; }
        public bool IsLoading { get; set; }
        public bool IsRefreshing { get; set; }
        public ICommand RefreshCommand { get; }

        public VisitsPage()
        {
            InitializeComponent();
            _visitService = new VisitService(new AuthService());
            _authService = new AuthService();
            _allVisits = new ObservableCollection<Visit>();
            Visits = new ObservableCollection<Visit>();
            
            RefreshCommand = new Command(async () => await LoadVisitsAsync());
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadVisitsAsync();
        }

        private async Task LoadVisitsAsync()
        {
            try
            {
                IsLoading = true;
                OnPropertyChanged(nameof(IsLoading));

                var visits = await _visitService.GetVisitsAsync();
                
                if (visits != null)
                {
                    _allVisits.Clear();
                    foreach (var visit in visits)
                    {
                        _allVisits.Add(visit);
                    }
                    
                    ApplyFilter(_currentFilter);
                }
                else
                {
                    await DisplayAlert("Error", "No se pudieron cargar las visitas", "OK");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error cargando visitas: {ex.Message}");
                await DisplayAlert("Error", "Error de conexión al cargar visitas", "OK");
            }
            finally
            {
                IsLoading = false;
                IsRefreshing = false;
                OnPropertyChanged(nameof(IsLoading));
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        private void ApplyFilter(string filter)
        {
            _currentFilter = filter;
            Visits.Clear();

            var filteredVisits = filter switch
            {
                "Pending" => _allVisits.Where(v => v.Status == "Pending"),
                "InProgress" => _allVisits.Where(v => v.Status == "InProgress"),
                "Completed" => _allVisits.Where(v => v.Status == "Completed"),
                _ => _allVisits.AsEnumerable()
            };

            foreach (var visit in filteredVisits)
            {
                Visits.Add(visit);
            }

            OnPropertyChanged(nameof(Visits));
            UpdateFilterButtons();
        }

        private void UpdateFilterButtons()
        {
            // Reset all buttons
            AllButton.BackgroundColor = Colors.Transparent;
            AllButton.TextColor = Colors.White;
            PendingButton.BackgroundColor = Colors.Transparent;
            PendingButton.TextColor = Colors.White;
            InProgressButton.BackgroundColor = Colors.Transparent;
            InProgressButton.TextColor = Colors.White;
            CompletedButton.BackgroundColor = Colors.Transparent;
            CompletedButton.TextColor = Colors.White;

            // Highlight current filter
            switch (_currentFilter)
            {
                case "All":
                    AllButton.BackgroundColor = Colors.White;
                    AllButton.TextColor = Color.FromArgb("#1976D2");
                    break;
                case "Pending":
                    PendingButton.BackgroundColor = Colors.White;
                    PendingButton.TextColor = Color.FromArgb("#1976D2");
                    break;
                case "InProgress":
                    InProgressButton.BackgroundColor = Colors.White;
                    InProgressButton.TextColor = Color.FromArgb("#1976D2");
                    break;
                case "Completed":
                    CompletedButton.BackgroundColor = Colors.White;
                    CompletedButton.TextColor = Color.FromArgb("#1976D2");
                    break;
            }
        }

        private async void OnFilterClicked(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                var filter = button.Text switch
                {
                    "Todas" => "All",
                    "Pendientes" => "Pending",
                    "En Progreso" => "InProgress",
                    "Completadas" => "Completed",
                    _ => "All"
                };

                ApplyFilter(filter);
            }
        }

        private async void OnVisitSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Visit selectedVisit)
            {
                VisitsCollectionView.SelectedItem = null; // Clear selection
                await ShowVisitDetailsAsync(selectedVisit);
            }
        }

        private async Task ShowVisitDetailsAsync(Visit visit)
        {
            var action = await DisplayActionSheet(
                $"Visita: {visit.VisitCode}",
                "Cancelar",
                null,
                "Ver Detalles",
                "Check-in",
                "Check-out"
            );

            switch (action)
            {
                case "Ver Detalles":
                    await ShowVisitDetailsModalAsync(visit);
                    break;
                case "Check-in":
                    await PerformCheckInAsync(visit);
                    break;
                case "Check-out":
                    await PerformCheckOutAsync(visit);
                    break;
            }
        }

        private async Task ShowVisitDetailsModalAsync(Visit visit)
        {
            var details = $"**Código de Visita:** {visit.VisitCode}\n\n" +
                         $"**Visitante:** {visit.Visitor?.FullName}\n" +
                         $"**Documento:** {visit.Visitor?.DocumentInfo}\n" +
                         $"**Empresa:** {visit.Company?.Name}\n" +
                         $"**Fecha:** {visit.FormattedScheduledDate}\n" +
                         $"**Hora:** {visit.FormattedScheduledTime}\n" +
                         $"**Estado:** {visit.StatusDisplay}\n" +
                         $"**Propósito:** {visit.Purpose}\n\n" +
                         $"**Check-in:** {visit.FormattedCheckInTime}\n" +
                         $"**Check-out:** {visit.FormattedCheckOutTime}";

            if (!string.IsNullOrEmpty(visit.Notes))
            {
                details += $"\n\n**Notas:** {visit.Notes}";
            }

            await DisplayAlert("Detalles de la Visita", details, "OK");
        }

        private async Task PerformCheckInAsync(Visit visit)
        {
            if (visit.Status != "Pending")
            {
                await DisplayAlert("Error", "Solo se puede hacer check-in a visitas pendientes", "OK");
                return;
            }

            var confirm = await DisplayAlert(
                "Confirmar Check-in",
                $"¿Confirmar entrada del visitante {visit.Visitor?.FullName}?",
                "Confirmar",
                "Cancelar"
            );

            if (confirm)
            {
                try
                {
                    // Por ahora usamos un gateId fijo (1), en el futuro se seleccionará
                    var success = await _visitService.CheckInVisitAsync(visit.Id, 1);
                    
                    if (success)
                    {
                        await DisplayAlert("Éxito", "Check-in realizado correctamente", "OK");
                        await LoadVisitsAsync(); // Recargar lista
                    }
                    else
                    {
                        await DisplayAlert("Error", "No se pudo realizar el check-in", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", "Error de conexión al realizar check-in", "OK");
                }
            }
        }

        private async Task PerformCheckOutAsync(Visit visit)
        {
            if (visit.Status != "InProgress")
            {
                await DisplayAlert("Error", "Solo se puede hacer check-out a visitas en progreso", "OK");
                return;
            }

            var confirm = await DisplayAlert(
                "Confirmar Check-out",
                $"¿Confirmar salida del visitante {visit.Visitor?.FullName}?",
                "Confirmar",
                "Cancelar"
            );

            if (confirm)
            {
                try
                {
                    // Por ahora usamos un gateId fijo (1), en el futuro se seleccionará
                    var success = await _visitService.CheckOutVisitAsync(visit.Id, 1);
                    
                    if (success)
                    {
                        await DisplayAlert("Éxito", "Check-out realizado correctamente", "OK");
                        await LoadVisitsAsync(); // Recargar lista
                    }
                    else
                    {
                        await DisplayAlert("Error", "No se pudo realizar el check-out", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", "Error de conexión al realizar check-out", "OK");
                }
            }
        }

        private async void OnScanQrClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Escáner QR", "Funcionalidad de escáner QR próximamente", "OK");
        }
    }
}
