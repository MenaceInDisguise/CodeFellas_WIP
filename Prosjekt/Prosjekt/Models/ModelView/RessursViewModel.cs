using System.ComponentModel.DataAnnotations;

namespace Prosjekt.Models.ModelView
{
    //ViewModel som inneholder informasjon om en ressurs som brukeren kan registrere.
    public class RessursViewModel
    {
        public enum RessursType
        {
            Provisjon,
            Klær,
            Verktøy,
            Kjøretøy,
            Materialer,
            Annet
        }

        //Navnet på ressursen.
        [Required(ErrorMessage = "Navn må fylles ut. ")]
        public string Navn { get; set; } = string.Empty;

        //Beskrivelse av ressursen.
        public string Beskrivelse { get; set; } = string.Empty;

        //Antall som er tilgjengelig av ressursen.
        [Required(ErrorMessage = "Antall må være minst 1.")]
        public int Antall { get; set; } = 0;

        //Breddegraden til posisjonen.
        [Display(Name = "Breddegrad")]
        public double? Latitude { get; set; } 

        //Lengdegraden til posisjonen.
        [Display(Name = "Lengdegrad")]
        public double? Longitude { get; set; }

        // Kategorien som ressursen tilhører.
        public RessursType Kategori { get; set; } = RessursType.Annet;
    }
}
