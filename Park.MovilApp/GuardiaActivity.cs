using Android.App;
using Android.OS;
using Android.Widget;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Park.MovilApp.Models;
using Park.MovilApp.Services;
using System.Collections.Generic;
using System.Linq;

namespace Park.MovilApp
{
    [Activity(Label = "Panel de Guardia", Theme = "@android:style/Theme.Material.Light")]
    public class GuardiaActivity : Activity
    {
        private ApiService _apiService;
        private UserInfo? _currentUser;
        private List<VisitInfo> _visits = new List<VisitInfo>();
        private List<GateInfo> _assignedGates = new List<GateInfo>();
        
        private TextView welcomeText;
        private TextView statsText;
        private ListView visitsListView;
        private EditText searchEditText;
        private Spinner gateSpinner;
        private Button refreshButton;
        private Button logoutButton;
        private ProgressBar loadingProgressBar;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.guardia_layout);

            _apiService = new ApiService();

            InitializeViews();
            LoadUserData();
        }

        private void InitializeViews()
        {
            welcomeText = FindViewById<TextView>(Resource.Id.welcomeText);
            statsText = FindViewById<TextView>(Resource.Id.statsText);
            visitsListView = FindViewById<ListView>(Resource.Id.visitsListView);
            searchEditText = FindViewById<EditText>(Resource.Id.searchEditText);
            gateSpinner = FindViewById<Spinner>(Resource.Id.gateSpinner);
            refreshButton = FindViewById<Button>(Resource.Id.refreshButton);
            logoutButton = FindViewById<Button>(Resource.Id.logoutButton);
            loadingProgressBar = FindViewById<ProgressBar>(Resource.Id.loadingProgressBar);

            refreshButton.Click += OnRefreshClick;
            logoutButton.Click += OnLogoutClick;
            searchEditText.TextChanged += OnSearchTextChanged;
            visitsListView.ItemClick += OnVisitItemClick;
        }

        private async void LoadUserData()
        {
            ShowLoading(true);
            
            try
            {
                var userResponse = await _apiService.GetCurrentUserAsync();
                if (userResponse.Success && userResponse.User != null)
                {
                    _currentUser = userResponse.User;
                    _assignedGates = _currentUser.AssignedGates;
                    
                    UpdateUI();
                    LoadVisits();
                }
                else
                {
                    Toast.MakeText(this, "Error al cargar datos del usuario", ToastLength.Long).Show();
                    Finish();
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, $"Error: {ex.Message}", ToastLength.Long).Show();
                Finish();
            }
            finally
            {
                ShowLoading(false);
            }
        }

        private async void LoadVisits()
        {
            ShowLoading(true);
            
            try
            {
                var visitsResponse = await _apiService.GetMyVisitsAsync();
                if (visitsResponse.Success && visitsResponse.Data != null)
                {
                    _visits = visitsResponse.Data;
                    UpdateVisitsList();
                    UpdateStats();
                }
                else
                {
                    Toast.MakeText(this, visitsResponse.Message, ToastLength.Short).Show();
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, $"Error al cargar visitas: {ex.Message}", ToastLength.Short).Show();
            }
            finally
            {
                ShowLoading(false);
            }
        }

        private void UpdateUI()
        {
            if (_currentUser != null)
            {
                welcomeText.Text = $"Bienvenido, {_currentUser.FullName}";
                
                // Configurar spinner de portones
                var gateNames = _assignedGates.Select(g => $"{g.Name} ({g.GateNumber})").ToList();
                var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, gateNames);
                adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                gateSpinner.Adapter = adapter;
            }
        }

        private void UpdateVisitsList()
        {
            var filteredVisits = FilterVisits();
            var adapter = new VisitAdapter(this, filteredVisits);
            visitsListView.Adapter = adapter;
        }

        private void UpdateStats()
        {
            var pending = _visits.Count(v => v.Status == "Pending");
            var inProgress = _visits.Count(v => v.Status == "InProgress");
            var completed = _visits.Count(v => v.Status == "Completed");
            
            statsText.Text = $"Pendientes: {pending} | En Progreso: {inProgress} | Completadas: {completed}";
        }

        private List<VisitInfo> FilterVisits()
        {
            var filtered = _visits.AsEnumerable();
            
            // Filtrar por búsqueda
            if (!string.IsNullOrEmpty(searchEditText.Text))
            {
                var searchTerm = searchEditText.Text.ToLower();
                filtered = filtered.Where(v => 
                    v.VisitorName.ToLower().Contains(searchTerm) ||
                    v.CompanyName.ToLower().Contains(searchTerm) ||
                    v.VisitCode.ToLower().Contains(searchTerm));
            }
            
            // Filtrar por portón seleccionado
            if (gateSpinner.SelectedItemPosition >= 0 && gateSpinner.SelectedItemPosition < _assignedGates.Count)
            {
                var selectedGate = _assignedGates[gateSpinner.SelectedItemPosition];
                filtered = filtered.Where(v => v.GateName == selectedGate.Name);
            }
            
            return filtered.ToList();
        }

        private void OnRefreshClick(object? sender, EventArgs e)
        {
            LoadVisits();
        }

        private void OnLogoutClick(object? sender, EventArgs e)
        {
            var alert = new AlertDialog.Builder(this);
            alert.SetTitle("Cerrar Sesión");
            alert.SetMessage("¿Está seguro que desea cerrar sesión?");
            alert.SetPositiveButton("Sí", async (sender, args) =>
            {
                await _apiService.LogoutAsync();
                Finish();
            });
            alert.SetNegativeButton("No", (sender, args) => { });
            alert.Show();
        }

        private void OnSearchTextChanged(object? sender, Android.Text.TextChangedEventArgs e)
        {
            UpdateVisitsList();
        }

        private void OnVisitItemClick(object? sender, AdapterView.ItemClickEventArgs e)
        {
            var visit = FilterVisits()[e.Position];
            ShowVisitActions(visit);
        }

        private void ShowVisitActions(VisitInfo visit)
        {
            var actions = new string[] { "Ver Detalles", "Check-In", "Check-Out" };
            
            var alert = new AlertDialog.Builder(this);
            alert.SetTitle($"Visita: {visit.VisitCode}");
            alert.SetItems(actions, (sender, args) =>
            {
                switch (args.Which)
                {
                    case 0: // Ver Detalles
                        ShowVisitDetails(visit);
                        break;
                    case 1: // Check-In
                        if (visit.Status == "Pending")
                            ShowCheckInDialog(visit);
                        else
                            Toast.MakeText(this, "Esta visita no puede hacer check-in", ToastLength.Short).Show();
                        break;
                    case 2: // Check-Out
                        if (visit.Status == "InProgress")
                            ShowCheckOutDialog(visit);
                        else
                            Toast.MakeText(this, "Esta visita no puede hacer check-out", ToastLength.Short).Show();
                        break;
                }
            });
            alert.Show();
        }

        private void ShowVisitDetails(VisitInfo visit)
        {
            var details = $"Código: {visit.VisitCode}\n" +
                         $"Visitante: {visit.VisitorName}\n" +
                         $"Empresa: {visit.CompanyName}\n" +
                         $"Portón: {visit.GateName}\n" +
                         $"Estado: {visit.StatusDisplay}\n" +
                         $"Fecha: {visit.ScheduledDate:dd/MM/yyyy HH:mm}\n" +
                         $"Propósito: {visit.Purpose}";

            var alert = new AlertDialog.Builder(this);
            alert.SetTitle("Detalles de la Visita");
            alert.SetMessage(details);
            alert.SetPositiveButton("OK", (sender, args) => { });
            alert.Show();
        }

        private async void ShowCheckInDialog(VisitInfo visit)
        {
            var alert = new AlertDialog.Builder(this);
            alert.SetTitle("Confirmar Check-In");
            alert.SetMessage($"¿Confirmar check-in para {visit.VisitorName}?");
            alert.SetPositiveButton("Confirmar", async (sender, args) =>
            {
                await PerformCheckIn(visit);
            });
            alert.SetNegativeButton("Cancelar", (sender, args) => { });
            alert.Show();
        }

        private async void ShowCheckOutDialog(VisitInfo visit)
        {
            var alert = new AlertDialog.Builder(this);
            alert.SetTitle("Confirmar Check-Out");
            alert.SetMessage($"¿Confirmar check-out para {visit.VisitorName}?");
            alert.SetPositiveButton("Confirmar", async (sender, args) =>
            {
                await PerformCheckOut(visit);
            });
            alert.SetNegativeButton("Cancelar", (sender, args) => { });
            alert.Show();
        }

        private async Task PerformCheckIn(VisitInfo visit)
        {
            ShowLoading(true);
            
            try
            {
                var selectedGate = _assignedGates[gateSpinner.SelectedItemPosition];
                var request = new CheckInRequest
                {
                    VisitId = visit.Id,
                    GateId = selectedGate.Id
                };

                var response = await _apiService.CheckInVisitAsync(request);
                if (response.Success)
                {
                    Toast.MakeText(this, "Check-in realizado exitosamente", ToastLength.Short).Show();
                    LoadVisits(); // Recargar datos
                }
                else
                {
                    Toast.MakeText(this, response.Message, ToastLength.Short).Show();
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, $"Error: {ex.Message}", ToastLength.Short).Show();
            }
            finally
            {
                ShowLoading(false);
            }
        }

        private async Task PerformCheckOut(VisitInfo visit)
        {
            ShowLoading(true);
            
            try
            {
                var selectedGate = _assignedGates[gateSpinner.SelectedItemPosition];
                var request = new CheckOutRequest
                {
                    VisitId = visit.Id,
                    GateId = selectedGate.Id
                };

                var response = await _apiService.CheckOutVisitAsync(request);
                if (response.Success)
                {
                    Toast.MakeText(this, "Check-out realizado exitosamente", ToastLength.Short).Show();
                    LoadVisits(); // Recargar datos
                }
                else
                {
                    Toast.MakeText(this, response.Message, ToastLength.Short).Show();
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, $"Error: {ex.Message}", ToastLength.Short).Show();
            }
            finally
            {
                ShowLoading(false);
            }
        }

        private void ShowLoading(bool show)
        {
            loadingProgressBar.Visibility = show ? ViewStates.Visible : ViewStates.Gone;
        }
    }

    public class VisitAdapter : BaseAdapter<VisitInfo>
    {
        private readonly Activity _context;
        private readonly List<VisitInfo> _visits;

        public VisitAdapter(Activity context, List<VisitInfo> visits)
        {
            _context = context;
            _visits = visits;
        }

        public override VisitInfo this[int position] => _visits[position];
        public override int Count => _visits.Count;
        public override long GetItemId(int position) => position;

        public override View GetView(int position, View? convertView, ViewGroup? parent)
        {
            var view = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.visit_item, null);
            var visit = _visits[position];

            var visitorNameText = view.FindViewById<TextView>(Resource.Id.visitorNameText);
            var companyNameText = view.FindViewById<TextView>(Resource.Id.companyNameText);
            var statusText = view.FindViewById<TextView>(Resource.Id.statusText);
            var timeText = view.FindViewById<TextView>(Resource.Id.timeText);

            visitorNameText.Text = visit.VisitorName;
            companyNameText.Text = visit.CompanyName;
            statusText.Text = visit.StatusDisplay;
            statusText.SetTextColor(Color.ParseColor(visit.StatusColor));
            timeText.Text = visit.ScheduledDate.ToString("dd/MM HH:mm");

            return view;
        }
    }
}
