using System.ComponentModel.DataAnnotations;

namespace Prosjekt.Models.ModelView
{
    //Viewmodel that contains information about a need
    //that the user can register.
    public class NeedViewModel
    {
        //The name of the need.
        [Display(Name = "Navn")]
        public string Name { get; set; } = string.Empty;

        //Description of what the need concerns.
        [Display(Name = "Beskrivelse")]
        public string Description { get; set; } = string.Empty;

        //Quantity needed of the resource/need.
        [Display(Name = "Antall")]
        public int Total { get; set; } = 0;

        //How urgent the need is
        [Display(Name = "Prioritet")]
        public Importance Importance { get; set; } = Importance.Low;

        //The latitude of the position.
        [Display(Name = "Breddegrad")]
        public string Latitude { get; set; } = string.Empty;

        //The longitude of the position.
        [Display(Name = "Lengdegrad")]
        public string Longitude { get; set; } = string.Empty;
    }
}
