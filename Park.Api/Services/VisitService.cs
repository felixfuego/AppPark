using Microsoft.EntityFrameworkCore;
using Park.Api.Data;
using Park.Api.Services.Interfaces;
using Park.Comun.DTOs;
using Park.Comun.Models;
using Park.Comun.Enums;

namespace Park.Api.Services
{
    public class VisitService : IVisitService
    {
        private readonly ParkDbContext _context;
        private readonly IQRService _qrService;

        public VisitService(ParkDbContext context, IQRService qrService)
        {
            _context = context;
            _qrService = qrService;
        }

        public async Task<IEnumerable<VisitDto>> GetAllVisitsAsync()
        {
            var visits = await _context.Visits
                .Include(v => v.Company)
                    .ThenInclude(c => c.Zone)
                .Include(v => v.Visitor)
                .Include(v => v.Gate)
                    .ThenInclude(g => g.Zone)
                .Include(v => v.CreatedBy)
                .Where(v => v.IsActive)
                .OrderByDescending(v => v.ScheduledDate)
                .ToListAsync();

            return visits.Select(MapToDto);
        }

        public async Task<IEnumerable<VisitDto>> GetVisitsByUserPermissionsAsync(int userId)
        {
            // Obtener empresas y zonas asignadas al usuario
            var userCompanies = await _context.UserCompanies
                .Where(uc => uc.UserId == userId && uc.IsActive)
                .Select(uc => uc.CompanyId)
                .ToListAsync();

            var userZones = await _context.UserZones
                .Where(uz => uz.UserId == userId && uz.IsActive)
                .Select(uz => uz.ZoneId)
                .ToListAsync();

            // Obtener portones en las zonas asignadas al usuario
            var gatesInUserZones = await _context.Gates
                .Where(g => userZones.Contains(g.ZoneId) && g.IsActive)
                .Select(g => g.Id)
                .ToListAsync();

            // Filtrar visitas según los permisos del usuario
            var visits = await _context.Visits
                .Include(v => v.Company)
                    .ThenInclude(c => c.Zone)
                .Include(v => v.Visitor)
                .Include(v => v.Gate)
                    .ThenInclude(g => g.Zone)
                .Include(v => v.CreatedBy)
                .Where(v => v.IsActive && 
                           (userCompanies.Contains(v.CompanyId) || 
                            gatesInUserZones.Contains(v.GateId)))
                .OrderByDescending(v => v.ScheduledDate)
                .ToListAsync();

            return visits.Select(MapToDto);
        }

        public async Task<VisitDto?> GetVisitByIdAsync(int id)
        {
            var visit = await _context.Visits
                .Include(v => v.Company)
                    .ThenInclude(c => c.Zone)
                .Include(v => v.Visitor)
                .Include(v => v.Gate)
                    .ThenInclude(g => g.Zone)
                .Include(v => v.CreatedBy)
                .FirstOrDefaultAsync(v => v.Id == id && v.IsActive);

            return visit != null ? MapToDto(visit) : null;
        }

        public async Task<VisitDto?> GetVisitByCodeAsync(string visitCode)
        {
            var visit = await _context.Visits
                .Include(v => v.Company)
                    .ThenInclude(c => c.Zone)
                .Include(v => v.Visitor)
                .Include(v => v.Gate)
                    .ThenInclude(g => g.Zone)
                .Include(v => v.CreatedBy)
                .FirstOrDefaultAsync(v => v.VisitCode == visitCode && v.IsActive);

            return visit != null ? MapToDto(visit) : null;
        }

        public async Task<VisitDto> CreateVisitAsync(CreateVisitDto createVisitDto, int createdById)
        {
            // Verificar si la empresa existe
            var company = await _context.Companies.FindAsync(createVisitDto.CompanyId);
            if (company == null || !company.IsActive)
            {
                throw new InvalidOperationException("La empresa especificada no existe o no está activa.");
            }

            // Verificar si el visitante existe
            var visitor = await _context.Visitors.FindAsync(createVisitDto.VisitorId);
            if (visitor == null || !visitor.IsActive)
            {
                throw new InvalidOperationException("El visitante especificado no existe o no está activo.");
            }

            // Verificar si el portón existe
            var gate = await _context.Gates.FindAsync(createVisitDto.GateId);
            if (gate == null || !gate.IsActive)
            {
                throw new InvalidOperationException("El portón especificado no existe o no está activo.");
            }

            // Generar código único de visita
            var visitCode = await GenerateUniqueVisitCodeAsync();

            var visit = new Visit
            {
                VisitCode = visitCode,
                Purpose = createVisitDto.Purpose,
                Status = "Pending",
                ScheduledDate = createVisitDto.ScheduledDate,
                Notes = createVisitDto.Notes,
                CompanyId = createVisitDto.CompanyId,
                VisitorId = createVisitDto.VisitorId,
                GateId = createVisitDto.GateId,
                CreatedById = createdById,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Visits.Add(visit);
            await _context.SaveChangesAsync();

            return await GetVisitByIdAsync(visit.Id) ?? MapToDto(visit);
        }

        public async Task<VisitDto?> UpdateVisitAsync(int id, UpdateVisitDto updateVisitDto)
        {
            var visit = await _context.Visits.FindAsync(id);

            if (visit == null || !visit.IsActive)
            {
                return null;
            }

            // Verificar si la empresa existe
            var company = await _context.Companies.FindAsync(updateVisitDto.CompanyId);
            if (company == null || !company.IsActive)
            {
                throw new InvalidOperationException("La empresa especificada no existe o no está activa.");
            }

            // Verificar si el visitante existe
            var visitor = await _context.Visitors.FindAsync(updateVisitDto.VisitorId);
            if (visitor == null || !visitor.IsActive)
            {
                throw new InvalidOperationException("El visitante especificado no existe o no está activo.");
            }

            // Verificar si el portón existe
            var gate = await _context.Gates.FindAsync(updateVisitDto.GateId);
            if (gate == null || !gate.IsActive)
            {
                throw new InvalidOperationException("El portón especificado no existe o no está activo.");
            }

            visit.Purpose = updateVisitDto.Purpose;
            visit.ScheduledDate = updateVisitDto.ScheduledDate;
            visit.Notes = updateVisitDto.Notes;
            visit.CompanyId = updateVisitDto.CompanyId;
            visit.VisitorId = updateVisitDto.VisitorId;
            visit.GateId = updateVisitDto.GateId;
            visit.IsActive = updateVisitDto.IsActive;
            visit.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return await GetVisitByIdAsync(id);
        }

        public async Task<bool> DeleteVisitAsync(int id)
        {
            var visit = await _context.Visits.FindAsync(id);

            if (visit == null || !visit.IsActive)
            {
                return false;
            }

            visit.IsActive = false;
            visit.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> VisitExistsAsync(int id)
        {
            return await _context.Visits.AnyAsync(v => v.Id == id && v.IsActive);
        }

        public async Task<bool> VisitCodeExistsAsync(string visitCode)
        {
            return await _context.Visits.AnyAsync(v => v.VisitCode == visitCode && v.IsActive);
        }

        // Funcionalidad de Check-in/Check-out
        public async Task<VisitDto?> CheckInVisitAsync(VisitCheckInDto checkInDto)
        {
            var visit = await _context.Visits
                .Include(v => v.Company)
                    .ThenInclude(c => c.Zone)
                .Include(v => v.Visitor)
                .Include(v => v.Gate)
                    .ThenInclude(g => g.Zone)
                .Include(v => v.CreatedBy)
                .FirstOrDefaultAsync(v => v.VisitCode == checkInDto.VisitCode && v.IsActive);

            if (visit == null)
            {
                throw new InvalidOperationException("Visita no encontrada.");
            }

            if (visit.Status != "Pending")
            {
                throw new InvalidOperationException($"La visita no puede ser registrada. Estado actual: {visit.Status}");
            }

            // Validar que la visita esté programada para hoy
            var today = DateTime.UtcNow.Date;
            if (visit.ScheduledDate.Date != today)
            {
                throw new InvalidOperationException($"La visita está programada para {visit.ScheduledDate:dd/MM/yyyy}, no para hoy.");
            }

            // Validar QR si se proporciona
            if (!string.IsNullOrEmpty(checkInDto.QRCode))
            {
                var decodedData = await _qrService.DecodeQRCodeAsync(checkInDto.QRCode);
                if (decodedData == null || decodedData.VisitCode != visit.VisitCode)
                {
                    throw new InvalidOperationException("Código QR inválido o no corresponde a esta visita.");
                }
            }

            // Validar que el guardia esté asignado a la zona del portón (si se proporciona)
            if (checkInDto.GuardId.HasValue && checkInDto.GateId.HasValue)
            {
                var gate = await _context.Gates.FindAsync(checkInDto.GateId.Value);
                if (gate == null)
                {
                    throw new InvalidOperationException("El portón especificado no existe.");
                }

                var guardZoneAssignment = await _context.UserZones
                    .FirstOrDefaultAsync(uz => uz.UserId == checkInDto.GuardId.Value && 
                                             uz.ZoneId == gate.ZoneId && 
                                             uz.IsActive);
                
                if (guardZoneAssignment == null)
                {
                    throw new InvalidOperationException("El guardia no está asignado a la zona de este portón.");
                }
            }

            visit.Status = "InProgress";
            visit.EntryTime = DateTime.UtcNow;
            visit.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return MapToDto(visit);
        }

        public async Task<VisitDto?> CheckOutVisitAsync(VisitCheckOutDto checkOutDto)
        {
            var visit = await _context.Visits
                .Include(v => v.Company)
                    .ThenInclude(c => c.Zone)
                .Include(v => v.Visitor)
                .Include(v => v.Gate)
                    .ThenInclude(g => g.Zone)
                .Include(v => v.CreatedBy)
                .FirstOrDefaultAsync(v => v.VisitCode == checkOutDto.VisitCode && v.IsActive);

            if (visit == null)
            {
                throw new InvalidOperationException("Visita no encontrada.");
            }

            if (visit.Status != "InProgress")
            {
                throw new InvalidOperationException($"La visita no puede ser finalizada. Estado actual: {visit.Status}");
            }

            // Validar que el guardia esté asignado a la zona del portón (si se proporciona)
            if (checkOutDto.GuardId.HasValue && checkOutDto.GateId.HasValue)
            {
                var gate = await _context.Gates.FindAsync(checkOutDto.GateId.Value);
                if (gate == null)
                {
                    throw new InvalidOperationException("El portón especificado no existe.");
                }

                var guardZoneAssignment = await _context.UserZones
                    .FirstOrDefaultAsync(uz => uz.UserId == checkOutDto.GuardId.Value && 
                                             uz.ZoneId == gate.ZoneId && 
                                             uz.IsActive);
                
                if (guardZoneAssignment == null)
                {
                    throw new InvalidOperationException("El guardia no está asignado a la zona de este portón.");
                }
            }

            visit.Status = "Completed";
            visit.ExitTime = DateTime.UtcNow;
            visit.Notes = string.IsNullOrEmpty(visit.Notes) ? checkOutDto.Notes : $"{visit.Notes}; {checkOutDto.Notes}";
            visit.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return MapToDto(visit);
        }

        public async Task<VisitDto?> UpdateVisitStatusAsync(int id, VisitStatusDto statusDto)
        {
            var visit = await _context.Visits
                .Include(v => v.Company)
                    .ThenInclude(c => c.Zone)
                .Include(v => v.Visitor)
                .Include(v => v.Gate)
                    .ThenInclude(g => g.Zone)
                .Include(v => v.CreatedBy)
                .FirstOrDefaultAsync(v => v.Id == id && v.IsActive);

            if (visit == null)
            {
                return null;
            }

            visit.Status = statusDto.Status.ToString();
            visit.Notes = string.IsNullOrEmpty(visit.Notes) ? statusDto.Notes : $"{visit.Notes}; {statusDto.Notes}";
            visit.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return MapToDto(visit);
        }

        // Consultas por filtros
        public async Task<IEnumerable<VisitDto>> GetVisitsByCompanyAsync(int companyId)
        {
            var visits = await _context.Visits
                .Include(v => v.Company)
                    .ThenInclude(c => c.Zone)
                .Include(v => v.Visitor)
                .Include(v => v.Gate)
                    .ThenInclude(g => g.Zone)
                .Include(v => v.CreatedBy)
                .Where(v => v.CompanyId == companyId && v.IsActive)
                .OrderByDescending(v => v.ScheduledDate)
                .ToListAsync();

            return visits.Select(MapToDto);
        }

        public async Task<IEnumerable<VisitDto>> GetVisitsByGateAsync(int gateId)
        {
            var visits = await _context.Visits
                .Include(v => v.Company)
                    .ThenInclude(c => c.Zone)
                .Include(v => v.Visitor)
                .Include(v => v.Gate)
                    .ThenInclude(g => g.Zone)
                .Include(v => v.CreatedBy)
                .Where(v => v.GateId == gateId && v.IsActive)
                .OrderByDescending(v => v.ScheduledDate)
                .ToListAsync();

            return visits.Select(MapToDto);
        }

        public async Task<IEnumerable<VisitDto>> GetVisitsByStatusAsync(string status)
        {
            var visits = await _context.Visits
                .Include(v => v.Company)
                    .ThenInclude(c => c.Zone)
                .Include(v => v.Visitor)
                .Include(v => v.Gate)
                    .ThenInclude(g => g.Zone)
                .Include(v => v.CreatedBy)
                .Where(v => v.Status == status && v.IsActive)
                .OrderByDescending(v => v.ScheduledDate)
                .ToListAsync();

            return visits.Select(MapToDto);
        }

        public async Task<IEnumerable<VisitDto>> GetVisitsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var visits = await _context.Visits
                .Include(v => v.Company)
                    .ThenInclude(c => c.Zone)
                .Include(v => v.Visitor)
                .Include(v => v.Gate)
                    .ThenInclude(g => g.Zone)
                .Include(v => v.CreatedBy)
                .Where(v => v.ScheduledDate >= startDate && v.ScheduledDate <= endDate && v.IsActive)
                .OrderByDescending(v => v.ScheduledDate)
                .ToListAsync();

            return visits.Select(MapToDto);
        }

        public async Task<IEnumerable<VisitDto>> GetVisitsByVisitorAsync(int visitorId)
        {
            var visits = await _context.Visits
                .Include(v => v.Company)
                    .ThenInclude(c => c.Zone)
                .Include(v => v.Visitor)
                .Include(v => v.Gate)
                    .ThenInclude(g => g.Zone)
                .Include(v => v.CreatedBy)
                .Where(v => v.VisitorId == visitorId && v.IsActive)
                .OrderByDescending(v => v.ScheduledDate)
                .ToListAsync();

            return visits.Select(MapToDto);
        }

        public async Task<IEnumerable<VisitDto>> GetVisitsByCreatorAsync(int createdById)
        {
            var visits = await _context.Visits
                .Include(v => v.Company)
                    .ThenInclude(c => c.Zone)
                .Include(v => v.Visitor)
                .Include(v => v.Gate)
                    .ThenInclude(g => g.Zone)
                .Include(v => v.CreatedBy)
                .Where(v => v.CreatedById == createdById && v.IsActive)
                .OrderByDescending(v => v.ScheduledDate)
                .ToListAsync();

            return visits.Select(MapToDto);
        }

        // Funcionalidad de QR
        public async Task<string> GenerateQRCodeAsync(int visitId)
        {
            var visit = await _context.Visits
                .Include(v => v.Company)
                    .ThenInclude(c => c.Zone)
                .Include(v => v.Visitor)
                .Include(v => v.Gate)
                    .ThenInclude(g => g.Zone)
                .FirstOrDefaultAsync(v => v.Id == visitId && v.IsActive);

            if (visit == null)
            {
                throw new InvalidOperationException("Visita no encontrada.");
            }

            var qrData = new QRCodeData
            {
                VisitCode = visit.VisitCode,
                VisitorName = $"{visit.Visitor.FirstName} {visit.Visitor.LastName}",
                CompanyName = visit.Company.Name,
                ScheduledDate = visit.ScheduledDate,
                GateId = visit.Gate.GateNumber,
                SecurityHash = await _qrService.GenerateSecurityHashAsync(new QRCodeData
                {
                    VisitCode = visit.VisitCode,
                    VisitorName = $"{visit.Visitor.FirstName} {visit.Visitor.LastName}",
                    CompanyName = visit.Company.Name,
                    ScheduledDate = visit.ScheduledDate,
                    GateId = visit.Gate.GateNumber,
                    SecurityHash = string.Empty
                })
            };

            return await _qrService.GenerateQRCodeBase64Async(qrData);
        }

        public async Task<bool> ValidateQRCodeAsync(string qrCodeData)
        {
            return await _qrService.ValidateQRCodeAsync(qrCodeData, "");
        }

        public async Task<VisitDto?> GetVisitByQRCodeAsync(string qrCodeData)
        {
            var decodedData = await _qrService.DecodeQRCodeAsync(qrCodeData);
            if (decodedData == null)
            {
                return null;
            }

            return await GetVisitByCodeAsync(decodedData.VisitCode);
        }

        private async Task<string> GenerateUniqueVisitCodeAsync()
        {
            string visitCode;
            do
            {
                visitCode = $"VIS{DateTime.UtcNow:yyyyMMdd}{Random.Shared.Next(1000, 9999)}";
            } while (await VisitCodeExistsAsync(visitCode));

            return visitCode;
        }

        private VisitDto MapToDto(Visit visit)
        {
            // Mapear el estado de string a enum de forma segura
            VisitStatus status;
            if (!Enum.TryParse<VisitStatus>(visit.Status, out status))
            {
                // Si no se puede parsear, usar Pending como valor por defecto
                status = VisitStatus.Pending;
            }

            return new VisitDto
            {
                Id = visit.Id,
                VisitCode = visit.VisitCode,
                Purpose = visit.Purpose,
                Status = status,
                ScheduledDate = visit.ScheduledDate,
                EntryTime = visit.EntryTime,
                ExitTime = visit.ExitTime,
                Notes = visit.Notes,
                IsActive = visit.IsActive,
                CreatedAt = visit.CreatedAt,
                Company = new CompanyDto
                {
                    Id = visit.Company.Id,
                    Name = visit.Company.Name,
                    Description = visit.Company.Description,
                    Address = visit.Company.Address,
                    Phone = visit.Company.Phone,
                    Email = visit.Company.Email,
                    ContactPerson = visit.Company.ContactPerson,
                    ContactPhone = visit.Company.ContactPhone,
                    ContactEmail = visit.Company.ContactEmail,
                    IsActive = visit.Company.IsActive,
                    CreatedAt = visit.Company.CreatedAt,
                    Zone = new ZoneDto
                    {
                        Id = visit.Company.Zone.Id,
                        Name = visit.Company.Zone.Name,
                        Description = visit.Company.Zone.Description,
                        IsActive = visit.Company.Zone.IsActive,
                        CreatedAt = visit.Company.Zone.CreatedAt,
                        CompaniesCount = 0,
                        GatesCount = 0
                    },
                    VisitsCount = 0
                },
                Visitor = new VisitorDto
                {
                    Id = visit.Visitor.Id,
                    FirstName = visit.Visitor.FirstName,
                    LastName = visit.Visitor.LastName,
                    Email = visit.Visitor.Email,
                    Phone = visit.Visitor.Phone,
                    DocumentType = visit.Visitor.DocumentType,
                    DocumentNumber = visit.Visitor.DocumentNumber,
                    Company = visit.Visitor.Company,
                    IsActive = visit.Visitor.IsActive,
                    CreatedAt = visit.Visitor.CreatedAt,
                    VisitsCount = 0
                },
                Gate = new GateDto
                {
                    Id = visit.Gate.Id,
                    Name = visit.Gate.Name,
                    Description = visit.Gate.Description,
                    GateNumber = visit.Gate.GateNumber,
                    IsActive = visit.Gate.IsActive,
                    CreatedAt = visit.Gate.CreatedAt,
                    Zone = new ZoneDto
                    {
                        Id = visit.Gate.Zone.Id,
                        Name = visit.Gate.Zone.Name,
                        Description = visit.Gate.Zone.Description,
                        IsActive = visit.Gate.Zone.IsActive,
                        CreatedAt = visit.Gate.Zone.CreatedAt,
                        CompaniesCount = 0,
                        GatesCount = 0
                    },
                    VisitsCount = 0,
                    GuardsCount = 0
                },
                CreatedBy = new UserDto
                {
                    Id = visit.CreatedBy.Id,
                    Username = visit.CreatedBy.Username,
                    Email = visit.CreatedBy.Email,
                    FirstName = visit.CreatedBy.FirstName,
                    LastName = visit.CreatedBy.LastName,
                    Roles = new List<RoleDto>(),
                    IsActive = visit.CreatedBy.IsActive,
                    LastLogin = visit.CreatedBy.LastLogin,
                    CreatedAt = visit.CreatedBy.CreatedAt
                },
                QRCode = string.Empty // Se generará cuando se solicite específicamente
            };
        }
    }
}
