using MudBlazor;

namespace Park.Front.Services
{
    /// <summary>
    /// Servicio centralizado para mostrar notificaciones al usuario usando MudBlazor Snackbar
    /// </summary>
    public class NotificationService
    {
        private readonly ISnackbar _snackbar;

        public NotificationService(ISnackbar snackbar)
        {
            _snackbar = snackbar;
        }

        /// <summary>
        /// Muestra una notificación de éxito
        /// </summary>
        public void ShowSuccess(string message)
        {
            _snackbar.Add(message, Severity.Success, config =>
            {
                config.VisibleStateDuration = 3000; // 3 segundos
                config.HideTransitionDuration = 500;
                config.ShowTransitionDuration = 500;
            });
        }

        /// <summary>
        /// Muestra una notificación de error
        /// </summary>
        public void ShowError(string message)
        {
            _snackbar.Add(message, Severity.Error, config =>
            {
                config.VisibleStateDuration = 5000; // 5 segundos para errores
                config.HideTransitionDuration = 500;
                config.ShowTransitionDuration = 500;
            });
        }

        /// <summary>
        /// Muestra una notificación de advertencia
        /// </summary>
        public void ShowWarning(string message)
        {
            _snackbar.Add(message, Severity.Warning, config =>
            {
                config.VisibleStateDuration = 4000; // 4 segundos
                config.HideTransitionDuration = 500;
                config.ShowTransitionDuration = 500;
            });
        }

        /// <summary>
        /// Muestra una notificación informativa
        /// </summary>
        public void ShowInfo(string message)
        {
            _snackbar.Add(message, Severity.Info, config =>
            {
                config.VisibleStateDuration = 3000; // 3 segundos
                config.HideTransitionDuration = 500;
                config.ShowTransitionDuration = 500;
            });
        }

        /// <summary>
        /// Muestra una notificación de error genérico con mensaje predeterminado
        /// </summary>
        public void ShowGenericError()
        {
            ShowError("Ocurrió un error inesperado. Por favor, intente nuevamente.");
        }

        /// <summary>
        /// Muestra una notificación de error de red
        /// </summary>
        public void ShowNetworkError()
        {
            ShowError("Error de conexión. Verifique su conexión a internet.");
        }

        /// <summary>
        /// Muestra una notificación de error de autorización
        /// </summary>
        public void ShowUnauthorizedError()
        {
            ShowError("No tiene permisos para realizar esta acción.");
        }
    }
}
