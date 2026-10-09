using System.ComponentModel.DataAnnotations;

namespace Prosjekt.Models.ModelView
{
    public class GeoChangeViewModel
    {
        [Display(Name = "Breddegrad")]
        [Required(ErrorMessage = "Du må legge inn breddegrad")]
        public double? Latitude { get; set; }


        [Display(Name = "Lengdegrad")]
        [Required(ErrorMessage = "Du må legge inn lengdegrad")]
        public double? Longitude { get; set; }


        [Display(Name = "Beskrivelse")]
        [Required(ErrorMessage = "Beskrivelse må fylles ut.")]
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


        [Display(Name = "Endringstyper")]
        [Required(ErrorMessage = "Du må velge minst en endringstype")]
        public List<string> SelectedChangeTypes { get; set; } = new List<string>();


        [Display(Name = "Radius")]
        [Range(1, double.MaxValue, ErrorMessage = "Radius må være større enn 0.")]
        public double Radius { get; set; } = 500;
    }
}