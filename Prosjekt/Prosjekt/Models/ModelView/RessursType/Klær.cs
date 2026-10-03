namespace Prosjekt.Models.ModelView.RessursType
{
    public class KlærViewModel : RessursViewModel
    {
        public enum KlærStørrelse
        {
            XS, S, M, L, XL, XXL
        }

        public KlærStørrelse Størrelse { get; set; } = KlærStørrelse.M;

        public KlærViewModel()
        {
            Kategori = RessursViewModel.RessursType.Klær;
        }
    }
}