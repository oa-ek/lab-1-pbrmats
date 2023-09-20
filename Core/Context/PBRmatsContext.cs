using Microsoft.EntityFrameworkCore;
using PBRmats.Core.Entities;
using PBRmatsCore.Entities;

namespace PBRmatsCore.Context
{
    public class PBRmatsContext : DbContext
    {
        public PBRmatsContext(DbContextOptions<PBRmatsContext> options) : base(options)
        {
        }

        public DbSet<Material> Materials => Set<Material>();
        public DbSet<MaterialsCollection> MaterialsCollections => Set<MaterialsCollection>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<PBRmats.Core.Entities.License> Licenses => Set<PBRmats.Core.Entities.License>();
        public DbSet<Source> Sources => Set<Source>();
        public DbSet<User> Users => Set<User>();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=USER;Database=PBRmatsDB;Integrated Security=True;Encrypt=True;TrustServerCertificate=True");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Material>()
                .HasOne(m => m.Category)
                .WithMany()
                /*.HasForeignKey(m => m.CategoryId)*/;

            modelBuilder.Entity<Material>()
                .HasMany(m => m.Licenses)
                .WithMany()
                .UsingEntity(j => j.ToTable("MaterialLicenses"));

            modelBuilder.Entity<Material>()
                .HasMany(m => m.Sources)
                .WithMany()
                .UsingEntity(j => j.ToTable("MaterialSources"));

            modelBuilder.Entity<User>()
                .HasMany(u => u.MaterialsCollections)
                .WithOne(mc => mc.ParentUser)
                /*.HasForeignKey(mc => mc.UserId)*/;
        }
    }
}
