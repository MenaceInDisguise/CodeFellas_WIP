using System.ComponentModel.DataAnnotations;

namespace Prosjekt.Models.ModelView
{
    //ViewModel som brukes til å sende posisjonsdatafra kartet til controlleren.
    public class PositionViewModel
    {
        //Breddegraden til posisjonen som hentes fra kartet.
        public string Latitude { get; set; } = string.Empty;
        //Lengdegraden til posisjonen osm hentes fra kartet.
        public string Longitude { get; set; } = string.Empty;
        //Beskrivelse av posisjonen eller hendelsen.
        public string Description { get; set; } = string.Empty;
    }
}
