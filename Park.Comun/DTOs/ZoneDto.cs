namespace Park.Comun.DTOs
{
    public class ZoneDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int GatesCount { get; set; }
        public int CompaniesCount { get; set; }
    }

    public class CreateZoneDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public int CompanyId { get; set; }
        public int Capacity { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class UpdateZoneDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public int CompanyId { get; set; }
        public int Capacity { get; set; }
        public bool IsActive { get; set; }
    }
}
