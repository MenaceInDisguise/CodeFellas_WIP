using System.ComponentModel.DataAnnotations;

namespace Prosjekt.Models.ModelView
{
    //Viewmodel that contains information about a need
    //that the user can register.
    public class NeedViewModel
    {
        //The name of the need.
        public string Name { get; set; } = string.Empty;

        //Description of what the need concerns.
        public string Description { get; set; } = string.Empty;

        //Quantity needed of the resource/need.
        public int Total { get; set; } = 0;

        //How urgent the need is
        [Display(Name = "Importance")]
        public Importance Importance { get; set; } = Importance.Low;

        //The latitude of the position.
        [Display(Name = "Latitude")]
        public string Latitude { get; set; } = string.Empty;

        //The longitude of the position.
        [Display(Name = "Longitude")]
        public string Longitude { get; set; } = string.Empty;
    }
}
