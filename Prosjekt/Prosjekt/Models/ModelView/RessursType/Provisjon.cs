using System.ComponentModel.DataAnnotations;

namespace Prosjekt.Models.ModelView.RessursType
{
    public class ProvisjonViewModel : RessursViewModel
    {
        public enum Provisjonstype
        {
            Vann,

            [Display(Name = "Mat (1 porsjon)")]
            MatEnPorsjon
        }

        public Provisjonstype Type { get; set; } = Provisjonstype.MatEnPorsjon;

        public ProvisjonViewModel()
        {
            Kategori = RessursViewModel.RessursType.Provisjon;
        }
    }
}