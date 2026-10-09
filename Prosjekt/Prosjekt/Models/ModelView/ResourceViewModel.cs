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

        [Display(Name = "Navn")]
        [Required(ErrorMessage = "Navn må fylles ut. ")]
        public string Name { get; set; } = string.Empty;


        [Display(Name = "Beskrivelse")]
        [Required(ErrorMessage = "Beskrivelse må fylles ut.")]
        public string Description { get; set; } = string.Empty;


        [Display(Name = "Antall")]
        [Required(ErrorMessage = "Antall må være minst 1.")]
        public int Quantity { get; set; } = 0;


        [Display(Name = "Breddegrad")]
        [Required(ErrorMessage = "Du må legge inn breddegrad")]
        public double? Latitude { get; set; }


        [Display(Name = "Lengdegrad")]
        [Required(ErrorMessage = "Du må legge inn lengdegrad")]
        public double? Longitude { get; set; }


        [Display(Name = "Kategori")]
        [Required(ErrorMessage = "Du må velge en kategori")]
        public ResourceType Category { get; set; } = ResourceType.Other;
    }
}