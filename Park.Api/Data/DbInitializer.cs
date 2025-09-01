using Microsoft.EntityFrameworkCore;
using Park.Comun.Models;
using System.Security.Cryptography;
using System.Text;

namespace Park.Api.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(ParkDbContext context)
        {
            // Asegurar que la base de datos esté creada
            await context.Database.EnsureCreatedAsync();

            // Inicializar roles si no existen
            await SeedRolesAsync(context);

            // Inicializar usuario administrador si no existe
            await SeedAdminUserAsync(context);

            await context.SaveChangesAsync();
        }

        private static async Task SeedRolesAsync(ParkDbContext context)
        {
            if (await context.Roles.AnyAsync())
                return;

            var roles = new List<Role>
            {
                new Role
                {
                    Name = "Admin",
                    Description = "Administrador del sistema con acceso completo",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new Role
                {
                    Name = "Operacion",
                    Description = "Operador del sistema con acceso a gestión de entidades y reportes",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new Role
                {
                    Name = "Guardia",
                    Description = "Guardia de seguridad con acceso a validación de visitas",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                }
            };

            await context.Roles.AddRangeAsync(roles);
        }

        private static async Task SeedAdminUserAsync(ParkDbContext context)
        {
            // Verificar si ya existe un usuario administrador
            if (await context.Users.AnyAsync(u => u.Username == "admin"))
                return;

            // Crear usuario administrador
            var adminUser = new User
            {
                Username = "admin",
                Email = "admin@parqueindustrial.com",
                PasswordHash = HashPassword("admin25"),
                FirstName = "Administrador",
                LastName = "Sistema",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await context.Users.AddAsync(adminUser);
            await context.SaveChangesAsync(); // Guardar para obtener el ID

            // Obtener el rol Admin
            var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
            if (adminRole != null)
            {
                // Asignar rol Admin al usuario administrador
                var userRole = new UserRole
                {
                    UserId = adminUser.Id,
                    RoleId = adminRole.Id,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                await context.UserRoles.AddAsync(userRole);
            }
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}
