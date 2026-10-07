using Microsoft.EntityFrameworkCore;
using Prosjekt.Models.Entities;

namespace Prosjekt.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<GeoChange> GeoChanges { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the GeoChange entity
            modelBuilder.Entity<GeoChange>(entity =>
            {
                // Keeps the existing table name so the database schema is unchanged
                entity.ToTable("GeoEndringer");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Latitude).IsRequired();
                entity.Property(e => e.Longitude).IsRequired();
                entity.Property(e => e.Description);
                entity.Property(e => e.ChangeTypes);
                entity.Property(e => e.Radius);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
        }
    }
}