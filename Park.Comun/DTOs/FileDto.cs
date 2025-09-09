using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Park.Comun.DTOs
{
    /// <summary>
    /// DTO para archivos
    /// </summary>
    public class FileDto
    {
        public int Id { get; set; }
        public string NombreOriginal { get; set; } = string.Empty;
        public string NombreArchivo { get; set; } = string.Empty;
        public string RutaArchivo { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;
        public long TamañoBytes { get; set; }
        public string TipoMime { get; set; } = string.Empty;
        public TipoArchivo Tipo { get; set; }
        public string? Descripcion { get; set; }
        public int? IdEntidad { get; set; }
        public string? Entidad { get; set; }
        public int? IdUsuario { get; set; }
        public bool IsActive { get; set; }
        public DateTime FechaSubida { get; set; }
        public DateTime? FechaEliminacion { get; set; }
        
        // Propiedades de navegación
        public UserDto? Usuario { get; set; }
    }

    /// <summary>
    /// DTO para subir archivos
    /// </summary>
    public class UploadFileDto
    {
        [Required(ErrorMessage = "El archivo es obligatorio")]
        public IFormFile Archivo { get; set; } = null!;
        
        [Required(ErrorMessage = "El tipo de archivo es obligatorio")]
        public TipoArchivo Tipo { get; set; }
        
        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        public string? Descripcion { get; set; }
        
        public int? IdEntidad { get; set; }
        public string? Entidad { get; set; }
    }

    /// <summary>
    /// DTO para búsqueda de archivos
    /// </summary>
    public class FileSearchDto : PaginationDto
    {
        public string? NombreOriginal { get; set; }
        public string? Extension { get; set; }
        public TipoArchivo? Tipo { get; set; }
        public int? IdUsuario { get; set; }
        public int? IdEntidad { get; set; }
        public string? Entidad { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public long? TamañoMinimo { get; set; }
        public long? TamañoMaximo { get; set; }
    }

    /// <summary>
    /// DTO para estadísticas de archivos
    /// </summary>
    public class FileStatsDto
    {
        public int TotalArchivos { get; set; }
        public long TamañoTotalBytes { get; set; }
        public int ArchivosHoy { get; set; }
        public int ArchivosEstaSemana { get; set; }
        public int ArchivosEsteMes { get; set; }
        public Dictionary<string, int> ArchivosPorTipo { get; set; } = new();
        public Dictionary<string, int> ArchivosPorExtension { get; set; } = new();
        public Dictionary<string, long> TamañoPorTipo { get; set; } = new();
        public DateTime UltimaSubida { get; set; }
    }

    /// <summary>
    /// DTO para información de archivo
    /// </summary>
    public class FileInfoDto
    {
        public string NombreOriginal { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;
        public long TamañoBytes { get; set; }
        public string TipoMime { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public bool Existe { get; set; }
    }

    /// <summary>
    /// DTO para respuesta de subida de archivo
    /// </summary>
    public class FileUploadResponseDto
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public FileDto? Archivo { get; set; }
        public string? Error { get; set; }
    }
}

/// <summary>
/// Tipos de archivos permitidos
/// </summary>
public enum TipoArchivo
{
    Documento = 1,
    Imagen = 2,
    Video = 3,
    Audio = 4,
    Archivo = 5,
    QRCode = 6,
    Reporte = 7,
    Backup = 8,
    Log = 9,
    Configuracion = 10
}

/// <summary>
/// Extensiones de archivo permitidas
/// </summary>
public static class ExtensionesPermitidas
{
    public static readonly string[] Imagenes = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
    public static readonly string[] Documentos = { ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".txt" };
    public static readonly string[] Videos = { ".mp4", ".avi", ".mov", ".wmv", ".flv", ".webm" };
    public static readonly string[] Audio = { ".mp3", ".wav", ".ogg", ".aac", ".flac" };
    public static readonly string[] Archivos = { ".zip", ".rar", ".7z", ".tar", ".gz" };
    
    public static readonly Dictionary<TipoArchivo, string[]> PorTipo = new()
    {
        { TipoArchivo.Imagen, Imagenes },
        { TipoArchivo.Documento, Documentos },
        { TipoArchivo.Video, Videos },
        { TipoArchivo.Audio, Audio },
        { TipoArchivo.Archivo, Archivos }
    };
}
