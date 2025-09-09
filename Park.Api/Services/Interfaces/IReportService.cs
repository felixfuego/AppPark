using Park.Comun.DTOs;

namespace Park.Api.Services.Interfaces
{
    public interface IReportService
    {
        Task<DashboardStatsDto> GetDashboardStatsAsync();
        Task<IEnumerable<VisitasPorPeriodoDto>> GetVisitasPorPeriodoAsync(DateTime fechaInicio, DateTime fechaFin);
        Task<IEnumerable<ColaboradoresPorCompaniaDto>> GetColaboradoresPorCompaniaAsync();
        Task<IEnumerable<CentrosMasVisitadosDto>> GetCentrosMasVisitadosAsync(int? top = 10);
        Task<IEnumerable<TiposTransporteDto>> GetTiposTransporteAsync(DateTime? fechaInicio = null, DateTime? fechaFin = null);
        Task<IEnumerable<TiposVisitaDto>> GetTiposVisitaAsync(DateTime? fechaInicio = null, DateTime? fechaFin = null);
        Task<IEnumerable<ActividadPorHoraDto>> GetActividadPorHoraAsync(DateTime fecha);
        Task<IEnumerable<VisitantesFrecuentesDto>> GetVisitantesFrecuentesAsync(int? top = 10);
        Task<RendimientoSistemaDto> GetRendimientoSistemaAsync();
        Task<byte[]> ExportarReporteVisitasAsync(ReporteRequestDto request);
        Task<byte[]> ExportarReporteColaboradoresAsync(ReporteRequestDto request);
    }
}
