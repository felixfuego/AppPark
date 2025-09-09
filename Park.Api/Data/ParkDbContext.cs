using Microsoft.EntityFrameworkCore;
using Park.Comun.Models;

namespace Park.Api.Data
{
    public class ParkDbContext : DbContext
    {
        public ParkDbContext(DbContextOptions<ParkDbContext> options) : base(options)
        {
        }

        // DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        
        // DbSets del Parque Industrial
        public DbSet<Sitio> Sitios { get; set; }
        public DbSet<Zona> Zonas { get; set; }
        public DbSet<Centro> Centros { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyCentro> CompanyCentros { get; set; }
        public DbSet<Colaborador> Colaboradores { get; set; }
        public DbSet<ColaboradorByCentro> ColaboradorByCentros { get; set; }
        public DbSet<Visita> Visitas { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<FileEntity> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                
                // Índices únicos
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Configuración de Role
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Description).HasMaxLength(200);
                
                // Índice único para el nombre del rol
                entity.HasIndex(e => e.Name).IsUnique();
            });

            // Configuración de UserRole (tabla intermedia)
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                // Relación muchos a muchos
                entity.HasOne(e => e.User)
                      .WithMany(u => u.UserRoles)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
                      
                entity.HasOne(e => e.Role)
                      .WithMany(r => r.UserRoles)
                      .HasForeignKey(e => e.RoleId)
                      .OnDelete(DeleteBehavior.Cascade);
                      
                // Índice compuesto para evitar duplicados
                entity.HasIndex(e => new { e.UserId, e.RoleId }).IsUnique();
            });

            // Configuración de la relación muchos a muchos User-Role
            modelBuilder.Entity<User>()
                .HasMany(u => u.Roles)
                .WithMany()
                .UsingEntity<UserRole>(
                    j => j
                        .HasOne(ur => ur.Role)
                        .WithMany(r => r.UserRoles)
                        .HasForeignKey(ur => ur.RoleId),
                    j => j
                        .HasOne(ur => ur.User)
                        .WithMany(u => u.UserRoles)
                        .HasForeignKey(ur => ur.UserId),
                    j =>
                    {
                        j.HasKey(t => t.Id);
                        j.Property(t => t.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                    });

            // Configuración de Sitio
            modelBuilder.Entity<Sitio>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Descripcion).HasMaxLength(500);
                
                // Índices
                entity.HasIndex(e => e.Nombre).IsUnique();
            });

            // Configuración de Zona
            modelBuilder.Entity<Zona>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Descripcion).HasMaxLength(500);
                
                // Relación con Sitio
                entity.HasOne(e => e.Sitio)
                      .WithMany(s => s.Zonas)
                      .HasForeignKey(e => e.IdSitio)
                      .OnDelete(DeleteBehavior.Restrict);
                      
                // Índices
                entity.HasIndex(e => new { e.IdSitio, e.Nombre }).IsUnique();
            });

            // Configuración de Centro
            modelBuilder.Entity<Centro>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Localidad).IsRequired().HasMaxLength(200);
                
                // Relación con Zona
                entity.HasOne(e => e.Zona)
                      .WithMany(z => z.Centros)
                      .HasForeignKey(e => e.IdZona)
                      .OnDelete(DeleteBehavior.Restrict);
                      
                // Índices
                entity.HasIndex(e => new { e.IdZona, e.Nombre }).IsUnique();
            });

            // Configuración de Company
            modelBuilder.Entity<Company>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Address).IsRequired().HasMaxLength(300);
                entity.Property(e => e.Phone).HasMaxLength(20);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.ContactPerson).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ContactPhone).HasMaxLength(20);
                entity.Property(e => e.ContactEmail).HasMaxLength(100);
                
                // Relación con Sitio
                entity.HasOne(e => e.Sitio)
                      .WithMany(s => s.Companias)
                      .HasForeignKey(e => e.IdSitio)
                      .OnDelete(DeleteBehavior.Restrict);
                      
                // Índices
                entity.HasIndex(e => e.Name).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Configuración de CompanyCentro
            modelBuilder.Entity<CompanyCentro>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                // Relaciones
                entity.HasOne(e => e.Compania)
                      .WithMany(c => c.CompanyCentros)
                      .HasForeignKey(e => e.IdCompania)
                      .OnDelete(DeleteBehavior.Cascade);
                      
                entity.HasOne(e => e.Centro)
                      .WithMany(c => c.CompanyCentros)
                      .HasForeignKey(e => e.IdCentro)
                      .OnDelete(DeleteBehavior.Cascade);
                      
                // Índice compuesto para evitar duplicados
                entity.HasIndex(e => new { e.IdCompania, e.IdCentro }).IsUnique();
            });

            // Configuración de Colaborador
            modelBuilder.Entity<Colaborador>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Identidad).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Puesto).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.Tel1).HasMaxLength(20);
                entity.Property(e => e.Tel2).HasMaxLength(20);
                entity.Property(e => e.Tel3).HasMaxLength(20);
                entity.Property(e => e.PlacaVehiculo).HasMaxLength(20);
                entity.Property(e => e.Comentario).HasMaxLength(1000);
                
                // Relación con Compañía
                entity.HasOne(e => e.Compania)
                      .WithMany(c => c.Colaboradores)
                      .HasForeignKey(e => e.IdCompania)
                      .OnDelete(DeleteBehavior.Restrict);
                      
                // Índices
                entity.HasIndex(e => e.Identidad).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Configuración de ColaboradorByCentro
            modelBuilder.Entity<ColaboradorByCentro>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                // Relaciones
                entity.HasOne(e => e.Centro)
                      .WithMany(c => c.ColaboradorByCentros)
                      .HasForeignKey(e => e.IdCentro)
                      .OnDelete(DeleteBehavior.Cascade);
                      
                entity.HasOne(e => e.Colaborador)
                      .WithMany(c => c.ColaboradorByCentros)
                      .HasForeignKey(e => e.IdColaborador)
                      .OnDelete(DeleteBehavior.Cascade);
                      
                // Índice compuesto para evitar duplicados
                entity.HasIndex(e => new { e.IdCentro, e.IdColaborador }).IsUnique();
            });

            // Configuración de Visita
            modelBuilder.Entity<Visita>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.NumeroSolicitud).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Procedencia).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Destino).IsRequired().HasMaxLength(200);
                entity.Property(e => e.IdentidadVisitante).IsRequired().HasMaxLength(50);
                entity.Property(e => e.MotivoVisita).IsRequired().HasMaxLength(500);
                entity.Property(e => e.NombreCompleto).IsRequired().HasMaxLength(200);
                entity.Property(e => e.PlacaVehiculo).HasMaxLength(20);
                
                // Relaciones
                entity.HasOne(e => e.Solicitante)
                      .WithMany(c => c.Visitas)
                      .HasForeignKey(e => e.IdSolicitante)
                      .OnDelete(DeleteBehavior.Restrict);
                      
                entity.HasOne(e => e.Compania)
                      .WithMany(c => c.Visitas)
                      .HasForeignKey(e => e.IdCompania)
                      .OnDelete(DeleteBehavior.Restrict);
                      
                entity.HasOne(e => e.RecibidoPor)
                      .WithMany()
                      .HasForeignKey(e => e.IdRecibidoPor)
                      .OnDelete(DeleteBehavior.Restrict);
                      
                entity.HasOne(e => e.Centro)
                      .WithMany(c => c.Visitas)
                      .HasForeignKey(e => e.IdCentro)
                      .OnDelete(DeleteBehavior.Restrict);
                      
                // Índices
                entity.HasIndex(e => e.NumeroSolicitud).IsUnique();
                entity.HasIndex(e => e.Fecha);
                entity.HasIndex(e => e.Estado);
            });


        }
    }
}
