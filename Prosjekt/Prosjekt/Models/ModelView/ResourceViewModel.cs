using System.ComponentModel.DataAnnotations;

namespace Prosjekt.Models.ModelView
{
    //ViewModel that contains information about a resource that the user can register.
    public class ResourceViewModel
    {
        public enum ResourceType
        {
            [Display(Name = "Provisjoner")]
            Provisions,
            [Display(Name = "Klær")]
            Clothing,
            [Display(Name = "Verktøy")]
            Tool,
            [Display(Name = "Kjøretøy")]
            Vehicle,
            [Display(Name = "Materialer")]
            Materials,
            [Display(Name = "Annet")]
            Other
        }

        //The name of the resource.
        [Display(Name = "Navn")]
        public string Name { get; set; } = string.Empty;

        //Description of the resource.
        [Display(Name = "Beskrivelse")]
        public string Description { get; set; } = string.Empty;

        //Quantity of the resource that is available.
        [Display(Name = "Antall")]
        public int Quantity { get; set; } = 0;

        //The latitude of the position.
        [Display(Name = "Breddegrad")]
        public string Latitude { get; set; } = string.Empty;

        //The longitude of the position.
        [Display(Name = "Lengdegrad")]
        public string Longitude { get; set; } = string.Empty;

        // The category the resource belongs to.
        [Display(Name = "Kategori")]
        public ResourceType Category { get; set; } = ResourceType.Other;
    }
}
