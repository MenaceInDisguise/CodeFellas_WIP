using System.ComponentModel.DataAnnotations;

namespace Prosjekt.Models.Entities
{
    public class GeoEndring
    {
        public int Id { get; set; }

        // Erstatter de separate feltene for Latitude og Longitude
        public Coordinates Coords { get; set; }

        [MaxLength(4000)]
        public string Description { get; set; } = string.Empty;

        [MaxLength(200)]
        public string ChangeTypes { get; set; } = string.Empty;

        public double Radius { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}