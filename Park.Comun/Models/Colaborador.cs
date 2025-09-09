namespace Park.Comun.Models
{
    public class Colaborador : BaseEntity
    {
        public int IdCompania { get; set; }
        public string Identidad { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Puesto { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Tel1 { get; set; } = string.Empty;
        public string Tel2 { get; set; } = string.Empty;
        public string Tel3 { get; set; } = string.Empty;
        public string PlacaVehiculo { get; set; } = string.Empty;
        public string Comentario { get; set; } = string.Empty;
        public bool IsBlackList { get; set; } = false;
        public new bool IsActive { get; set; } = true;
        
        // Navigation properties
        public virtual Company Compania { get; set; } = null!;
        public virtual ICollection<ColaboradorByCentro> ColaboradorByCentros { get; set; } = new List<ColaboradorByCentro>();
        public virtual ICollection<User> Users { get; set; } = new List<User>();
        public virtual ICollection<Visita> Visitas { get; set; } = new List<Visita>();
    }
}
