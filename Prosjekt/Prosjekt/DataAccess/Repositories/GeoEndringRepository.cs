using Microsoft.EntityFrameworkCore;
using Prosjekt.Models.Entities;

namespace Prosjekt.DataAccess.Repositories
{
    public class GeoEndringRepository : IGeoEndringRepository
    {
        private readonly ApplicationDbContext _context;

        public GeoEndringRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(GeoEndring geoEndring)
        {
            await _context.GeoEndringer.AddAsync(geoEndring);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<GeoEndring>> GetAllAsync()
        {
            // Henter alle registreringer fra databasen
            return await _context.GeoEndringer.ToListAsync();
        }
    }
}