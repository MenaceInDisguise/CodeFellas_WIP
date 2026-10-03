using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Prosjekt.Models.ModelView
{
    public class RessursOppretterViewModel
    {
        public double Latitude { get; set; } = 0.0;
        public double Longitude { get; set; } = 0.0;

        public List<RessursViewModel> RessursListe { get; set; } = new List<RessursViewModel>();
    }
}
