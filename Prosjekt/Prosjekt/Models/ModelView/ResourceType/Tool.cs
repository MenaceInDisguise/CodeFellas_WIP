namespace Prosjekt.Models.ModelView.ResourceType
{
    public class ToolViewModel : ResourceViewModel
    {
        public enum ToolType
        {
            ElectricHandTool, MeasuringEquipment, HeavyEquipment, ManualTool, Other
        }

        public ToolType Type { get; set; } = ToolType.ElectricHandTool;

        public ToolViewModel()
        {
            Category = ResourceViewModel.ResourceType.Tool;
        }
    }
}