namespace Park.Comun.DTOs
{
    public class GateDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string GateNumber { get; set; } = string.Empty;
        public string GateType { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int ZoneId { get; set; }
        public string ZoneName { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public int VisitsCount { get; set; }
        public int GuardsCount { get; set; }
        public ZoneDto? Zone { get; set; }
    }

    public class CreateGateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string GateNumber { get; set; } = string.Empty;
        public string GateType { get; set; } = string.Empty;
        public int ZoneId { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class UpdateGateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string GateNumber { get; set; } = string.Empty;
        public string GateType { get; set; } = string.Empty;
        public int ZoneId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}