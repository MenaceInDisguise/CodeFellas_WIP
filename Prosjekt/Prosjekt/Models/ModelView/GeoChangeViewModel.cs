using System.ComponentModel.DataAnnotations;

namespace Prosjekt.Models.ModelView
{
    //ViewModel used to send position data from the map to the controller.
    public class GeoChangeViewModel
    {
        //The latitude of the position fetched from the map.
        public double? Latitude { get; set; }

        //The longitude of the position fetched from the map.
        public double? Longitude { get; set; }


        //Description of the position or the event.
        public string Description { get; set; } = string.Empty;

        //List of change types that can be registered on the map.
        public List<string> ChangeTypes { get; set; } = new List<string>
        {
            "Avalanche",
            "Flood",
            "Landslide",
            "Forest fire",
            "Earthquake",
            "Other"
        };
        public List<string> SelectedChangeTypes { get; set; } = new List<string>();

        public string DisplayChangeTypes => SelectedChangeTypes != null && SelectedChangeTypes.Any()
            ? string.Join(", ", SelectedChangeTypes)
            : "No change type selected";



        //The radius around the position to be shown on the map.
        public double Radius { get; set; } = 500;
    }
}
