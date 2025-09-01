namespace Park.Maui.Utils
{
    public static class Constants
    {
        // API Configuration
        public const string BaseApiUrl = "https://localhost:7001/api";
        public const string AuthEndpoint = "/auth/login";
        public const string VisitsEndpoint = "/visit";
        public const string CheckInEndpoint = "/visit/check-in";
        public const string CheckOutEndpoint = "/visit/check-out";

        // Storage Keys
        public const string TokenKey = "auth_token";
        public const string RefreshTokenKey = "refresh_token";
        public const string UserInfoKey = "user_info";
        public const string TokenExpiryKey = "token_expiry";

        // App Settings
        public const string AppName = "Park Guardia";
        public const int RequestTimeout = 30; // seconds

        // Visit Status
        public const string StatusPending = "Pending";
        public const string StatusInProgress = "InProgress";
        public const string StatusCompleted = "Completed";
        public const string StatusCancelled = "Cancelled";
    }
}
