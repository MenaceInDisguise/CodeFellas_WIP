using Microsoft.EntityFrameworkCore;
using Prosjekt.Models.Entities;

namespace Prosjekt.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new ApplicationDbContext with the specified DbContextOptions.
        /// </summary>
        /// <remarks>Register the context in the service container and supply
        /// DbContextOptions<ApplicationDbContext>; the options are passed to the base DbContext constructor.</remarks>
        /// <param name="options">Options for configuring the context, typically provided by dependency injection and forwarded to the base
        /// DbContext.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<GeoChange> GeoChanges { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<GeoChange>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ComplexProperty(e => e.Coords, coord =>
                {
                    coord.Property(c => c.Latitude).HasColumnName("Latitude").IsRequired();
                    coord.Property(c => c.Longitude).HasColumnName("Longitude").IsRequired();
                });
                entity.Property(e => e.Description);
                entity.Property(e => e.ChangeTypes);
                entity.Property(e => e.Radius);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
        }
    }
}