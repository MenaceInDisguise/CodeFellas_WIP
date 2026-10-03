namespace Prosjekt.Models.ModelView.RessursType
{
    public class KjøretøyViewModel : RessursViewModel
    {
        public enum KjøretøyType
        {
            Bil,
            Buss,
            Sykkel,
            Motorsykkel, Traktor,
            Lastebil,
            Doser,
            Gravemaskin,
            Annet
        }

        public KjøretøyType Type { get; set; } = KjøretøyType.Annet;
        public string Skiltnummer { get; set; } = string.Empty;

        public KjøretøyViewModel()
        {
            Kategori = RessursViewModel.RessursType.Kjøretøy;
        }
    }
}