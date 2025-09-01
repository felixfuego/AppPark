using Newtonsoft.Json;

namespace Park.MovilApp.Models
{
    public class VisitInfo
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("visitCode")]
        public string VisitCode { get; set; } = string.Empty;

        [JsonProperty("visitorName")]
        public string VisitorName { get; set; } = string.Empty;

        [JsonProperty("visitorEmail")]
        public string VisitorEmail { get; set; } = string.Empty;

        [JsonProperty("visitorPhone")]
        public string VisitorPhone { get; set; } = string.Empty;

        [JsonProperty("companyName")]
        public string CompanyName { get; set; } = string.Empty;

        [JsonProperty("gateName")]
        public string GateName { get; set; } = string.Empty;

        [JsonProperty("status")]
        public string Status { get; set; } = string.Empty;

        [JsonProperty("scheduledDate")]
        public DateTime ScheduledDate { get; set; }

        [JsonProperty("checkInTime")]
        public DateTime? CheckInTime { get; set; }

        [JsonProperty("checkOutTime")]
        public DateTime? CheckOutTime { get; set; }

        [JsonProperty("purpose")]
        public string Purpose { get; set; } = string.Empty;

        [JsonProperty("notes")]
        public string Notes { get; set; } = string.Empty;

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
            "Pending" => "#FFA500", // Naranja
            "InProgress" => "#007BFF", // Azul
            "Completed" => "#28A745", // Verde
            "Cancelled" => "#DC3545", // Rojo
            _ => "#6C757D" // Gris
        };
    }

    public class CheckInRequest
    {
        [JsonProperty("visitId")]
        public int VisitId { get; set; }

        [JsonProperty("gateId")]
        public int GateId { get; set; }

        [JsonProperty("notes")]
        public string? Notes { get; set; }
    }

    public class CheckOutRequest
    {
        [JsonProperty("visitId")]
        public int VisitId { get; set; }

        [JsonProperty("gateId")]
        public int GateId { get; set; }

        [JsonProperty("notes")]
        public string? Notes { get; set; }
    }
}
