using Microsoft.EntityFrameworkCore;
using Prosjekt.Models.Entities;

namespace Prosjekt.DataAccess.Repositories
{
    public class GeoChangeRepository : IGeoChangeRepository
    {
        /// <summary>
        /// Application database context used to access the underlying data store.
        /// </summary>
        /// <remarks>Assigned via constructor injection and intended for use only within the declaring
        /// class.</remarks>
        private readonly ApplicationDbContext _context;

        public GeoChangeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(GeoChange geoChange)
        {
            await _context.GeoChanges.AddAsync(geoChange);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<GeoChange>> GetAllAsync()
        {
            // Henter alle registreringer fra databasen
            return await _context.GeoChanges.ToListAsync();
        }
    }
}