namespace Park.Comun.Models
{
    public class Visit : BaseEntity
    {
        public string VisitCode { get; set; } = string.Empty; // Código único de la visita
        public string Purpose { get; set; } = string.Empty; // Propósito de la visita
        public string Status { get; set; } = string.Empty; // Pending, InProgress, Completed, Cancelled
        public DateTime ScheduledDate { get; set; }
        public DateTime? EntryTime { get; set; }
        public DateTime? ExitTime { get; set; }
        public string Notes { get; set; } = string.Empty;
        public new bool IsActive { get; set; } = true;
        
        // Relaciones
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; } = null!;
        
        public int VisitorId { get; set; }
        public virtual Visitor Visitor { get; set; } = null!;
        
        public int GateId { get; set; }
        public virtual Gate Gate { get; set; } = null!;
        
        public int CreatedById { get; set; }
        public virtual User CreatedBy { get; set; } = null!;
    }
}
