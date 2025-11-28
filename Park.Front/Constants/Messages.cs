namespace Park.Front.Constants
{
    /// <summary>
    /// Mensajes de error estandarizados para la aplicación
    /// </summary>
    public static class ErrorMessages
    {
        // Errores Generales
        public const string GENERIC_ERROR = "Ocurrió un error inesperado. Por favor, intente nuevamente.";
        public const string NETWORK_ERROR = "Error de conexión. Verifique su conexión a internet.";
        public const string UNAUTHORIZED = "No tiene permisos para realizar esta acción.";
        public const string VALIDATION_ERROR = "Por favor, complete todos los campos requeridos correctamente.";

        // Errores de Visitas
        public const string VISITA_LOAD_ERROR = "Error al cargar las visitas.";
        public const string VISITA_CREATE_ERROR = "Error al crear la visita.";
        public const string VISITA_UPDATE_ERROR = "Error al actualizar la visita.";
        public const string VISITA_DELETE_ERROR = "Error al eliminar la visita.";
        public const string VISITA_CANCEL_ERROR = "Error al cancelar la visita.";
        public const string VISITA_CHECKIN_ERROR = "Error al registrar la entrada.";
        public const string VISITA_CHECKOUT_ERROR = "Error al registrar la salida.";
        public const string VISITA_QR_DOWNLOAD_ERROR = "Error al descargar el código QR.";
        public const string VISITA_QR_COPY_ERROR = "Error al copiar la URL del código QR.";

        // Errores de Usuarios
        public const string USER_LOAD_ERROR = "Error al cargar los usuarios.";
        public const string USER_CREATE_ERROR = "Error al crear el usuario.";
        public const string USER_UPDATE_ERROR = "Error al actualizar el usuario.";
        public const string USER_DELETE_ERROR = "Error al eliminar el usuario.";
        public const string USER_LOCK_ERROR = "Error al bloquear el usuario.";
        public const string USER_UNLOCK_ERROR = "Error al desbloquear el usuario.";
        public const string USER_PASSWORD_CHANGE_ERROR = "Error al cambiar la contraseña.";
        public const string USER_ROLE_REQUIRED = "Debe seleccionar al menos un rol.";

        // Errores de Visitantes
        public const string VISITOR_LOAD_ERROR = "Error al cargar los visitantes.";
        public const string VISITOR_CREATE_ERROR = "Error al crear el visitante.";
        public const string VISITOR_UPDATE_ERROR = "Error al actualizar el visitante.";
        public const string VISITOR_DELETE_ERROR = "Error al eliminar el visitante.";

        // Errores de Empresas
        public const string COMPANY_LOAD_ERROR = "Error al cargar las empresas.";
        public const string COMPANY_CREATE_ERROR = "Error al crear la empresa.";
        public const string COMPANY_UPDATE_ERROR = "Error al actualizar la empresa.";
        public const string COMPANY_DELETE_ERROR = "Error al eliminar la empresa.";

        // Errores de Validación de Fechas
        public const string DATE_INVALID_RANGE = "La fecha de inicio debe ser anterior a la fecha de vencimiento.";
        public const string DATE_PAST_EXPIRATION = "La fecha de vencimiento debe ser posterior a la fecha actual.";

        // Errores de Autenticación
        public const string AUTH_INVALID_CREDENTIALS = "Usuario o contraseña incorrectos.";
        public const string AUTH_SESSION_EXPIRED = "Su sesión ha expirado. Por favor, inicie sesión nuevamente.";
        public const string AUTH_USER_NOT_FOUND = "No se pudo encontrar el usuario.";
    }

    /// <summary>
    /// Mensajes de éxito estandarizados para la aplicación
    /// </summary>
    public static class SuccessMessages
    {
        // Éxitos de Visitas
        public const string VISITA_CREATED = "Visita creada exitosamente.";
        public const string VISITA_UPDATED = "Visita actualizada exitosamente.";
        public const string VISITA_DELETED = "Visita eliminada exitosamente.";
        public const string VISITA_CANCELED = "Visita cancelada exitosamente.";
        public const string VISITA_CHECKIN = "Entrada registrada exitosamente.";
        public const string VISITA_CHECKOUT = "Salida registrada exitosamente.";
        public const string VISITA_QR_COPIED = "URL del código QR copiada al portapapeles.";

        // Éxitos de Usuarios
        public const string USER_CREATED = "Usuario creado exitosamente.";
        public const string USER_UPDATED = "Usuario actualizado exitosamente.";
        public const string USER_DELETED = "Usuario eliminado exitosamente.";
        public const string USER_LOCKED = "Usuario bloqueado exitosamente.";
        public const string USER_UNLOCKED = "Usuario desbloqueado exitosamente.";
        public const string USER_PASSWORD_CHANGED = "Contraseña cambiada exitosamente.";
        public const string USER_ZONE_ASSIGNED = "Zona asignada exitosamente.";

        // Éxitos de Visitantes
        public const string VISITOR_CREATED = "Visitante creado exitosamente.";
        public const string VISITOR_UPDATED = "Visitante actualizado exitosamente.";
        public const string VISITOR_DELETED = "Visitante eliminado exitosamente.";
        public const string VISITOR_ACTIVATED = "Visitante activado exitosamente.";
        public const string VISITOR_DEACTIVATED = "Visitante desactivado exitosamente.";

        // Éxitos de Empresas
        public const string COMPANY_CREATED = "Empresa creada exitosamente.";
        public const string COMPANY_UPDATED = "Empresa actualizada exitosamente.";
        public const string COMPANY_DELETED = "Empresa eliminada exitosamente.";
    }
}
