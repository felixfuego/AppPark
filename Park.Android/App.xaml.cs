namespace Park.Android;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // Configurar tema
        UserAppTheme = AppTheme.Light;

        // Usar Shell para navegación
        MainPage = new AppShell();
    }
}
