using Android.App;
using Android.OS;
using Android.Widget;
using Android.Content;
using Android.Views;
using Park.MovilApp.Services;
using Park.MovilApp.Models;

namespace Park.MovilApp
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private EditText usernameEditText;
        private EditText passwordEditText;
        private Button loginButton;
        private ProgressBar loadingProgressBar;
        private ApiService _apiService;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            _apiService = new ApiService();

            // Initialize views
            usernameEditText = FindViewById<EditText>(Resource.Id.usernameEditText);
            passwordEditText = FindViewById<EditText>(Resource.Id.passwordEditText);
            loginButton = FindViewById<Button>(Resource.Id.loginButton);
            loadingProgressBar = FindViewById<ProgressBar>(Resource.Id.loadingProgressBar);

            // Set up click event for login button
            loginButton.Click += OnLoginButtonClick;
        }

        private async void OnLoginButtonClick(object? sender, System.EventArgs e)
        {
            string username = usernameEditText.Text;
            string password = passwordEditText.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                Toast.MakeText(this, "Por favor complete todos los campos", ToastLength.Short).Show();
                return;
            }

            ShowLoading(true);
            loginButton.Enabled = false;

            try
            {
                var response = await _apiService.LoginAsync(username, password);
                
                if (response.Success && response.User != null)
                {
                    // Verificar si el usuario es guardia
                    var isGuardia = response.User.Roles.Any(r => r.Name == "Guardia");
                    
                    if (isGuardia)
                    {
                        Toast.MakeText(this, $"Bienvenido {response.User.FullName}!", ToastLength.Short).Show();
                        
                        // Navegar al panel de guardia
                        var intent = new Intent(this, typeof(GuardiaActivity));
                        StartActivity(intent);
                        Finish(); // Cerrar la actividad de login
                    }
                    else
                    {
                        Toast.MakeText(this, "Acceso denegado. Solo guardias pueden usar esta aplicación.", ToastLength.Long).Show();
                    }
                }
                else
                {
                    Toast.MakeText(this, response.Message, ToastLength.Long).Show();
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, $"Error de conexión: {ex.Message}", ToastLength.Long).Show();
            }
            finally
            {
                ShowLoading(false);
                loginButton.Enabled = true;
            }
        }

        private void ShowLoading(bool show)
        {
            loadingProgressBar.Visibility = show ? ViewStates.Visible : ViewStates.Gone;
            loginButton.Visibility = show ? ViewStates.Gone : ViewStates.Visible;
        }
    }
}