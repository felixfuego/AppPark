namespace Park.Web.Models;

public class Visit
{
    public int Id { get; set; }
    public string VisitCode { get; set; } = string.Empty;
    public string Purpose { get; set; } = string.Empty;
    public VisitStatus Status { get; set; }
    public DateTime ScheduledDate { get; set; }
    public DateTime ScheduledEndTime { get; set; }
    public DateTime? CheckInTime { get; set; }
    public DateTime? CheckOutTime { get; set; }
    public string Notes { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public int CompanyId { get; set; }
    public int VisitorId { get; set; }
    public int GateId { get; set; }
    public Company Company { get; set; } = new();
    public Visitor Visitor { get; set; } = new();
    public Gate Gate { get; set; } = new();
    public UserInfo CreatedBy { get; set; } = new();
    public string QRCode { get; set; } = string.Empty;
}

public class CreateVisit
{
    public string Purpose { get; set; } = string.Empty;
    public DateTime ScheduledDate { get; set; }
    public DateTime ScheduledEndTime { get; set; }
    public string Notes { get; set; } = string.Empty;
    public int CompanyId { get; set; }
    public int VisitorId { get; set; }
    public int GateId { get; set; }
}

public class UpdateVisit
{
    public string Purpose { get; set; } = string.Empty;
    public DateTime ScheduledDate { get; set; }
    public DateTime ScheduledEndTime { get; set; }
    public string Notes { get; set; } = string.Empty;
    public int CompanyId { get; set; }
    public int VisitorId { get; set; }
    public int GateId { get; set; }
    public bool IsActive { get; set; }
}

public class VisitCheckIn
{
    public string VisitCode { get; set; } = string.Empty;
    public string QRCode { get; set; } = string.Empty;
}

public class VisitCheckOut
{
    public string VisitCode { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}

public class VisitStatusUpdate
{
    public VisitStatus Status { get; set; }
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
