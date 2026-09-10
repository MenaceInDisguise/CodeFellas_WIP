using System.ComponentModel.DataAnnotations;

namespace Prosjekt.Models.ModelView
{
    public class PositionViewModel
    {
        public string Latitude { get; set; } = string.Empty;
        public string Longitude { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
