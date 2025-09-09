namespace Park.Comun.DTOs
{
    public class ColaboradorByCentroDto
    {
        public int Id { get; set; }
        public int IdCentro { get; set; }
        public int IdColaborador { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        // Propiedades de navegación
        public CentroDto? Centro { get; set; }
        public ColaboradorDto? Colaborador { get; set; }
    }

    public class CreateColaboradorByCentroDto
    {
        public int IdCentro { get; set; }
        public int IdColaborador { get; set; }
    }

    public class UpdateColaboradorByCentroDto
    {
        public int Id { get; set; }
        public int IdCentro { get; set; }
        public int IdColaborador { get; set; }
        public bool IsActive { get; set; }
    }
}
