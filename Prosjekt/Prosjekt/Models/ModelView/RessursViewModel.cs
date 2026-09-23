using System.ComponentModel.DataAnnotations;

namespace Prosjekt.Models.ModelView
{
    //ViewModel som inneholder informasjon om en ressurs som brukeren kan registrere.
    public class RessursViewModel
    {
        //Navnet på ressursen.
        public string Navn { get; set; } = string.Empty;

        //Beskrivelse av ressursen.
        public string Beskrivelse { get; set; } = string.Empty;

        //Antall som er tilgjengelig av ressursen.
        public int Antall { get; set; } = 0;

        //Breddegraden til posisjonen.
        [Display(Name = "Breddegrad")]
        public string Latitude { get; set; } = string.Empty;

        //Lengdegraden til posisjonen.
        [Display(Name = "Lengdegrad")]
        public string Longitude { get; set; } = string.Empty;
    }
}
