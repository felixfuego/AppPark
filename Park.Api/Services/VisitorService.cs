using Microsoft.EntityFrameworkCore;
using Park.Api.Data;
using Park.Api.Services.Interfaces;
using Park.Comun.DTOs;
using Park.Comun.Models;
using Park.Comun.Enums;

namespace Park.Api.Services
{
    public class VisitorService : IVisitorService
    {
        private readonly ParkDbContext _context;

        public VisitorService(ParkDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<VisitorDto>> GetAllVisitorsAsync()
        {
            var visitors = await _context.Visitors
                .Include(v => v.Visits)
                .Where(v => v.IsActive)
                .ToListAsync();

            return visitors.Select(MapToDto);
        }

        public async Task<VisitorDto?> GetVisitorByIdAsync(int id)
        {
            var visitor = await _context.Visitors
                .Include(v => v.Visits)
                .FirstOrDefaultAsync(v => v.Id == id && v.IsActive);

            return visitor != null ? MapToDto(visitor) : null;
        }

        public async Task<VisitorDto?> GetVisitorByEmailAsync(string email)
        {
            var visitor = await _context.Visitors
                .Include(v => v.Visits)
                .FirstOrDefaultAsync(v => v.Email == email && v.IsActive);

            return visitor != null ? MapToDto(visitor) : null;
        }

        public async Task<VisitorDto?> GetVisitorByDocumentAsync(string documentType, string documentNumber)
        {
            var visitor = await _context.Visitors
                .Include(v => v.Visits)
                .FirstOrDefaultAsync(v => v.DocumentType == documentType && v.DocumentNumber == documentNumber && v.IsActive);

            return visitor != null ? MapToDto(visitor) : null;
        }

        public async Task<VisitorDto> CreateVisitorAsync(CreateVisitorDto createVisitorDto)
        {
            // Verificar si el visitante ya existe por email o documento
            var existingVisitor = await _context.Visitors
                .FirstOrDefaultAsync(v => v.Email == createVisitorDto.Email || 
                                        (v.DocumentType == createVisitorDto.DocumentType && v.DocumentNumber == createVisitorDto.DocumentNumber));

            if (existingVisitor != null)
            {
                throw new InvalidOperationException($"El visitante con email '{createVisitorDto.Email}' o documento '{createVisitorDto.DocumentType}: {createVisitorDto.DocumentNumber}' ya existe.");
            }

            var visitor = new Visitor
            {
                FirstName = createVisitorDto.FirstName,
                LastName = createVisitorDto.LastName,
                Email = createVisitorDto.Email,
                Phone = createVisitorDto.Phone,
                DocumentType = createVisitorDto.DocumentType,
                DocumentNumber = createVisitorDto.DocumentNumber,
                Company = createVisitorDto.Company,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Visitors.Add(visitor);
            await _context.SaveChangesAsync();

            return await GetVisitorByIdAsync(visitor.Id) ?? MapToDto(visitor);
        }

        public async Task<VisitorDto?> UpdateVisitorAsync(int id, UpdateVisitorDto updateVisitorDto)
        {
            var visitor = await _context.Visitors.FindAsync(id);

            if (visitor == null || !visitor.IsActive)
            {
                return null;
            }

            // Verificar si el nuevo email o documento ya existe en otro visitante
            var existingVisitor = await _context.Visitors
                .FirstOrDefaultAsync(v => (v.Email == updateVisitorDto.Email || 
                                         (v.DocumentType == updateVisitorDto.DocumentType && v.DocumentNumber == updateVisitorDto.DocumentNumber)) && v.Id != id);

            if (existingVisitor != null)
            {
                throw new InvalidOperationException($"El visitante con email '{updateVisitorDto.Email}' o documento '{updateVisitorDto.DocumentType}: {updateVisitorDto.DocumentNumber}' ya existe.");
            }

            visitor.FirstName = updateVisitorDto.FirstName;
            visitor.LastName = updateVisitorDto.LastName;
            visitor.Email = updateVisitorDto.Email;
            visitor.Phone = updateVisitorDto.Phone;
            visitor.DocumentType = updateVisitorDto.DocumentType;
            visitor.DocumentNumber = updateVisitorDto.DocumentNumber;
            visitor.Company = updateVisitorDto.Company;
            visitor.IsActive = updateVisitorDto.IsActive;
            visitor.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return await GetVisitorByIdAsync(id);
        }

        public async Task<bool> DeleteVisitorAsync(int id)
        {
            var visitor = await _context.Visitors.FindAsync(id);

            if (visitor == null || !visitor.IsActive)
            {
                return false;
            }

            // Verificar si el visitante tiene visitas asociadas
            var hasVisits = await _context.Visits.AnyAsync(v => v.VisitorId == id && v.IsActive);

            if (hasVisits)
            {
                throw new InvalidOperationException("No se puede eliminar el visitante porque tiene visitas asociadas.");
            }

            visitor.IsActive = false;
            visitor.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> VisitorExistsAsync(int id)
        {
            return await _context.Visitors.AnyAsync(v => v.Id == id && v.IsActive);
        }

        public async Task<bool> VisitorEmailExistsAsync(string email)
        {
            return await _context.Visitors.AnyAsync(v => v.Email == email && v.IsActive);
        }

        public async Task<bool> VisitorDocumentExistsAsync(string documentType, string documentNumber)
        {
            return await _context.Visitors.AnyAsync(v => v.DocumentType == documentType && v.DocumentNumber == documentNumber && v.IsActive);
        }

        public async Task<IEnumerable<VisitDto>> GetVisitorVisitsAsync(int visitorId)
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

            return visits.Select(v => new VisitDto
            {
                Id = v.Id,
                VisitCode = v.VisitCode,
                Purpose = v.Purpose,
                Status = Enum.Parse<VisitStatus>(v.Status),
                ScheduledDate = v.ScheduledDate,
                EntryTime = v.EntryTime,
                ExitTime = v.ExitTime,
                Notes = v.Notes,
                IsActive = v.IsActive,
                CreatedAt = v.CreatedAt,
                Company = new CompanyDto
                {
                    Id = v.Company.Id,
                    Name = v.Company.Name,
                    Description = v.Company.Description,
                    Address = v.Company.Address,
                    Phone = v.Company.Phone,
                    Email = v.Company.Email,
                    ContactPerson = v.Company.ContactPerson,
                    ContactPhone = v.Company.ContactPhone,
                    ContactEmail = v.Company.ContactEmail,
                    IsActive = v.Company.IsActive,
                    CreatedAt = v.Company.CreatedAt,
                    Zone = new ZoneDto
                    {
                        Id = v.Company.Zone.Id,
                        Name = v.Company.Zone.Name,
                        Description = v.Company.Zone.Description,
                        IsActive = v.Company.Zone.IsActive,
                        CreatedAt = v.Company.Zone.CreatedAt,
                        CompaniesCount = 0,
                        GatesCount = 0
                    },
                    VisitsCount = 0
                },
                Visitor = new VisitorDto
                {
                    Id = v.Visitor.Id,
                    FirstName = v.Visitor.FirstName,
                    LastName = v.Visitor.LastName,
                    Email = v.Visitor.Email,
                    Phone = v.Visitor.Phone,
                    DocumentType = v.Visitor.DocumentType,
                    DocumentNumber = v.Visitor.DocumentNumber,
                    Company = v.Visitor.Company,
                    IsActive = v.Visitor.IsActive,
                    CreatedAt = v.Visitor.CreatedAt,
                    VisitsCount = 0
                },
                Gate = new GateDto
                {
                    Id = v.Gate.Id,
                    Name = v.Gate.Name,
                    Description = v.Gate.Description,
                    GateNumber = v.Gate.GateNumber,
                    IsActive = v.Gate.IsActive,
                    CreatedAt = v.Gate.CreatedAt,
                    Zone = new ZoneDto
                    {
                        Id = v.Gate.Zone.Id,
                        Name = v.Gate.Zone.Name,
                        Description = v.Gate.Zone.Description,
                        IsActive = v.Gate.Zone.IsActive,
                        CreatedAt = v.Gate.Zone.CreatedAt,
                        CompaniesCount = 0,
                        GatesCount = 0
                    },
                    VisitsCount = 0,
                    GuardsCount = 0
                },
                CreatedBy = new UserDto
                {
                    Id = v.CreatedBy.Id,
                    Username = v.CreatedBy.Username,
                    Email = v.CreatedBy.Email,
                    FirstName = v.CreatedBy.FirstName,
                    LastName = v.CreatedBy.LastName,
                    Roles = new List<RoleDto>(),
                    IsActive = v.CreatedBy.IsActive,
                    LastLogin = v.CreatedBy.LastLogin,
                    CreatedAt = v.CreatedBy.CreatedAt
                },
                QRCode = string.Empty // Se generará cuando se solicite
            });
        }

        public async Task<IEnumerable<VisitorDto>> SearchVisitorsAsync(string searchTerm)
        {
            var visitors = await _context.Visitors
                .Include(v => v.Visits)
                .Where(v => v.IsActive && 
                           (v.FirstName.Contains(searchTerm) || 
                            v.LastName.Contains(searchTerm) || 
                            v.Email.Contains(searchTerm) || 
                            v.DocumentNumber.Contains(searchTerm) ||
                            v.Company.Contains(searchTerm)))
                .ToListAsync();

            return visitors.Select(MapToDto);
        }

        private static VisitorDto MapToDto(Visitor visitor)
        {
            return new VisitorDto
            {
                Id = visitor.Id,
                FirstName = visitor.FirstName,
                LastName = visitor.LastName,
                Email = visitor.Email,
                Phone = visitor.Phone,
                DocumentType = visitor.DocumentType,
                DocumentNumber = visitor.DocumentNumber,
                Company = visitor.Company,
                IsActive = visitor.IsActive,
                CreatedAt = visitor.CreatedAt,
                VisitsCount = visitor.Visits?.Count(v => v.IsActive) ?? 0
            };
        }
    }
}
