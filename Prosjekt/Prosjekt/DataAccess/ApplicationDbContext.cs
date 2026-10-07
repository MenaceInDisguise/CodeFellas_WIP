using Microsoft.EntityFrameworkCore;
using Prosjekt.Models.Entities;

namespace Prosjekt.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<GeoEndring> GeoEndringer { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Konfigurer GeoEndring-entiteten
            modelBuilder.Entity<GeoEndring>(entity =>
            {
                entity.HasKey(e => e.Id);

                // Forteller EF Core at Coordinates skal mappes til kolonnene Latitude og Longitude
                entity.ComplexProperty(e => e.Coords, coord =>
                {
                    coord.Property(c => c.Latitude)
                         .HasColumnName("Latitude")
                         .IsRequired();

                    coord.Property(c => c.Longitude)
                         .HasColumnName("Longitude")
                         .IsRequired();
                });

                entity.Property(e => e.Description);
                entity.Property(e => e.ChangeTypes);
                entity.Property(e => e.Radius);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
        }
    }
}