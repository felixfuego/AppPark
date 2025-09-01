namespace Park.Comun.DTOs
{
    public class RoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateRoleDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class UpdateRoleDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    public class AssignRoleDto
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }

    public class RemoveRoleDto
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
}
