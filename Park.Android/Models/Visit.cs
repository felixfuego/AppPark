using Newtonsoft.Json;

namespace Park.Maui.Models
{
    public class Visit
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("visitCode")]
        public string VisitCode { get; set; } = string.Empty;

        [JsonProperty("scheduledDate")]
        public DateTime ScheduledDate { get; set; }

        [JsonProperty("scheduledTime")]
        public TimeSpan ScheduledTime { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; } = string.Empty;

        [JsonProperty("purpose")]
        public string Purpose { get; set; } = string.Empty;

        [JsonProperty("notes")]
        public string? Notes { get; set; }

        [JsonProperty("checkInTime")]
        public DateTime? CheckInTime { get; set; }

        [JsonProperty("checkOutTime")]
        public DateTime? CheckOutTime { get; set; }

        [JsonProperty("visitor")]
        public Visitor? Visitor { get; set; }

        [JsonProperty("company")]
        public Company? Company { get; set; }

        [JsonProperty("gate")]
        public Gate? Gate { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        // Propiedades calculadas para la UI
        public string StatusDisplay => Status switch
        {
            "Pending" => "Pendiente",
            "InProgress" => "En Progreso",
            "Completed" => "Completada",
            "Cancelled" => "Cancelada",
            _ => Status
        };

        public string StatusColor => Status switch
        {
            "Pending" => "#FF9800",      // Orange
            "InProgress" => "#2196F3",   // Blue
            "Completed" => "#4CAF50",    // Green
            "Cancelled" => "#F44336",    // Red
            _ => "#757575"               // Gray
        };

        public string FormattedScheduledDate => ScheduledDate.ToString("dd/MM/yyyy");
        public string FormattedScheduledTime => ScheduledTime.ToString("HH:mm");
        public string FormattedCheckInTime => CheckInTime?.ToString("dd/MM/yyyy HH:mm") ?? "No registrado";
        public string FormattedCheckOutTime => CheckOutTime?.ToString("dd/MM/yyyy HH:mm") ?? "No registrado";
    }
}
