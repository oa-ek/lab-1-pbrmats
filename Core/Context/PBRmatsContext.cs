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
        public DbSet<Tag> Tags { get; set; }
        public DbSet<MaterialTag> MaterialTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Material>()
                .HasOne(m => m.Category)
                .WithMany()
                .HasForeignKey(m => m.CategoryId);

            modelBuilder.Entity<Material>()
                .HasOne(m => m.License)
                .WithMany();

            modelBuilder.Entity<Tag>()
                .HasKey(d => d.Id);

            modelBuilder.Entity<MaterialTag>()
                .HasKey(mt => mt.Id);

            modelBuilder.Entity<MaterialsCollection>()
                .HasKey(mc => mc.Id);

            modelBuilder.Entity<AppUser>()
                .HasKey(u => u.Id);

            AddAutoIncludes(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PBRmatsContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        private static void AddAutoIncludes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Material>().Navigation(material => material.License).AutoInclude();
            modelBuilder.Entity<Material>().Navigation(material => material.Category).AutoInclude();
            modelBuilder.Entity<Material>().Navigation(material => material.MaterialTags).AutoInclude();

            modelBuilder.Entity<MaterialTag>().Navigation(materialTag => materialTag.Material).AutoInclude();
            modelBuilder.Entity<MaterialTag>().Navigation(materialTag => materialTag.Tag).AutoInclude();

            modelBuilder.Entity<AppUser>().Navigation(user => user.MaterialsCollections).AutoInclude();
            modelBuilder.Entity<MaterialsCollection>().Navigation(mc => mc.AppUser).AutoInclude();
            modelBuilder.Entity<MaterialsCollection>().Navigation(material => material.MaterialMaterialsCollection).AutoInclude();

            modelBuilder.Entity<MaterialMaterialsCollection>().Navigation(mmc => mmc.Material).AutoInclude();
            modelBuilder.Entity<MaterialMaterialsCollection>().Navigation(mmc => mmc.MaterialsCollection).AutoInclude();
        }
    }
}
