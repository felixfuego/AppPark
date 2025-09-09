using Park.Comun.DTOs;
using Park.Front.Services;

namespace Park.Front.Services
{
    public class RoleService
    {
        private readonly AuthService _authService;

        public RoleService(AuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Verifica si el usuario actual tiene un rol específico
        /// </summary>
        public async Task<bool> HasRoleAsync(string roleName)
        {
            var user = await _authService.GetCurrentUserAsync();
            if (user == null) return false;

            return user.Roles.Any(r => r.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Verifica si el usuario actual tiene alguno de los roles especificados
        /// </summary>
        public async Task<bool> HasAnyRoleAsync(params string[] roleNames)
        {
            var user = await _authService.GetCurrentUserAsync();
            if (user == null) return false;

            return user.Roles.Any(r => roleNames.Any(rn => r.Name.Equals(rn, StringComparison.OrdinalIgnoreCase)));
        }

        /// <summary>
        /// Verifica si el usuario actual tiene todos los roles especificados
        /// </summary>
        public async Task<bool> HasAllRolesAsync(params string[] roleNames)
        {
            var user = await _authService.GetCurrentUserAsync();
            if (user == null) return false;

            return roleNames.All(rn => user.Roles.Any(r => r.Name.Equals(rn, StringComparison.OrdinalIgnoreCase)));
        }

        /// <summary>
        /// Obtiene los roles del usuario actual
        /// </summary>
        public async Task<List<string>> GetCurrentUserRolesAsync()
        {
            var user = await _authService.GetCurrentUserAsync();
            if (user == null) return new List<string>();

            return user.Roles.Select(r => r.Name).ToList();
        }

        /// <summary>
        /// Verifica si el usuario puede acceder a una sección específica del menú
        /// Basado en la lógica de roles del API
        /// </summary>
        public async Task<bool> CanAccessSectionAsync(string sectionName)
        {
            return sectionName.ToLower() switch
            {
                "configuracion" => await HasRoleAsync("Admin"), // Solo Admin puede configurar
                "gestion" => await HasAnyRoleAsync("Admin", "Operador"), // Admin y Operador pueden gestionar
                "vigilancia" => await HasAnyRoleAsync("Admin", "Guardia"), // Admin y Guardia pueden vigilar
                _ => false
            };
        }

        /// <summary>
        /// Verifica si el usuario puede acceder a una página específica
        /// Basado en los permisos definidos en los controladores del API
        /// </summary>
        public async Task<bool> CanAccessPageAsync(string pageName)
        {
            return pageName.ToLower() switch
            {
                // CONFIGURACIÓN - Solo Admin (según UserController, SitioController, ZonaController, CentroController, CompanyController)
                "users" => await HasRoleAsync("Admin"),
                "sites" => await HasRoleAsync("Admin"),
                "zones" => await HasRoleAsync("Admin"),
                "centers" => await HasRoleAsync("Admin"),
                "companies" => await HasRoleAsync("Admin"),
                
                // GESTIÓN - Admin y Operador (según VisitaController, ColaboradorController)
                "visits" => await HasAnyRoleAsync("Admin", "Operador"),
                "visitors" => await HasAnyRoleAsync("Admin", "Operador"),
                
                // VIGILANCIA - Admin, Operador y Guardia (según VisitaController para check-in/check-out)
                "guard-panel" => await HasAnyRoleAsync("Admin", "Operador", "Guardia"),
                
                // DASHBOARD - Todos los usuarios autenticados
                "dashboard" => true,
                
                _ => false
            };
        }

        /// <summary>
        /// Obtiene las secciones del menú que el usuario puede ver
        /// </summary>
        public async Task<List<string>> GetAccessibleSectionsAsync()
        {
            var sections = new List<string>();
            
            if (await CanAccessSectionAsync("configuracion"))
                sections.Add("configuracion");
                
            if (await CanAccessSectionAsync("gestion"))
                sections.Add("gestion");
                
            if (await CanAccessSectionAsync("vigilancia"))
                sections.Add("vigilancia");
                
            return sections;
        }
    }
}
