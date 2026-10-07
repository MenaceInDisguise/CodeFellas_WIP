using Prosjekt.Models.ModelView;

namespace Prosjekt.Models.ModelView.ResourceType
{
    public class MaterialsViewModel : ResourceViewModel
    {
        public enum MaterialType
        {
            Wood,
            Metal,
            Plastic,
            Glass,
            Gravel,
            Stone,
            Soil,
            Sand,
            Other
        }

        public MaterialType Type { get; set; } = MaterialType.Other;

        public MaterialsViewModel()
        {
            Category = ResourceViewModel.ResourceType.Materials;
        }
    }
}
