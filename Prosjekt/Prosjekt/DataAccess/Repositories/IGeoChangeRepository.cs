using Prosjekt.Models.Entities;

namespace Prosjekt.DataAccess.Repositories
{
    public interface IGeoChangeRepository
    {
        Task AddAsync(GeoChange geoChange);
        Task<IEnumerable<GeoChange>> GetAllAsync();
    }
}