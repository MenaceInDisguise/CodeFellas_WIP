using System.ComponentModel.DataAnnotations;

namespace Prosjekt.Models.ModelView.ResourceType
{
    public class ProvisionsViewModel : ResourceViewModel
    {
        public enum ProvisionsType
        {
            [Display(Name = "Vann")]
            Water,

            [Display(Name = "Food (1 porsjon)")]
            FoodOneServing
        }

        public ProvisionsType Type { get; set; } = ProvisionsType.FoodOneServing;

        public ProvisionsViewModel()
        {
            Category = ResourceViewModel.ResourceType.Provisions;
        }
    }
}