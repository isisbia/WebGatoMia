using Microsoft.EntityFrameworkCore;
using WebGatoMia.Models;

namespace WebGatoMia.Data
{
    public class GatoMiaDbContext : DbContext
    {
        public GatoMiaDbContext(DbContextOptions<GatoMiaDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<UserType> UserTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed UserType
            modelBuilder.Entity<UserType>().HasData(
                new UserType { Id = 1, Name = "Admin" },
                new UserType { Id = 2, Name = "Gerente" },
                new UserType { Id = 3, Name = "Padrão" },
                new UserType { Id = 4, Name = "Responsável" },
                new UserType { Id = 5, Name = "Psicólogo" },
                new UserType { Id = 6, Name = "Advogado" }
            );

            // Configura relacionamento User → UserType via FK UserTypeId
            modelBuilder.Entity<User>()
                .HasOne(u => u.UserType)
                .WithMany(ut => ut.Users)
                .HasForeignKey(u => u.UserTypeId)
                .OnDelete(DeleteBehavior.Restrict); // opcional, para evitar deleção em cascata

            // Configurações da entidade User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);

                // Guid gerado no código, então ValueGeneratedOnAdd pode ser removido
                // entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Phone)
                    .HasMaxLength(20);

                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Seed Users
            modelBuilder.Entity<User>().HasData(
                new
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Name = "Maria Silva",
                    Email = "maria@gatomia.com",
                    PasswordHash = "hashedpassword1",
                    Phone = "(11) 99999-9999",
                    UserTypeId = 1,
                    DateRegistration = new DateTime(2024, 6, 1),
                    IsActive = true
                },
                new
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Name = "João Santos",
                    Email = "joao@gatomia.com",
                    PasswordHash = "hashedpassword2",
                    Phone = "(11) 88888-8888",
                    UserTypeId = 3,
                    DateRegistration = new DateTime(2024, 6, 1),
                    IsActive = true
                }
            );
        }
    }
}
