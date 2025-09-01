using Microsoft.EntityFrameworkCore;
using Park.Api.Data;
using Park.Api.Services.Interfaces;
using Park.Comun.DTOs;
using Park.Comun.Models;

namespace Park.Api.Services
{
    public class ZoneService : IZoneService
    {
        private readonly ParkDbContext _context;

        public ZoneService(ParkDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ZoneDto>> GetAllZonesAsync()
        {
            var zones = await _context.Zones
                .Include(z => z.Companies)
                .Include(z => z.Gates)
                .Where(z => z.IsActive)
                .ToListAsync();

            return zones.Select(MapToDto);
        }

        public async Task<ZoneDto?> GetZoneByIdAsync(int id)
        {
            var zone = await _context.Zones
                .Include(z => z.Companies)
                .Include(z => z.Gates)
                .FirstOrDefaultAsync(z => z.Id == id && z.IsActive);

            return zone != null ? MapToDto(zone) : null;
        }

        public async Task<ZoneDto?> GetZoneByNameAsync(string name)
        {
            var zone = await _context.Zones
                .Include(z => z.Companies)
                .Include(z => z.Gates)
                .FirstOrDefaultAsync(z => z.Name == name && z.IsActive);

            return zone != null ? MapToDto(zone) : null;
        }

        public async Task<ZoneDto> CreateZoneAsync(CreateZoneDto createZoneDto)
        {
            // Verificar si la zona ya existe
            var existingZone = await _context.Zones
                .FirstOrDefaultAsync(z => z.Name == createZoneDto.Name);

            if (existingZone != null)
            {
                throw new InvalidOperationException($"La zona '{createZoneDto.Name}' ya existe.");
            }

            var zone = new Zone
            {
                Name = createZoneDto.Name,
                Description = createZoneDto.Description,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Zones.Add(zone);
            await _context.SaveChangesAsync();

            return MapToDto(zone);
        }

        public async Task<ZoneDto?> UpdateZoneAsync(int id, UpdateZoneDto updateZoneDto)
        {
            var zone = await _context.Zones.FindAsync(id);

            if (zone == null || !zone.IsActive)
            {
                return null;
            }

            // Verificar si el nuevo nombre ya existe en otra zona
            var existingZone = await _context.Zones
                .FirstOrDefaultAsync(z => z.Name == updateZoneDto.Name && z.Id != id);

            if (existingZone != null)
            {
                throw new InvalidOperationException($"La zona '{updateZoneDto.Name}' ya existe.");
            }

            zone.Name = updateZoneDto.Name;
            zone.Description = updateZoneDto.Description;
            zone.IsActive = updateZoneDto.IsActive;
            zone.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return await GetZoneByIdAsync(id);
        }

        public async Task<bool> DeleteZoneAsync(int id)
        {
            var zone = await _context.Zones.FindAsync(id);

            if (zone == null || !zone.IsActive)
            {
                return false;
            }

            // Verificar si la zona tiene empresas o portones asociados
            var hasCompanies = await _context.Companies.AnyAsync(c => c.ZoneId == id && c.IsActive);
            var hasGates = await _context.Gates.AnyAsync(g => g.ZoneId == id && g.IsActive);

            if (hasCompanies || hasGates)
            {
                throw new InvalidOperationException("No se puede eliminar la zona porque tiene empresas o portones asociados.");
            }

            zone.IsActive = false;
            zone.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ZoneExistsAsync(int id)
        {
            return await _context.Zones.AnyAsync(z => z.Id == id && z.IsActive);
        }

        public async Task<bool> ZoneNameExistsAsync(string name)
        {
            return await _context.Zones.AnyAsync(z => z.Name == name && z.IsActive);
        }

        public async Task<IEnumerable<CompanyDto>> GetZoneCompaniesAsync(int zoneId)
        {
            var companies = await _context.Companies
                .Include(c => c.Zone)
                .Where(c => c.ZoneId == zoneId && c.IsActive)
                .ToListAsync();

            return companies.Select(c => new CompanyDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Address = c.Address,
                Phone = c.Phone,
                Email = c.Email,
                ContactPerson = c.ContactPerson,
                ContactPhone = c.ContactPhone,
                ContactEmail = c.ContactEmail,
                IsActive = c.IsActive,
                CreatedAt = c.CreatedAt,
                Zone = new ZoneDto
                {
                    Id = c.Zone.Id,
                    Name = c.Zone.Name,
                    Description = c.Zone.Description,
                    IsActive = c.Zone.IsActive,
                    CreatedAt = c.Zone.CreatedAt,
                    CompaniesCount = 0,
                    GatesCount = 0
                },
                VisitsCount = 0
            });
        }

        public async Task<IEnumerable<GateDto>> GetZoneGatesAsync(int zoneId)
        {
            var gates = await _context.Gates
                .Include(g => g.Zone)
                .Where(g => g.ZoneId == zoneId && g.IsActive)
                .ToListAsync();

            return gates.Select(g => new GateDto
            {
                Id = g.Id,
                Name = g.Name,
                Description = g.Description,
                GateNumber = g.GateNumber,
                IsActive = g.IsActive,
                CreatedAt = g.CreatedAt,
                Zone = new ZoneDto
                {
                    Id = g.Zone.Id,
                    Name = g.Zone.Name,
                    Description = g.Zone.Description,
                    IsActive = g.Zone.IsActive,
                    CreatedAt = g.Zone.CreatedAt,
                    CompaniesCount = 0,
                    GatesCount = 0
                },
                VisitsCount = 0,
                GuardsCount = 0
            });
        }

        private static ZoneDto MapToDto(Zone zone)
        {
            return new ZoneDto
            {
                Id = zone.Id,
                Name = zone.Name,
                Description = zone.Description,
                IsActive = zone.IsActive,
                CreatedAt = zone.CreatedAt,
                CompaniesCount = zone.Companies?.Count(c => c.IsActive) ?? 0,
                GatesCount = zone.Gates?.Count(g => g.IsActive) ?? 0
            };
        }
    }
}
