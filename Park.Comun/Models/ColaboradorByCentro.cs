namespace Park.Comun.Models
{
    public class ColaboradorByCentro : BaseEntity
    {
        public int IdCentro { get; set; }
        public int IdColaborador { get; set; }
        public new bool IsActive { get; set; } = true;
        
        // Navigation properties
        public virtual Centro Centro { get; set; } = null!;
        public virtual Colaborador Colaborador { get; set; } = null!;
    }
}
