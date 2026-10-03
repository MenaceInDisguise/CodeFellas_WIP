using Prosjekt.Models.ModelView;

namespace Prosjekt.Models.ModelView.RessursType
{
    public class MaterialerViewModel : RessursViewModel
    {
        public enum MaterialerType
        {
            Treverk,
            Metall,
            Plast,
            Glass,
            Grus,
            Stein,
            Jord,
            Sand,
            Annet
        }

        public MaterialerType Type { get; set; } = MaterialerType.Annet;

        public MaterialerViewModel()
        {
            Kategori = RessursViewModel.RessursType.Materialer;
        }
    }
}
