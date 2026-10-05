using System.ComponentModel.DataAnnotations;

namespace Prosjekt.Models.Entities
{
    public class GeoEndring
    {
        public int Id { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        [MaxLength(4000)]
        public string Description { get; set; } = string.Empty;

        [MaxLength(200)]
        public string ChangeTypes { get; set; } = string.Empty;

        public double Radius { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}