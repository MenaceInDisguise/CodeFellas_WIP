using System.ComponentModel.DataAnnotations;

namespace Prosjekt.Models.ModelView
{
    //Viewmodel som inneholder informasjon om et behov 
    //som brukeren kan registrere.
    public class BehovViewModel
    {
        //Navnet på behovet.
        [Required(ErrorMessage = "Navn må fylles ut.")]
        public string Navn { get; set; } = string.Empty;

        //Beskrivelse av hva behovet gjelder.
        [Required(ErrorMessage = "Beskrivelse må fylles ut.")]
        public string Beskrivelse { get; set; } = string.Empty;

        //Antall som trengs av ressursen/behovet.
        [Range(1, int.MaxValue, ErrorMessage = "Totalt må være minst 1.")]
        public int Totalt { get; set; } = 0;

        //Hvor akutt behovet er
        [Display(Name = "Viktighet")]
        public Viktighet Viktighet { get; set; } = Viktighet.Lav;

        //Breddegraden til posisjonen.
        [Display(Name = "Breddegrad")]
        public double? Latitude { get; set; } 

        //Lengdegraden til posisjonen.
        [Display(Name = "Lengdegrad")]
        public double? Longitude { get; set; } 
    }
}
