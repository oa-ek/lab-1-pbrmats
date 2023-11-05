using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PBRmats.Core.Entities;
using System.Diagnostics;

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
                .HasForeignKey(m => m.CategoryId);

            modelBuilder.Entity<Material>()
                .HasOne(m => m.License)
                .WithMany();

            modelBuilder.Entity<Tag>()
                .HasKey(d => d.Id);

            modelBuilder.Entity<MaterialTag>()
                .HasKey(mt => mt.Id);

            /*modelBuilder.Entity<MaterialTag>()
                .HasOne(mt => mt.Material)
                .WithMany(m => m.MaterialTags)
                .HasForeignKey(mt => mt.MaterialId);

            modelBuilder.Entity<MaterialTag>()
                .HasOne(mt => mt.Tag)
                .WithMany(t => t.MaterialTags)
                .HasForeignKey(mt => mt.TagsId);*/

            modelBuilder.Entity<User>()
                .HasMany(u => u.MaterialsCollections)
                .WithOne(mc => mc.ParentUser)
                /*.HasForeignKey(mc => mc.UserId)*/;

            modelBuilder.Entity<MaterialsCollection>()
                .HasMany(m => m.Materials)
                .WithMany();

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
        }
    }
}
