using Microsoft.EntityFrameworkCore;
using Park.Api.Data;
using Park.Api.Services.Interfaces;
using Park.Comun.DTOs;
using Park.Comun.Models;

namespace Park.Api.Services
{
    public class VisitorService : IVisitorService
    {
        private readonly ParkDbContext _context;
        private readonly ILogger<VisitorService> _logger;

        public VisitorService(ParkDbContext context, ILogger<VisitorService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<VisitorDto>> GetAllVisitorsAsync()
        {
            try
            {
                var visitors = await _context.Visitors
                    .OrderBy(v => v.FirstName)
                    .ThenBy(v => v.LastName)
                    .ToListAsync();

                return visitors.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los visitantes");
                throw;
            }
        }

        public async Task<IEnumerable<VisitorDto>> GetActiveVisitorsAsync()
        {
            try
            {
                var visitors = await _context.Visitors
                    .Where(v => v.IsActive)
                    .OrderBy(v => v.FirstName)
                    .ThenBy(v => v.LastName)
                    .ToListAsync();

                return visitors.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visitantes activos");
                throw;
            }
        }

        public async Task<VisitorDto?> GetVisitorByIdAsync(int id)
        {
            try
            {
                var visitor = await _context.Visitors
                    .FirstOrDefaultAsync(v => v.Id == id);

                return visitor != null ? MapToDto(visitor) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visitante con ID {Id}", id);
                throw;
            }
        }

        public async Task<VisitorDto?> GetVisitorByEmailAsync(string email)
        {
            try
            {
                var visitor = await _context.Visitors
                    .FirstOrDefaultAsync(v => v.Email == email);

                return visitor != null ? MapToDto(visitor) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visitante con email {Email}", email);
                throw;
            }
        }

        public async Task<VisitorDto?> GetVisitorByDocumentAsync(string documentType, string documentNumber)
        {
            try
            {
                var visitor = await _context.Visitors
                    .FirstOrDefaultAsync(v => v.DocumentType == documentType && v.DocumentNumber == documentNumber);

                return visitor != null ? MapToDto(visitor) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visitante con documento {DocumentType} {DocumentNumber}", documentType, documentNumber);
                throw;
            }
        }

        public async Task<IEnumerable<VisitorDto>> GetVisitorsByCompanyAsync(string company)
        {
            try
            {
                var visitors = await _context.Visitors
                    .Where(v => v.Company.Contains(company))
                    .OrderBy(v => v.FirstName)
                    .ThenBy(v => v.LastName)
                    .ToListAsync();

                return visitors.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visitantes de la empresa {Company}", company);
                throw;
            }
        }

        public async Task<VisitorDto> CreateVisitorAsync(CreateVisitorDto createVisitorDto)
        {
            try
            {
                // Verificar si ya existe un visitante con el mismo email
                var existingEmail = await _context.Visitors
                    .FirstOrDefaultAsync(v => v.Email == createVisitorDto.Email);

                if (existingEmail != null)
                {
                    throw new InvalidOperationException($"Ya existe un visitante con el email {createVisitorDto.Email}");
                }

                // Verificar si ya existe un visitante con el mismo documento
                var existingDocument = await _context.Visitors
                    .FirstOrDefaultAsync(v => v.DocumentType == createVisitorDto.DocumentType && 
                                            v.DocumentNumber == createVisitorDto.DocumentNumber);

                if (existingDocument != null)
                {
                    throw new InvalidOperationException($"Ya existe un visitante con el documento {createVisitorDto.DocumentType} {createVisitorDto.DocumentNumber}");
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
                    IsActive = createVisitorDto.IsActive,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Visitors.Add(visitor);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Visitante creado exitosamente: {FullName} ({Email})", visitor.FullName, visitor.Email);
                return MapToDto(visitor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear visitante: {Email}", createVisitorDto.Email);
                throw;
            }
        }

        public async Task<VisitorDto> UpdateVisitorAsync(UpdateVisitorDto updateVisitorDto)
        {
            try
            {
                var visitor = await _context.Visitors.FindAsync(updateVisitorDto.Id);
                if (visitor == null)
                {
                    throw new InvalidOperationException($"Visitante con ID {updateVisitorDto.Id} no encontrado");
                }

                // Verificar si ya existe otro visitante con el mismo email
                var existingEmail = await _context.Visitors
                    .FirstOrDefaultAsync(v => v.Email == updateVisitorDto.Email && v.Id != updateVisitorDto.Id);

                if (existingEmail != null)
                {
                    throw new InvalidOperationException($"Ya existe otro visitante con el email {updateVisitorDto.Email}");
                }

                // Verificar si ya existe otro visitante con el mismo documento
                var existingDocument = await _context.Visitors
                    .FirstOrDefaultAsync(v => v.DocumentType == updateVisitorDto.DocumentType && 
                                            v.DocumentNumber == updateVisitorDto.DocumentNumber &&
                                            v.Id != updateVisitorDto.Id);

                if (existingDocument != null)
                {
                    throw new InvalidOperationException($"Ya existe otro visitante con el documento {updateVisitorDto.DocumentType} {updateVisitorDto.DocumentNumber}");
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

                _logger.LogInformation("Visitante actualizado exitosamente: {FullName} ({Email})", visitor.FullName, visitor.Email);
                return MapToDto(visitor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar visitante con ID {Id}", updateVisitorDto.Id);
                throw;
            }
        }

        public async Task<bool> DeleteVisitorAsync(int id)
        {
            try
            {
                var visitor = await _context.Visitors.FindAsync(id);
                if (visitor == null)
                {
                    return false;
                }

                // Verificar si el visitante tiene visitas asociadas
                var hasVisits = await _context.Visitas
                    .AnyAsync(v => v.IdentidadVisitante == visitor.DocumentNumber);

                if (hasVisits)
                {
                    throw new InvalidOperationException($"No se puede eliminar el visitante {visitor.FullName} porque tiene visitas asociadas. Use la opción de desactivar en su lugar.");
                }

                _context.Visitors.Remove(visitor);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Visitante eliminado exitosamente: {FullName} ({Email})", visitor.FullName, visitor.Email);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar visitante con ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> ActivateVisitorAsync(int id)
        {
            try
            {
                var visitor = await _context.Visitors.FindAsync(id);
                if (visitor == null)
                {
                    return false;
                }

                visitor.IsActive = true;
                visitor.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Visitante activado exitosamente: {FullName} ({Email})", visitor.FullName, visitor.Email);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al activar visitante con ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> DeactivateVisitorAsync(int id)
        {
            try
            {
                var visitor = await _context.Visitors.FindAsync(id);
                if (visitor == null)
                {
                    return false;
                }

                visitor.IsActive = false;
                visitor.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Visitante desactivado exitosamente: {FullName} ({Email})", visitor.FullName, visitor.Email);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al desactivar visitante con ID {Id}", id);
                throw;
            }
        }

        public async Task<PagedResultDto<VisitorDto>> SearchVisitorsAsync(VisitorSearchDto searchDto)
        {
            try
            {
                var query = _context.Visitors.AsQueryable();

                // Aplicar filtros
                if (!string.IsNullOrWhiteSpace(searchDto.SearchTerm))
                {
                    var searchLower = searchDto.SearchTerm.ToLower();
                    query = query.Where(v => 
                        v.FirstName.ToLower().Contains(searchLower) ||
                        v.LastName.ToLower().Contains(searchLower) ||
                        v.Email.ToLower().Contains(searchLower) ||
                        v.Company.ToLower().Contains(searchLower) ||
                        v.DocumentNumber.Contains(searchLower));
                }

                if (!string.IsNullOrWhiteSpace(searchDto.DocumentType))
                {
                    query = query.Where(v => v.DocumentType == searchDto.DocumentType);
                }

                if (!string.IsNullOrWhiteSpace(searchDto.Company))
                {
                    query = query.Where(v => v.Company.Contains(searchDto.Company));
                }

                if (searchDto.IsActive.HasValue)
                {
                    query = query.Where(v => v.IsActive == searchDto.IsActive.Value);
                }

                // Aplicar ordenamiento
                query = searchDto.SortBy?.ToLower() switch
                {
                    "email" => searchDto.SortDescending ? query.OrderByDescending(v => v.Email) : query.OrderBy(v => v.Email),
                    "company" => searchDto.SortDescending ? query.OrderByDescending(v => v.Company) : query.OrderBy(v => v.Company),
                    "documenttype" => searchDto.SortDescending ? query.OrderByDescending(v => v.DocumentType) : query.OrderBy(v => v.DocumentType),
                    "createdat" => searchDto.SortDescending ? query.OrderByDescending(v => v.CreatedAt) : query.OrderBy(v => v.CreatedAt),
                    "fullname" => searchDto.SortDescending ? query.OrderByDescending(v => v.FirstName).ThenByDescending(v => v.LastName) : query.OrderBy(v => v.FirstName).ThenBy(v => v.LastName),
                    _ => searchDto.SortDescending ? query.OrderByDescending(v => v.FirstName).ThenByDescending(v => v.LastName) : query.OrderBy(v => v.FirstName).ThenBy(v => v.LastName)
                };

                // Obtener total de registros
                var totalCount = await query.CountAsync();

                // Aplicar paginación
                var visitors = await query
                    .Skip((searchDto.Page - 1) * searchDto.PageSize)
                    .Take(searchDto.PageSize)
                    .ToListAsync();

                var visitorDtos = visitors.Select(MapToDto).ToList();

                return new PagedResultDto<VisitorDto>
                {
                    Data = visitorDtos,
                    TotalCount = totalCount,
                    Page = searchDto.Page,
                    PageSize = searchDto.PageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar visitantes");
                throw;
            }
        }

        // Nuevos métodos para gestión de visitantes desde visitas
        public async Task<VisitorExisteDto> BuscarPorIdentidadAsync(string documentNumber)
        {
            try
            {
                var visitor = await _context.Visitors
                    .FirstOrDefaultAsync(v => v.DocumentNumber == documentNumber);

                if (visitor == null)
                {
                    return new VisitorExisteDto
                    {
                        Existe = false,
                        Visitor = null,
                        Mensaje = "No se encontró un visitante con esta identidad"
                    };
                }

                return new VisitorExisteDto
                {
                    Existe = true,
                    Visitor = MapToDto(visitor),
                    Mensaje = $"Visitante encontrado: {visitor.FullName}"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar visitante por identidad {DocumentNumber}", documentNumber);
                throw;
            }
        }

        public async Task<VisitorDto> CrearDesdeVisitaAsync(CrearVisitorDesdeVisitaDto dto)
        {
            try
            {
                // Verificar si ya existe un visitante con el mismo documento
                var existingVisitor = await _context.Visitors
                    .FirstOrDefaultAsync(v => v.DocumentNumber == dto.DocumentNumber);

                if (existingVisitor != null)
                {
                    throw new InvalidOperationException($"Ya existe un visitante con el documento {dto.DocumentNumber}");
                }

                var visitor = new Visitor
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email ?? string.Empty,
                    Phone = dto.Phone ?? string.Empty,
                    DocumentType = dto.DocumentType,
                    DocumentNumber = dto.DocumentNumber,
                    Company = dto.Company ?? string.Empty,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Visitors.Add(visitor);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Visitante creado desde visita: {FullName} ({DocumentNumber})", visitor.FullName, visitor.DocumentNumber);
                return MapToDto(visitor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear visitante desde visita: {DocumentNumber}", dto.DocumentNumber);
                throw;
            }
        }

        public async Task<VisitorDto> ActualizarDesdeVisitaAsync(int id, CrearVisitorDesdeVisitaDto dto)
        {
            try
            {
                var visitor = await _context.Visitors.FindAsync(id);
                if (visitor == null)
                {
                    throw new InvalidOperationException($"Visitante con ID {id} no encontrado");
                }

                // Verificar si ya existe otro visitante con el mismo documento
                var existingVisitor = await _context.Visitors
                    .FirstOrDefaultAsync(v => v.DocumentNumber == dto.DocumentNumber && v.Id != id);

                if (existingVisitor != null)
                {
                    throw new InvalidOperationException($"Ya existe otro visitante con el documento {dto.DocumentNumber}");
                }

                visitor.FirstName = dto.FirstName;
                visitor.LastName = dto.LastName;
                visitor.Email = dto.Email ?? string.Empty;
                visitor.Phone = dto.Phone ?? string.Empty;
                visitor.DocumentType = dto.DocumentType;
                visitor.DocumentNumber = dto.DocumentNumber;
                visitor.Company = dto.Company ?? string.Empty;
                visitor.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Visitante actualizado desde visita: {FullName} ({DocumentNumber})", visitor.FullName, visitor.DocumentNumber);
                return MapToDto(visitor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar visitante desde visita con ID {Id}", id);
                throw;
            }
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
                UpdatedAt = visitor.UpdatedAt
            };
        }
    }
}
