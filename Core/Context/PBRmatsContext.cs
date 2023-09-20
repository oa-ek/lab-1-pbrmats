using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PBRmats.Core.Entities;

namespace PBRmats.Core.Context
{
    public class PBRmatsContext : IdentityDbContext
    {
        public PBRmatsContext(DbContextOptions<PBRmatsContext> options) : base(options)
        {
        }

        public DbSet<Material> Materials { get; set; }
        public DbSet<MaterialsCollection> MaterialsCollections { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<License> Licenses { get; set; }
        public DbSet<Source> Sources { get; set; }
        public DbSet<User> Users { get; set; }
        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=USER;Database=PBRmatsDB;Integrated Security=True;Encrypt=True;TrustServerCertificate=True");
            base.OnConfiguring(optionsBuilder);
        }*/

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Material>()
                .HasOne(m => m.Category)
                .WithMany()
                /*.HasForeignKey(m => m.CategoryId)*/;

            modelBuilder.Entity<Material>()
                .HasOne(m => m.License)
                .WithMany();

            modelBuilder.Entity<Material>()
                .HasMany(m => m.Sources)
                .WithMany()
                .UsingEntity(j => j.ToTable("MaterialSources"));

            modelBuilder.Entity<User>()
                .HasMany(u => u.MaterialsCollections)
                .WithOne(mc => mc.ParentUser)
                /*.HasForeignKey(mc => mc.UserId)*/;

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PBRmatsContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
