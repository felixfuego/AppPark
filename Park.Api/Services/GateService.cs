using Microsoft.EntityFrameworkCore;
using Park.Api.Data;
using Park.Api.Services.Interfaces;
using Park.Comun.DTOs;
using Park.Comun.Models;

namespace Park.Api.Services
{
    public class GateService : IGateService
    {
        private readonly ParkDbContext _context;

        public GateService(ParkDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GateDto>> GetAllGatesAsync()
        {
            var gates = await _context.Gates
                .Include(g => g.Zone)
                .Include(g => g.Visits)
                .Where(g => g.IsActive)
                .ToListAsync();

            return gates.Select(MapToDto);
        }

        public async Task<GateDto?> GetGateByIdAsync(int id)
        {
            var gate = await _context.Gates
                .Include(g => g.Zone)
                .Include(g => g.Visits)
                .FirstOrDefaultAsync(g => g.Id == id && g.IsActive);

            return gate != null ? MapToDto(gate) : null;
        }

        public async Task<GateDto?> GetGateByNameAsync(string name)
        {
            var gate = await _context.Gates
                .Include(g => g.Zone)
                .Include(g => g.Visits)
                .FirstOrDefaultAsync(g => g.Name == name && g.IsActive);

            return gate != null ? MapToDto(gate) : null;
        }

        public async Task<GateDto?> GetGateByNumberAsync(string gateNumber)
        {
            var gate = await _context.Gates
                .Include(g => g.Zone)
                .Include(g => g.Visits)
                .FirstOrDefaultAsync(g => g.GateNumber == gateNumber && g.IsActive);

            return gate != null ? MapToDto(gate) : null;
        }

        public async Task<GateDto> CreateGateAsync(CreateGateDto createGateDto)
        {
            // Verificar si el portón ya existe
            var existingGate = await _context.Gates
                .FirstOrDefaultAsync(g => g.Name == createGateDto.Name || g.GateNumber == createGateDto.GateNumber);

            if (existingGate != null)
            {
                throw new InvalidOperationException($"El portón '{createGateDto.Name}' o número '{createGateDto.GateNumber}' ya existe.");
            }

            // Verificar si la zona existe
            var zone = await _context.Zones.FindAsync(createGateDto.ZoneId);
            if (zone == null || !zone.IsActive)
            {
                throw new InvalidOperationException("La zona especificada no existe o no está activa.");
            }

            var gate = new Gate
            {
                Name = createGateDto.Name,
                Description = createGateDto.Description,
                GateNumber = createGateDto.GateNumber,
                ZoneId = createGateDto.ZoneId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Gates.Add(gate);
            await _context.SaveChangesAsync();

            return await GetGateByIdAsync(gate.Id) ?? MapToDto(gate);
        }

        public async Task<GateDto?> UpdateGateAsync(int id, UpdateGateDto updateGateDto)
        {
            var gate = await _context.Gates.FindAsync(id);

            if (gate == null || !gate.IsActive)
            {
                return null;
            }

            // Verificar si el nuevo nombre o número ya existe en otro portón
            var existingGate = await _context.Gates
                .FirstOrDefaultAsync(g => (g.Name == updateGateDto.Name || g.GateNumber == updateGateDto.GateNumber) && g.Id != id);

            if (existingGate != null)
            {
                throw new InvalidOperationException($"El portón '{updateGateDto.Name}' o número '{updateGateDto.GateNumber}' ya existe.");
            }

            // Verificar si la zona existe
            var zone = await _context.Zones.FindAsync(updateGateDto.ZoneId);
            if (zone == null || !zone.IsActive)
            {
                throw new InvalidOperationException("La zona especificada no existe o no está activa.");
            }

            gate.Name = updateGateDto.Name;
            gate.Description = updateGateDto.Description;
            gate.GateNumber = updateGateDto.GateNumber;
            gate.ZoneId = updateGateDto.ZoneId;
            gate.IsActive = updateGateDto.IsActive;
            gate.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return await GetGateByIdAsync(id);
        }

        public async Task<bool> DeleteGateAsync(int id)
        {
            var gate = await _context.Gates.FindAsync(id);

            if (gate == null || !gate.IsActive)
            {
                return false;
            }

            // Verificar si el portón tiene visitas asociadas
            var hasVisits = await _context.Visits.AnyAsync(v => v.GateId == id && v.IsActive);

            if (hasVisits)
            {
                throw new InvalidOperationException("No se puede eliminar el portón porque tiene visitas asociadas.");
            }

            gate.IsActive = false;
            gate.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> GateExistsAsync(int id)
        {
            return await _context.Gates.AnyAsync(g => g.Id == id && g.IsActive);
        }

        public async Task<bool> GateNameExistsAsync(string name)
        {
            return await _context.Gates.AnyAsync(g => g.Name == name && g.IsActive);
        }

        public async Task<bool> GateNumberExistsAsync(string gateNumber)
        {
            return await _context.Gates.AnyAsync(g => g.GateNumber == gateNumber && g.IsActive);
        }

        public async Task<IEnumerable<GateDto>> GetGatesByZoneAsync(int zoneId)
        {
            var gates = await _context.Gates
                .Include(g => g.Zone)
                .Include(g => g.Visits)
                .Where(g => g.ZoneId == zoneId && g.IsActive)
                .ToListAsync();

            return gates.Select(MapToDto);
        }

        private static GateDto MapToDto(Gate gate)
        {
            return new GateDto
            {
                Id = gate.Id,
                Name = gate.Name,
                Description = gate.Description,
                GateNumber = gate.GateNumber,
                IsActive = gate.IsActive,
                CreatedAt = gate.CreatedAt,
                Zone = new ZoneDto
                {
                    Id = gate.Zone.Id,
                    Name = gate.Zone.Name,
                    Description = gate.Zone.Description,
                    IsActive = gate.Zone.IsActive,
                    CreatedAt = gate.Zone.CreatedAt,
                    CompaniesCount = 0,
                    GatesCount = 0
                },
                VisitsCount = gate.Visits?.Count(v => v.IsActive) ?? 0
            };
        }
    }
}
