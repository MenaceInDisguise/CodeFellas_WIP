namespace Prosjekt.Models.ModelView.ResourceType
{
    public class ClothingViewModel : ResourceViewModel
    {
        public enum ClothingSize
        {
            XS, S, M, L, XL, XXL
        }

        public ClothingSize Size { get; set; } = ClothingSize.M;

        public ClothingViewModel()
        {
            Category = ResourceViewModel.ResourceType.Clothing;
        }
    }
}