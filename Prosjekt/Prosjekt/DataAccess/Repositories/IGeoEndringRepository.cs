using Prosjekt.Models.Entities;

namespace Prosjekt.DataAccess.Repositories
{
    public interface IGeoEndringRepository
    {
        Task AddAsync(GeoEndring geoEndring);
        Task<IEnumerable<GeoEndring>> GetAllAsync();
    }
}