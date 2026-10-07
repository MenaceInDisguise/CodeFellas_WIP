using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Prosjekt.Models.ModelView
{
    public class ResourceCreatorViewModel
    {
        public double Latitude { get; set; } = 0.0;
        public double Longitude { get; set; } = 0.0;

        public List<ResourceViewModel> ResourceList { get; set; } = new List<ResourceViewModel>();
    }
}
