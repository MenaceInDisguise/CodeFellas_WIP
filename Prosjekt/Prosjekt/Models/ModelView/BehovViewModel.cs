namespace Prosjekt.Models.ModelView
{
    //Viewmodel som inneholder informasjon om et behov 
    //som brukeren kan registrere.
    public class BehovViewModel
    {
        //Navnet på behovet.
        public string Navn { get; set; } = string.Empty;
        //Beskirvelse av hva behovet gjelder.
        public string Beskrivelse { get; set; } = string.Empty;
        //Antall som trengs av ressursen/behovet.
        public int Totalt { get; set; } = 0;
    }
}
