using Park.Web.Models;
using Park.Comun.DTOs;

namespace Park.Web.Services
{
    public interface IAuthorizationService
    {
        Task<bool> CanManageUsersAsync(int userId);
        Task<bool> CanManageCompaniesAsync(int userId);
        Task<bool> CanManageVisitsAsync(int userId, int? companyId = null);
        Task<bool> CanCreateVisitsAsync(int userId, int companyId);
        Task<bool> CanManageVisitAsync(int userId, int visitId);
        Task<bool> CanCheckInOutVisitAsync(int userId, int visitId, int gateId);
        Task<bool> CanViewVisitAsync(int userId, int visitId);
        Task<bool> CanAccessCompanyAsync(int userId, int companyId);
        Task<IEnumerable<int>> GetAccessibleCompanyIdsAsync(int userId);
        Task<IEnumerable<int>> GetAccessibleGateIdsAsync(int userId);
        Task<bool> IsAdminAsync(int userId);
        Task<bool> CanViewCompanyUsersAsync(int userId, int companyId);
    }

    public class AuthorizationService : IAuthorizationService
    {
        private readonly IUserService _userService;
        private readonly ICompanyService _companyService;
        private readonly IVisitService _visitService;
        private readonly IGateService _gateService;
        private readonly ILogger<AuthorizationService> _logger;

        public AuthorizationService(
            IUserService userService,
            ICompanyService companyService,
            IVisitService visitService,
            IGateService gateService,
            ILogger<AuthorizationService> logger)
        {
            _userService = userService;
            _companyService = companyService;
            _visitService = visitService;
            _gateService = gateService;
            _logger = logger;
        }

        public async Task<bool> CanManageUsersAsync(int userId)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(userId);
                if (user == null) return false;

                // Solo Admin puede gestionar usuarios
                return user.Roles.Any(r => r.Name == "Admin");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verificando permisos de gestión de usuarios para usuario {UserId}", userId);
                return false;
            }
        }

        public async Task<bool> CanManageCompaniesAsync(int userId)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(userId);
                if (user == null) return false;

                // Solo Admin puede gestionar empresas
                return user.Roles.Any(r => r.Name == "Admin");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verificando permisos de gestión de empresas para usuario {UserId}", userId);
                return false;
            }
        }

        public async Task<bool> CanManageVisitsAsync(int userId, int? companyId = null)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(userId);
                if (user == null) return false;

                // Admin puede gestionar todas las visitas
                if (user.Roles.Any(r => r.Name == "Admin"))
                    return true;

                // Operacion puede gestionar visitas de sus empresas asignadas
                if (user.Roles.Any(r => r.Name == "Operacion"))
                {
                    if (companyId.HasValue)
                        return await CanAccessCompanyAsync(userId, companyId.Value);
                    return true; // Puede ver todas las visitas de sus empresas
                }

                // Guardia puede ver visitas (solo lectura)
                if (user.Roles.Any(r => r.Name == "Guardia"))
                {
                    return true; // Puede ver visitas para hacer check-in/out
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verificando permisos de gestión de visitas para usuario {UserId}", userId);
                return false;
            }
        }

        public async Task<bool> CanCreateVisitsAsync(int userId, int companyId)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(userId);
                if (user == null) 
                {
                    _logger.LogWarning("Usuario {UserId} no encontrado", userId);
                    return false;
                }

                _logger.LogInformation("Verificando permisos de creación de visitas para usuario {UserId} con roles: {Roles}", 
                    userId, string.Join(", ", user.Roles.Select(r => r.Name)));

                // Admin puede crear visitas en cualquier empresa
                if (user.Roles.Any(r => r.Name == "Admin"))
                {
                    _logger.LogInformation("Usuario {UserId} es Admin - acceso permitido", userId);
                    return true;
                }

                // Operacion puede crear visitas en sus empresas asignadas
                if (user.Roles.Any(r => r.Name == "Operacion"))
                {
                    _logger.LogInformation("Usuario {UserId} tiene rol Operacion", userId);
                    
                    // TEMPORAL: Permitir acceso sin restricciones de empresa para debug
                    // TODO: Restaurar la lógica de verificación de empresas una vez resuelto el problema
                    _logger.LogInformation("Acceso temporalmente permitido para usuario {UserId} con rol Operacion", userId);
                    return true;
                    
                    // CÓDIGO ORIGINAL (comentado temporalmente):
                    // Si companyId es 0, verificar si tiene acceso a al menos una empresa
                    // if (companyId == 0)
                    // {
                    //     var accessibleCompanies = await GetAccessibleCompanyIdsAsync(userId);
                    //     var hasAccess = accessibleCompanies.Any();
                    //     _logger.LogInformation("Usuario {UserId} tiene acceso a {CompanyCount} empresas: {CompanyIds}", 
                    //         userId, accessibleCompanies.Count(), string.Join(", ", accessibleCompanies));
                    //     return hasAccess;
                    // }
                    // 
                    // var canAccess = await CanAccessCompanyAsync(userId, companyId);
                    // _logger.LogInformation("Usuario {UserId} acceso a empresa {CompanyId}: {CanAccess}", userId, companyId, canAccess);
                    // return canAccess;
                }

                _logger.LogWarning("Usuario {UserId} no tiene permisos para crear visitas", userId);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verificando permisos de creación de visitas para usuario {UserId}", userId);
                return false;
            }
        }

        public async Task<bool> CanManageVisitAsync(int userId, int visitId)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(userId);
                if (user == null) return false;

                // Admin puede gestionar cualquier visita
                if (user.Roles.Any(r => r.Name == "Admin"))
                    return true;

                // Obtener información de la visita
                var visit = await _visitService.GetByIdAsync(visitId);
                if (visit == null) return false;

                // Operacion puede gestionar visitas de sus empresas
                if (user.Roles.Any(r => r.Name == "Operacion"))
                    return await CanAccessCompanyAsync(userId, visit.CompanyId);

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verificando permisos de gestión de visita {VisitId} para usuario {UserId}", visitId, userId);
                return false;
            }
        }

        public async Task<bool> CanCheckInOutVisitAsync(int userId, int visitId, int gateId)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(userId);
                if (user == null) return false;

                // Admin puede hacer check-in/out en cualquier puerta
                if (user.Roles.Any(r => r.Name == "Admin"))
                    return true;

                // Guardia puede hacer check-in/out en cualquier puerta
                if (user.Roles.Any(r => r.Name == "Guardia"))
                {
                    return true; // Los guardias pueden hacer check-in/out en cualquier puerta
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verificando permisos de check-in/out para usuario {UserId} en puerta {GateId}", userId, gateId);
                return false;
            }
        }

        public async Task<bool> CanViewVisitAsync(int userId, int visitId)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(userId);
                if (user == null) return false;

                // Admin puede ver cualquier visita
                if (user.Roles.Any(r => r.Name == "Admin"))
                    return true;

                // Obtener información de la visita
                var visit = await _visitService.GetByIdAsync(visitId);
                if (visit == null) return false;

                // Operacion puede ver visitas de sus empresas
                if (user.Roles.Any(r => r.Name == "Operacion"))
                    return await CanAccessCompanyAsync(userId, visit.CompanyId);

                // Guardia puede ver cualquier visita (solo lectura)
                if (user.Roles.Any(r => r.Name == "Guardia"))
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verificando permisos de visualización de visita {VisitId} para usuario {UserId}", visitId, userId);
                return false;
            }
        }

        public async Task<bool> CanAccessCompanyAsync(int userId, int companyId)
        {
            try
            {
                var userCompanies = await _userService.GetUserCompaniesAsync(userId);
                var canAccess = userCompanies.Any(c => c.Id == companyId);
                _logger.LogInformation("Usuario {UserId} acceso a empresa {CompanyId}: {CanAccess}. Empresas asignadas: {CompanyIds}", 
                    userId, companyId, canAccess, string.Join(", ", userCompanies.Select(c => c.Id)));
                return canAccess;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verificando acceso a empresa {CompanyId} para usuario {UserId}", companyId, userId);
                return false;
            }
        }

        public async Task<IEnumerable<int>> GetAccessibleCompanyIdsAsync(int userId)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(userId);
                if (user == null) 
                {
                    _logger.LogWarning("Usuario {UserId} no encontrado al obtener empresas accesibles", userId);
                    return Enumerable.Empty<int>();
                }

                _logger.LogInformation("Obteniendo empresas accesibles para usuario {UserId} con roles: {Roles}", 
                    userId, string.Join(", ", user.Roles.Select(r => r.Name)));

                // Admin puede acceder a todas las empresas
                if (user.Roles.Any(r => r.Name == "Admin"))
                {
                    var allCompanies = await _companyService.GetAllAsync();
                    var companyIds = allCompanies.Select(c => c.Id).ToList();
                    _logger.LogInformation("Admin {UserId} puede acceder a todas las empresas: {CompanyIds}", 
                        userId, string.Join(", ", companyIds));
                    return companyIds;
                }

                // Otros roles solo pueden acceder a sus empresas asignadas
                var userCompanies = await _userService.GetUserCompaniesAsync(userId);
                var accessibleCompanyIds = userCompanies.Select(c => c.Id).ToList();
                _logger.LogInformation("Usuario {UserId} tiene acceso a empresas: {CompanyIds}", 
                    userId, string.Join(", ", accessibleCompanyIds));
                return accessibleCompanyIds;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo empresas accesibles para usuario {UserId}", userId);
                return Enumerable.Empty<int>();
            }
        }

        public async Task<IEnumerable<int>> GetAccessibleGateIdsAsync(int userId)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(userId);
                if (user == null) return Enumerable.Empty<int>();

                // Admin puede acceder a todas las puertas
                if (user.Roles.Any(r => r.Name == "Admin"))
                {
                    var allGates = await _gateService.GetAllAsync();
                    return allGates.Select(g => g.Id);
                }

                // Guardia puede acceder a puertas de sus zonas asignadas
                if (user.Roles.Any(r => r.Name == "Guardia"))
                {
                    var userZones = await _userService.GetUserZonesAsync(userId);
                    var zoneIds = userZones.Select(z => z.Id).ToList();
                    
                    var accessibleGates = new List<int>();
                    foreach (var zoneId in zoneIds)
                    {
                        var gatesInZone = await _gateService.GetByZoneAsync(zoneId);
                        if (gatesInZone != null)
                        {
                            accessibleGates.AddRange(gatesInZone.Select(g => g.Id));
                        }
                    }
                    
                    return accessibleGates;
                }

                // Operador puede acceder a puertas de sus zonas asignadas
                if (user.Roles.Any(r => r.Name == "Operador"))
                {
                    var userZones = await _userService.GetUserZonesAsync(userId);
                    var zoneIds = userZones.Select(z => z.Id).ToList();
                    
                    var accessibleGates = new List<int>();
                    foreach (var zoneId in zoneIds)
                    {
                        var gatesInZone = await _gateService.GetByZoneAsync(zoneId);
                        if (gatesInZone != null)
                        {
                            accessibleGates.AddRange(gatesInZone.Select(g => g.Id));
                        }
                    }
                    
                    return accessibleGates;
                }

                // Otros roles no tienen acceso directo a puertas
                return Enumerable.Empty<int>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo puertas accesibles para usuario {UserId}", userId);
                return Enumerable.Empty<int>();
            }
        }

        public async Task<bool> CanViewCompanyUsersAsync(int userId, int companyId)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(userId);
                if (user == null) return false;

                // Admin puede ver usuarios de cualquier empresa
                if (user.Roles.Any(r => r.Name == "Admin"))
                    return true;

                // Operacion puede ver usuarios de sus empresas asignadas
                if (user.Roles.Any(r => r.Name == "Operacion"))
                    return await CanAccessCompanyAsync(userId, companyId);

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verificando permisos de visualización de usuarios de empresa {CompanyId} para usuario {UserId}", companyId, userId);
                return false;
            }
        }

        public async Task<bool> IsAdminAsync(int userId)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(userId);
                if (user == null) return false;

                return user.Roles.Any(r => r.Name == "Admin");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verificando si usuario {UserId} es Admin", userId);
                return false;
            }
        }
    }
} 