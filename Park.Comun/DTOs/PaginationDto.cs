using System.ComponentModel.DataAnnotations;

namespace Park.Comun.DTOs
{
    /// <summary>
    /// DTO para paginación
    /// </summary>
    public class PaginationDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "La página debe ser mayor a 0")]
        public int Page { get; set; } = 1;
        
        [Range(1, 100, ErrorMessage = "El tamaño de página debe estar entre 1 y 100")]
        public int PageSize { get; set; } = 10;
        
        public string? SortBy { get; set; }
        public bool SortDescending { get; set; } = false;
    }

    /// <summary>
    /// DTO para respuesta paginada
    /// </summary>
    /// <typeparam name="T">Tipo de datos</typeparam>
    public class PagedResultDto<T>
    {
        public IEnumerable<T> Data { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasNextPage => Page < TotalPages;
        public bool HasPreviousPage => Page > 1;
    }

    /// <summary>
    /// DTO para búsqueda avanzada de visitas
    /// </summary>
    public class VisitaSearchDto : PaginationDto
    {
        public string? NumeroSolicitud { get; set; }
        public string? IdentidadVisitante { get; set; }
        public string? NombreCompleto { get; set; }
        public int? IdCompania { get; set; }
        public int? IdCentro { get; set; }
        public int? IdColaborador { get; set; }
        public int? IdSolicitante { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string? Estado { get; set; }
        public string? TipoVisita { get; set; }
        public string? TipoTransporte { get; set; }
        public bool? EsVisitaMasiva { get; set; }
        public string? SearchTerm { get; set; }
    }

    /// <summary>
    /// DTO para búsqueda avanzada de colaboradores
    /// </summary>
    public class ColaboradorSearchDto : PaginationDto
    {
        public string? Identidad { get; set; }
        public string? Nombre { get; set; }
        public string? Puesto { get; set; }
        public string? Email { get; set; }
        public int? IdCompania { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsBlackList { get; set; }
    }

    /// <summary>
    /// DTO para búsqueda avanzada de compañías
    /// </summary>
    public class CompanySearchDto : PaginationDto
    {
        public string? Name { get; set; }
        public string? ContactPerson { get; set; }
        public string? Email { get; set; }
        public int? IdSitio { get; set; }
        public bool? IsActive { get; set; }
    }

    /// <summary>
    /// DTO para búsqueda avanzada de centros
    /// </summary>
    public class CentroSearchDto : PaginationDto
    {
        public string? Nombre { get; set; }
        public string? Localidad { get; set; }
        public int? IdZona { get; set; }
        public bool? IsActive { get; set; }
    }

    /// <summary>
    /// DTO para búsqueda avanzada de zonas
    /// </summary>
    public class ZonaSearchDto : PaginationDto
    {
        public string? Nombre { get; set; }
        public int? IdSitio { get; set; }
        public bool? IsActive { get; set; }
    }

    /// <summary>
    /// DTO para búsqueda avanzada de sitios
    /// </summary>
    public class SitioSearchDto : PaginationDto
    {
        public string? Nombre { get; set; }
        public bool? IsActive { get; set; }
    }
}
