using Microsoft.EntityFrameworkCore;
using Park.Api.Data;
using Park.Api.Services.Interfaces;
using Park.Comun.DTOs;
using Park.Comun.Enums;
using System.Globalization;

namespace Park.Api.Services
{
    public class ReportService : IReportService
    {
        private readonly ParkDbContext _context;
        private readonly ILogger<ReportService> _logger;

        public ReportService(ParkDbContext context, ILogger<ReportService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<DashboardStatsDto> GetDashboardStatsAsync()
        {
            try
            {
                var hoy = DateTime.Today;
                var inicioMes = new DateTime(hoy.Year, hoy.Month, 1);

                var totalVisitas = await _context.Visitas.CountAsync();
                var visitasActivas = await _context.Visitas.CountAsync(v => v.Estado == VisitStatus.EnProceso);
                var visitasExpiradas = await _context.Visitas.CountAsync(v => v.Estado == VisitStatus.Expirada);
                var visitasHoy = await _context.Visitas.CountAsync(v => v.Fecha.Date == hoy);
                var totalColaboradores = await _context.Colaboradores.CountAsync();
                var totalCompanias = await _context.Companies.CountAsync();
                var totalCentros = await _context.Centros.CountAsync();
                var colaboradoresBlackList = await _context.Colaboradores.CountAsync(c => c.IsBlackList);

                return new DashboardStatsDto
                {
                    TotalVisitas = totalVisitas,
                    VisitasActivas = visitasActivas,
                    VisitasExpiradas = visitasExpiradas,
                    VisitasHoy = visitasHoy,
                    TotalColaboradores = totalColaboradores,
                    TotalCompanias = totalCompanias,
                    TotalCentros = totalCentros,
                    ColaboradoresBlackList = colaboradoresBlackList,
                    UltimaActualizacion = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener estadísticas del dashboard");
                throw;
            }
        }

        public async Task<IEnumerable<VisitasPorPeriodoDto>> GetVisitasPorPeriodoAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                var visitas = await _context.Visitas
                    .Where(v => v.Fecha.Date >= fechaInicio.Date && v.Fecha.Date <= fechaFin.Date)
                    .GroupBy(v => v.Fecha.Date)
                    .Select(g => new VisitasPorPeriodoDto
                    {
                        Fecha = g.Key,
                        TotalVisitas = g.Count(),
                        VisitasProgramadas = g.Count(v => v.Estado == VisitStatus.Programada),
                        VisitasEnProgreso = g.Count(v => v.Estado == VisitStatus.EnProceso),
                        VisitasCompletadas = g.Count(v => v.Estado == VisitStatus.Terminada),
                        VisitasCanceladas = g.Count(v => v.Estado == VisitStatus.Cancelada),
                        VisitasExpiradas = g.Count(v => v.Estado == VisitStatus.Expirada)
                    })
                    .OrderBy(r => r.Fecha)
                    .ToListAsync();

                return visitas;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visitas por período");
                throw;
            }
        }

        public async Task<IEnumerable<ColaboradoresPorCompaniaDto>> GetColaboradoresPorCompaniaAsync()
        {
            try
            {
                var colaboradoresPorCompania = await _context.Companies
                    .Select(c => new ColaboradoresPorCompaniaDto
                    {
                        IdCompania = c.Id,
                        NombreCompania = c.Name,
                        TotalColaboradores = c.Colaboradores.Count(),
                        ColaboradoresActivos = c.Colaboradores.Count(col => col.IsActive),
                        ColaboradoresBlackList = c.Colaboradores.Count(col => col.IsBlackList),
                        TotalVisitas = c.Colaboradores.SelectMany(col => col.Visitas).Count()
                    })
                    .OrderByDescending(c => c.TotalColaboradores)
                    .ToListAsync();

                return colaboradoresPorCompania;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener colaboradores por compañía");
                throw;
            }
        }

        public async Task<IEnumerable<CentrosMasVisitadosDto>> GetCentrosMasVisitadosAsync(int? top = 10)
        {
            try
            {
                var centrosMasVisitados = await _context.Centros
                    .Select(c => new CentrosMasVisitadosDto
                    {
                        IdCentro = c.Id,
                        NombreCentro = c.Nombre,
                        Localidad = c.Localidad,
                        NombreZona = c.Zona.Nombre,
                        TotalVisitas = c.Visitas.Count(),
                        VisitasActivas = c.Visitas.Count(v => v.Estado == VisitStatus.EnProceso)
                    })
                    .OrderByDescending(c => c.TotalVisitas)
                    .Take(top ?? 10)
                    .ToListAsync();

                return centrosMasVisitados;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener centros más visitados");
                throw;
            }
        }

        public async Task<IEnumerable<TiposTransporteDto>> GetTiposTransporteAsync(DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            try
            {
                var query = _context.Visitas.AsQueryable();

                if (fechaInicio.HasValue)
                    query = query.Where(v => v.Fecha.Date >= fechaInicio.Value.Date);

                if (fechaFin.HasValue)
                    query = query.Where(v => v.Fecha.Date <= fechaFin.Value.Date);

                var totalVisitas = await query.CountAsync();

                var tiposTransporte = await query
                    .GroupBy(v => v.TipoTransporte)
                    .Select(g => new TiposTransporteDto
                    {
                        TipoTransporte = g.Key.ToString(),
                        Cantidad = g.Count(),
                        Porcentaje = totalVisitas > 0 ? Math.Round((decimal)g.Count() / totalVisitas * 100, 2) : 0
                    })
                    .OrderByDescending(t => t.Cantidad)
                    .ToListAsync();

                return tiposTransporte;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener tipos de transporte");
                throw;
            }
        }

        public async Task<IEnumerable<TiposVisitaDto>> GetTiposVisitaAsync(DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            try
            {
                var query = _context.Visitas.AsQueryable();

                if (fechaInicio.HasValue)
                    query = query.Where(v => v.Fecha.Date >= fechaInicio.Value.Date);

                if (fechaFin.HasValue)
                    query = query.Where(v => v.Fecha.Date <= fechaFin.Value.Date);

                var totalVisitas = await query.CountAsync();

                var tiposVisita = await query
                    .GroupBy(v => v.TipoVisita)
                    .Select(g => new TiposVisitaDto
                    {
                        TipoVisita = g.Key.ToString(),
                        Cantidad = g.Count(),
                        Porcentaje = totalVisitas > 0 ? Math.Round((decimal)g.Count() / totalVisitas * 100, 2) : 0
                    })
                    .OrderByDescending(t => t.Cantidad)
                    .ToListAsync();

                return tiposVisita;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener tipos de visita");
                throw;
            }
        }

        public async Task<IEnumerable<ActividadPorHoraDto>> GetActividadPorHoraAsync(DateTime fecha)
        {
            try
            {
                var checkIns = await _context.Visitas
                    .Where(v => v.FechaLlegada.HasValue && v.FechaLlegada.Value.Date == fecha.Date)
                    .GroupBy(v => v.FechaLlegada.Value.Hour)
                    .Select(g => new { Hora = g.Key, Cantidad = g.Count() })
                    .ToListAsync();

                var checkOuts = await _context.Visitas
                    .Where(v => v.FechaSalida.HasValue && v.FechaSalida.Value.Date == fecha.Date)
                    .GroupBy(v => v.FechaSalida.Value.Hour)
                    .Select(g => new { Hora = g.Key, Cantidad = g.Count() })
                    .ToListAsync();

                var horas = Enumerable.Range(0, 24).Select(h => new ActividadPorHoraDto
                {
                    Hora = h,
                    CheckIns = checkIns.FirstOrDefault(c => c.Hora == h)?.Cantidad ?? 0,
                    CheckOuts = checkOuts.FirstOrDefault(c => c.Hora == h)?.Cantidad ?? 0,
                    TotalActividad = (checkIns.FirstOrDefault(c => c.Hora == h)?.Cantidad ?? 0) + 
                                   (checkOuts.FirstOrDefault(c => c.Hora == h)?.Cantidad ?? 0)
                }).ToList();

                return horas;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener actividad por hora");
                throw;
            }
        }

        public async Task<IEnumerable<VisitantesFrecuentesDto>> GetVisitantesFrecuentesAsync(int? top = 10)
        {
            try
            {
                var visitantesFrecuentes = await _context.Visitas
                    .GroupBy(v => new { v.IdentidadVisitante, v.NombreCompleto })
                    .Select(g => new VisitantesFrecuentesDto
                    {
                        IdentidadVisitante = g.Key.IdentidadVisitante,
                        NombreCompleto = g.Key.NombreCompleto,
                        TotalVisitas = g.Count(),
                        UltimaVisita = g.Max(v => v.Fecha),
                        CompaniaMasVisitada = g.Select(v => v.Compania.Name).FirstOrDefault() ?? ""
                    })
                    .OrderByDescending(v => v.TotalVisitas)
                    .Take(top ?? 10)
                    .ToListAsync();

                return visitantesFrecuentes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visitantes frecuentes");
                throw;
            }
        }

        public async Task<RendimientoSistemaDto> GetRendimientoSistemaAsync()
        {
            try
            {
                var totalUsuarios = await _context.Users.CountAsync();
                var usuariosActivos = await _context.Users.CountAsync(u => u.IsActive);
                var totalRoles = await _context.Roles.CountAsync();
                var totalSitios = await _context.Sitios.CountAsync();
                var totalZonas = await _context.Zonas.CountAsync();
                var totalCentros = await _context.Centros.CountAsync();

                return new RendimientoSistemaDto
                {
                    TotalUsuarios = totalUsuarios,
                    UsuariosActivos = usuariosActivos,
                    TotalRoles = totalRoles,
                    TotalSitios = totalSitios,
                    TotalZonas = totalZonas,
                    TotalCentros = totalCentros,
                    UltimaActualizacion = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener rendimiento del sistema");
                throw;
            }
        }

        public async Task<byte[]> ExportarReporteVisitasAsync(ReporteRequestDto request)
        {
            try
            {
                // Implementación básica - en producción usar una librería como EPPlus
                var visitas = await _context.Visitas
                    .Where(v => v.Fecha.Date >= request.FechaInicio.Date && v.Fecha.Date <= request.FechaFin.Date)
                    .Include(v => v.Solicitante)
                    .Include(v => v.Compania)
                    .Include(v => v.Centro)
                    .ToListAsync();

                // Por ahora retornamos un array vacío - implementar exportación real
                return Array.Empty<byte>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al exportar reporte de visitas");
                throw;
            }
        }

        public async Task<byte[]> ExportarReporteColaboradoresAsync(ReporteRequestDto request)
        {
            try
            {
                // Implementación básica - en producción usar una librería como EPPlus
                var colaboradores = await _context.Colaboradores
                    .Include(c => c.Compania)
                    .ToListAsync();

                // Por ahora retornamos un array vacío - implementar exportación real
                return Array.Empty<byte>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al exportar reporte de colaboradores");
                throw;
            }
        }
    }
}
