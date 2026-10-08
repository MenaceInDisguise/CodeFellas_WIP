using System.ComponentModel.DataAnnotations;

namespace Prosjekt.Models.ModelView
{
    public class ResourceViewModel
    {
        public enum ResourceType
        {
            [Display(Name = "Provisjon")]
            Provision,
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

        [Required(ErrorMessage = "Navn må fylles ut. ")]
        [Display(Name = "Navn")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Beskrivelse")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Antall må være minst 1.")]
        [Display(Name = "Antall")]
        public int Quantity { get; set; } = 0;

        [Display(Name = "Breddegrad")]
        public double? Latitude { get; set; }

        [Display(Name = "Lengdegrad")]
        public double? Longitude { get; set; }

        [Display(Name = "Kategori")]
        public ResourceType Category { get; set; } = ResourceType.Other;
    }
}