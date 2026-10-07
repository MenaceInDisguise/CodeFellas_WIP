using Microsoft.EntityFrameworkCore;
using Prosjekt.Models.Entities;

namespace Prosjekt.DataAccess.Repositories
{
    public class GeoChangeRepository : IGeoChangeRepository
    {
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
            // Fetches all registrations from the database
            return await _context.GeoChanges.ToListAsync();
        }
    }
}