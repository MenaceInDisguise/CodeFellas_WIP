using System.ComponentModel.DataAnnotations;

namespace Prosjekt.Models.ModelView
{
    //Viewmodel som inneholder informasjon om et behov 
    //som brukeren kan registrere.
    public class BehovViewModel
    {
        //Navnet på behovet.
        public string Navn { get; set; } = string.Empty;

        //Beskrivelse av hva behovet gjelder.
        public string Beskrivelse { get; set; } = string.Empty;

        //Antall som trengs av ressursen/behovet.
        public int Totalt { get; set; } = 0;

        //Breddegraden til posisjonen.
        [Display(Name = "Breddegrad")]
        public string Latitude { get; set; } = string.Empty;

        //Lengdegraden til posisjonen.
        [Display(Name = "Lengdegrad")]
        public string Longitude { get; set; } = string.Empty;
    }
}
