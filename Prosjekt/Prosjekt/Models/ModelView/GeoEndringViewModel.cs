using System.ComponentModel.DataAnnotations;

namespace Prosjekt.Models.ModelView
{
    //ViewModel som brukes til å sende posisjonsdatafra kartet til controlleren.
    public class GeoEndringViewModel
    {
        //Breddegraden til posisjonen som hentes fra kartet.
        public string Latitude { get; set; } = string.Empty;

        //Lengdegraden til posisjonen osm hentes fra kartet.
        public string Longitude { get; set; } = string.Empty;


        //Beskrivelse av posisjonen eller hendelsen.
        public string Description { get; set; } = string.Empty;

        //Liste med type endringer som kan registreres på kartet.
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

        public string DisplayChangeTypes => SelectedChangeTypes != null && SelectedChangeTypes.Any()
            ? string.Join(", ", SelectedChangeTypes)
            : "Ingen endringstype valgt";



        //Radiusen rundt posisjonen som skal vises på kartet.
        public double Radius { get; set; } = 500;
    }
}
