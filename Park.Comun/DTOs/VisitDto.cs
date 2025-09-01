using Park.Comun.Enums;

namespace Park.Comun.DTOs
{
    public class VisitDto
    {
        public int Id { get; set; }
        public string VisitCode { get; set; } = string.Empty;
        public string Purpose { get; set; } = string.Empty;
        public VisitStatus Status { get; set; }
        public DateTime ScheduledDate { get; set; }
        public DateTime? EntryTime { get; set; }
        public DateTime? ExitTime { get; set; }
        // Propiedades de compatibilidad con frontend
        public DateTime? CheckInTime => EntryTime;
        public DateTime? CheckOutTime => ExitTime;
        public string Notes { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CompanyId { get; set; }
        public int VisitorId { get; set; }
        public int GateId { get; set; }
        public CompanyDto Company { get; set; } = null!;
        public VisitorDto Visitor { get; set; } = null!;
        public GateDto Gate { get; set; } = null!;
        public UserDto CreatedBy { get; set; } = null!;
        public string QRCode { get; set; } = string.Empty; // Base64 del QR
    }

    public class CreateVisitDto
    {
        public string Purpose { get; set; } = string.Empty;
        public DateTime ScheduledDate { get; set; }
        public string Notes { get; set; } = string.Empty;
        public int CompanyId { get; set; }
        public int VisitorId { get; set; }
        public int GateId { get; set; }
    }

    public class UpdateVisitDto
    {
        public string Purpose { get; set; } = string.Empty;
        public DateTime ScheduledDate { get; set; }
        public string Notes { get; set; } = string.Empty;
        public int CompanyId { get; set; }
        public int VisitorId { get; set; }
        public int GateId { get; set; }
        public bool IsActive { get; set; }
    }

    public class VisitCheckInDto
    {
        public string VisitCode { get; set; } = string.Empty;
        public string QRCode { get; set; } = string.Empty; // QR escaneado
        public int? GuardId { get; set; } // ID del guardia que realiza el check-in
        public int? GateId { get; set; } // ID del portón donde se realiza el check-in
    }

    public class VisitCheckOutDto
    {
        public string VisitCode { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public int? GuardId { get; set; } // ID del guardia que realiza el check-out
        public int? GateId { get; set; } // ID del portón donde se realiza el check-out
    }

    public class VisitStatusDto
    {
        public VisitStatus Status { get; set; } // Pending, InProgress, Completed, Cancelled
        public string Notes { get; set; } = string.Empty;
    }

    public class QRCodeData
    {
        public string VisitCode { get; set; } = string.Empty;
        public string VisitorName { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public DateTime ScheduledDate { get; set; }
        public string GateId { get; set; } = string.Empty;
        public string SecurityHash { get; set; } = string.Empty;
    }
}
