using System.ComponentModel.DataAnnotations;

namespace Prosjekt.Models.ModelView
{
    public class GeoChangeViewModel
    {
        [Display(Name = "Breddegrad")]
        public double? Latitude { get; set; }

        [Display(Name = "Lengdegrad")]
        public double? Longitude { get; set; }

        [Display(Name = "Beskrivelse")]
        public string Description { get; set; } = string.Empty;

        public List<string> ChangeTypes { get; set; } = new List<string>
        {
            "Snøskred",
            "Flom",
            "Jordskred",
            "Skogbrann",
            "Jordskjelv",
            "Annet"
        };

        public List<string> SelectedChangeTypes { get; set; } = new List<string>();

        [Display(Name = "Radius")]
        public double Radius { get; set; } = 500;
    }
}