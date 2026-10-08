using System.ComponentModel.DataAnnotations;

namespace Prosjekt.Models.ModelView
{
    //ViewModel used to send position data from the map to the controller.
    public class GeoChangeViewModel
    {
        //The latitude of the position fetched from the map.
        [Display(Name = "Breddegrad")]
        public double? Latitude { get; set; }

        //The longitude of the position fetched from the map.
        [Display(Name = "Lengdegrad")]
        public double? Longitude { get; set; }


        //Description of the position or the event.
        [Display(Name = "Beskrivelse")]
        public string Description { get; set; } = string.Empty;

        //List of change types that can be registered on the map.
        [Display(Name = "Endringstyper")]
        public List<string> ChangeTypes { get; set; } = new List<string>
        {
            "Jordskjelv",
            "Flom",
            "Jordskred",
            "Skogbrann",
            "Jordskjelv",
            "Annet"
        };
        public List<string> SelectedChangeTypes { get; set; } = new List<string>();

        public string DisplayChangeTypes => SelectedChangeTypes != null && SelectedChangeTypes.Any()
            ? string.Join(", ", SelectedChangeTypes)
            : "Ingen endringstype valgt";



        //The radius around the position to be shown on the map.
        [Display(Name = "Radius")]
        public double Radius { get; set; } = 500;
    }
}
