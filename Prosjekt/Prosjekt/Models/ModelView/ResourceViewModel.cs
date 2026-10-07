using System.ComponentModel.DataAnnotations;

namespace Prosjekt.Models.ModelView
{
    //ViewModel that contains information about a resource that the user can register.
    public class ResourceViewModel
    {
        public enum ResourceType
        {
            Provisions,
            Clothing,
            Tool,
            Vehicle,
            Materials,
            Other
        }

        //The name of the resource.
        public string Name { get; set; } = string.Empty;

        //Description of the resource.
        public string Description { get; set; } = string.Empty;

        //Quantity of the resource that is available.
        public int Quantity { get; set; } = 0;

        //The latitude of the position.
        [Display(Name = "Latitude")]
        public string Latitude { get; set; } = string.Empty;

        //The longitude of the position.
        [Display(Name = "Longitude")]
        public string Longitude { get; set; } = string.Empty;

        // The category the resource belongs to.
        public ResourceType Category { get; set; } = ResourceType.Other;
    }
}
