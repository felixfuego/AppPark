using Park.Android.Views;

namespace Park.Android;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Registrar rutas para navegación (solo las páginas secundarias)
        Routing.RegisterRoute(nameof(VisitasListPage), typeof(VisitasListPage));
        Routing.RegisterRoute(nameof(CheckInPage), typeof(CheckInPage));
        Routing.RegisterRoute(nameof(CheckOutPage), typeof(CheckOutPage));
    }
}
