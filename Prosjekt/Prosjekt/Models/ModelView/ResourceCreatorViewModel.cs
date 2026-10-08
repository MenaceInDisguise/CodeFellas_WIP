using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Prosjekt.Models.ModelView
{
    public class ResourceCreatorViewModel
    {
        [Display(Name = "Breddegrad")]
        public double Latitude { get; set; } = 0.0;
        [Display(Name = "Lengdegrad")]
        public double Longitude { get; set; } = 0.0;

        [Display(Name = "Ressurser")]
        public List<ResourceViewModel> ResourceList { get; set; } = new List<ResourceViewModel>();
    }
}
