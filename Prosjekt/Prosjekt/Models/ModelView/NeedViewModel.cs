using System.ComponentModel.DataAnnotations;

namespace Prosjekt.Models.ModelView
{
    public class NeedViewModel
    {
        [Required(ErrorMessage = "Navn må fylles ut.")]
        [Display(Name = "Navn")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Beskrivelse må fylles ut.")]
        [Display(Name = "Beskrivelse")]
        public string Description { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Totalt må være minst 1.")]
        [Display(Name = "Antall som trengs")]
        public int Total { get; set; } = 0;

        [Display(Name = "Viktighet")]
        public Importance Importance { get; set; } = Importance.Low;

        [Display(Name = "Breddegrad")]
        public double? Latitude { get; set; }

        [Display(Name = "Lengdegrad")]
        public double? Longitude { get; set; }
    }
}