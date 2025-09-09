using Park.Comun.DTOs;
using Park.Comun.Enums;

namespace Park.Api.Services.Interfaces
{
    public interface IVisitaService
    {
        Task<IEnumerable<VisitaDto>> GetAllVisitasAsync();
        Task<VisitaDto?> GetVisitaByIdAsync(int id);
        Task<VisitaDto?> GetVisitaByNumeroSolicitudAsync(string numeroSolicitud);
        Task<IEnumerable<VisitaDto>> GetVisitasByCompaniaAsync(int idCompania);
        Task<IEnumerable<VisitaDto>> GetVisitasByColaboradorAsync(int idColaborador);
        Task<IEnumerable<VisitaDto>> GetVisitasByCentroAsync(int idCentro);
        Task<IEnumerable<VisitaDto>> GetVisitasByEstadoAsync(VisitStatus estado);
        Task<IEnumerable<VisitaDto>> GetVisitasByFechaAsync(DateTime fecha);
        Task<IEnumerable<VisitaDto>> GetVisitasByRangoFechasAsync(DateTime fechaInicio, DateTime fechaFin);
        Task<VisitaDto> CreateVisitaAsync(CreateVisitaDto createVisitaDto);
        Task<VisitaDto> UpdateVisitaAsync(UpdateVisitaDto updateVisitaDto);
        Task<bool> DeleteVisitaAsync(int id);
        Task<VisitaDto> CheckInVisitaAsync(VisitaCheckInDto checkInDto);
        Task<VisitaDto> CheckOutVisitaAsync(VisitaCheckOutDto checkOutDto);
        Task<bool> CancelarVisitaAsync(int id);
        Task<bool> ExpirarVisitasAsync();
        Task<IEnumerable<VisitaDto>> GetVisitasActivasAsync();
        Task<IEnumerable<VisitaDto>> GetVisitasExpiradasAsync();
        Task<PagedResultDto<VisitaDto>> SearchVisitasAsync(VisitaSearchDto searchDto);
    }
}
