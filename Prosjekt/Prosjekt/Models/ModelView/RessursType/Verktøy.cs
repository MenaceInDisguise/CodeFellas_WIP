namespace Prosjekt.Models.ModelView.RessursType
{
    public class VerktøyViewModel : RessursViewModel
    {
        public enum Verktøytype
        {
            ElektriskHåndverktøy, Måleutstyr, TungtUtstyr, ManueltVerktøy, Annet
        }

        public Verktøytype Type { get; set; } = Verktøytype.ElektriskHåndverktøy;

        public VerktøyViewModel()
        {
            Kategori = RessursViewModel.RessursType.Verktøy;
        }
    }
}