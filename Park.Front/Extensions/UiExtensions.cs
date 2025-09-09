using Park.Comun.DTOs;

namespace Park.Front.Extensions
{
    public static class UiExtensions
    {
        // Extensiones para UserDto
        public static string GetEstado(this UserDto user)
        {
            return user.IsActive ? "Activo" : "Inactivo";
        }

        public static string GetEstadoColor(this UserDto user)
        {
            return user.IsActive ? "success" : "error";
        }

        // Extensiones para RoleDto
        public static string GetEstado(this RoleDto role)
        {
            return role.IsActive ? "Activo" : "Inactivo";
        }

        public static string GetEstadoColor(this RoleDto role)
        {
            return role.IsActive ? "success" : "error";
        }

        // Extensiones para CompanyDto
        public static string GetEstado(this CompanyDto company)
        {
            return company.IsActive ? "Activo" : "Inactivo";
        }

        public static string GetEstadoColor(this CompanyDto company)
        {
            return company.IsActive ? "success" : "error";
        }

        // Extensiones para SitioDto
        public static int GetZonasCount(this SitioDto sitio)
        {
            return sitio.Zonas?.Count ?? 0;
        }

        public static int GetCompaniasCount(this SitioDto sitio)
        {
            return sitio.Companias?.Count ?? 0;
        }

        public static int GetActiveZonasCount(this SitioDto sitio)
        {
            return sitio.Zonas?.Count(z => z.IsActive) ?? 0;
        }

        public static int GetActiveCompaniasCount(this SitioDto sitio)
        {
            return sitio.Companias?.Count(c => c.IsActive) ?? 0;
        }

        public static bool HasActiveDependencies(this SitioDto sitio)
        {
            return GetActiveZonasCount(sitio) > 0 || GetActiveCompaniasCount(sitio) > 0;
        }

        public static string GetEstado(this SitioDto sitio)
        {
            return sitio.IsActive ? "Activo" : "Inactivo";
        }

        public static string GetEstadoColor(this SitioDto sitio)
        {
            return sitio.IsActive ? "success" : "error";
        }

        // Extensiones para ZonaDto
        public static int GetCentrosCount(this ZonaDto zona)
        {
            return zona.Centros?.Count ?? 0;
        }

        public static string GetEstado(this ZonaDto zona)
        {
            return zona.IsActive ? "Activo" : "Inactivo";
        }

        public static string GetEstadoColor(this ZonaDto zona)
        {
            return zona.IsActive ? "success" : "error";
        }

        // Extensiones para CentroDto
        public static int GetCompaniasCount(this CentroDto centro)
        {
            return centro.CompanyCentros?.Count ?? 0;
        }

        public static int GetColaboradoresCount(this CentroDto centro)
        {
            return centro.ColaboradorByCentros?.Count ?? 0;
        }

        public static string GetEstado(this CentroDto centro)
        {
            return centro.IsActive ? "Activo" : "Inactivo";
        }

        public static string GetEstadoColor(this CentroDto centro)
        {
            return centro.IsActive ? "success" : "error";
        }

        // Extensiones para ColaboradorDto
        public static string GetEstado(this ColaboradorDto colaborador)
        {
            return colaborador.IsActive ? "Activo" : "Inactivo";
        }

        public static string GetEstadoColor(this ColaboradorDto colaborador)
        {
            return colaborador.IsActive ? "success" : "error";
        }

        public static string GetBlackListStatus(this ColaboradorDto colaborador)
        {
            return colaborador.IsBlackList ? "Lista Negra" : "Normal";
        }

        public static string GetBlackListColor(this ColaboradorDto colaborador)
        {
            return colaborador.IsBlackList ? "error" : "success";
        }
    }
}
