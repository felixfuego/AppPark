using Park.Comun.DTOs;

namespace Park.Api.Services.Interfaces
{
    public interface IAuditService
    {
        Task LogActionAsync(CreateAuditLogDto auditLog);
        Task LogCreateAsync(string entidad, int? idEntidad, string descripcion, int? idUsuario, string? ipAddress = null, string? userAgent = null);
        Task LogUpdateAsync(string entidad, int? idEntidad, string descripcion, string datosAnteriores, string datosNuevos, int? idUsuario, string? ipAddress = null, string? userAgent = null);
        Task LogDeleteAsync(string entidad, int? idEntidad, string descripcion, int? idUsuario, string? ipAddress = null, string? userAgent = null);
        Task LogLoginAsync(int userId, string ipAddress, string userAgent, bool exitoso = true, string? errorMessage = null);
        Task LogLogoutAsync(int userId, string ipAddress, string userAgent);
        Task LogFailedLoginAsync(string username, string ipAddress, string userAgent, string errorMessage);
        Task LogPasswordChangeAsync(int userId, string ipAddress, string userAgent);
        Task LogPermissionChangeAsync(int userId, string descripcion, int? idUsuario, string? ipAddress = null, string? userAgent = null);
        Task LogSystemActionAsync(string accion, string descripcion, bool exitoso = true, string? errorMessage = null);
        Task<IEnumerable<AuditLogDto>> GetAuditLogsAsync(AuditLogSearchDto searchDto);
        Task<PagedResultDto<AuditLogDto>> SearchAuditLogsAsync(AuditLogSearchDto searchDto);
        Task<AuditStatsDto> GetAuditStatsAsync();
        Task<IEnumerable<AuditReportDto>> GetAuditReportAsync(DateTime fechaInicio, DateTime fechaFin);
        Task<bool> CleanOldAuditLogsAsync(int diasRetencion = 90);
    }
}
