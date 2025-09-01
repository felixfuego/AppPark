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
        public DbSet<Zone> Zones { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Gate> Gates { get; set; }
        public DbSet<Visitor> Visitors { get; set; }
        public DbSet<Visit> Visits { get; set; }
        public DbSet<UserCompany> UserCompanies { get; set; }
        public DbSet<UserZone> UserZones { get; set; }

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

            // Configuración de Zone
            modelBuilder.Entity<Zone>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                
                // Índice único para el nombre de la zona
                entity.HasIndex(e => e.Name).IsUnique();
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
                
                // Relación con Zone
                entity.HasOne(e => e.Zone)
                      .WithMany(z => z.Companies)
                      .HasForeignKey(e => e.ZoneId)
                      .OnDelete(DeleteBehavior.Restrict);
                      
                // Índices
                entity.HasIndex(e => e.Name).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Configuración de Gate
            modelBuilder.Entity<Gate>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.GateNumber).IsRequired().HasMaxLength(20);
                
                // Relación con Zone
                entity.HasOne(e => e.Zone)
                      .WithMany(z => z.Gates)
                      .HasForeignKey(e => e.ZoneId)
                      .OnDelete(DeleteBehavior.Restrict);
                      
                // Índices
                entity.HasIndex(e => e.Name).IsUnique();
                entity.HasIndex(e => e.GateNumber).IsUnique();
            });

            // Configuración de Visitor
            modelBuilder.Entity<Visitor>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Phone).HasMaxLength(20);
                entity.Property(e => e.DocumentType).IsRequired().HasMaxLength(50);
                entity.Property(e => e.DocumentNumber).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Company).HasMaxLength(200);
                
                // Índices
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => new { e.DocumentType, e.DocumentNumber }).IsUnique();
            });

            // Configuración de Visit
            modelBuilder.Entity<Visit>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.VisitCode).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Purpose).IsRequired().HasMaxLength(500);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Notes).HasMaxLength(1000);
                
                // Relaciones
                entity.HasOne(e => e.Company)
                      .WithMany(c => c.Visits)
                      .HasForeignKey(e => e.CompanyId)
                      .OnDelete(DeleteBehavior.Restrict);
                      
                entity.HasOne(e => e.Visitor)
                      .WithMany(v => v.Visits)
                      .HasForeignKey(e => e.VisitorId)
                      .OnDelete(DeleteBehavior.Restrict);
                      
                entity.HasOne(e => e.Gate)
                      .WithMany(g => g.Visits)
                      .HasForeignKey(e => e.GateId)
                      .OnDelete(DeleteBehavior.Restrict);
                      
                entity.HasOne(e => e.CreatedBy)
                      .WithMany(u => u.CreatedVisits)
                      .HasForeignKey(e => e.CreatedById)
                      .OnDelete(DeleteBehavior.Restrict);
                      
                // Índices
                entity.HasIndex(e => e.VisitCode).IsUnique();
                entity.HasIndex(e => e.ScheduledDate);
                entity.HasIndex(e => e.Status);
            });

            // Configuración de UserCompany
            modelBuilder.Entity<UserCompany>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                // Relaciones
                entity.HasOne(e => e.User)
                      .WithMany(u => u.UserCompanies)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
                      
                entity.HasOne(e => e.Company)
                      .WithMany(c => c.UserCompanies)
                      .HasForeignKey(e => e.CompanyId)
                      .OnDelete(DeleteBehavior.Cascade);
                      
                // Índice compuesto para evitar duplicados
                entity.HasIndex(e => new { e.UserId, e.CompanyId }).IsUnique();
            });

            // Configuración de UserZone
            modelBuilder.Entity<UserZone>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                // Relaciones
                entity.HasOne(e => e.User)
                      .WithMany(u => u.UserZones)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
                      
                entity.HasOne(e => e.Zone)
                      .WithMany(z => z.UserZones)
                      .HasForeignKey(e => e.ZoneId)
                      .OnDelete(DeleteBehavior.Cascade);
                      
                // Índice compuesto para evitar duplicados
                entity.HasIndex(e => new { e.UserId, e.ZoneId }).IsUnique();
            });


        }
    }
}
