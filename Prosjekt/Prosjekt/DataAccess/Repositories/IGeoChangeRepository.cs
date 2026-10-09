using Prosjekt.Models.Entities;

namespace Prosjekt.DataAccess.Repositories
{
    public interface IGeoChangeRepository
    {
        /// <summary>
        /// Adds the specified GeoChange to the repository or data store asynchronously.
        /// </summary>
        /// <param name="geoChange">The GeoChange to add.</param>
        /// <returns>A Task that represents the asynchronous add operation.</returns>
        Task AddAsync(GeoChange geoChange);
        Task<IEnumerable<GeoChange>> GetAllAsync();
    }
}