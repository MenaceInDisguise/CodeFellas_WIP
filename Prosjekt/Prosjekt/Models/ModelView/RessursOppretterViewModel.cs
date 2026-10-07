using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Prosjekt.Models.ModelView
{
    public class RessursOppretterViewModel
    {
        public double? Latitude { get; set; } 
        public double? Longitude { get; set; } 

        public List<RessursViewModel> RessursListe { get; set; } = new List<RessursViewModel>();
    }
}
