using System.ComponentModel.DataAnnotations;

namespace Prosjekt.Models.ModelView.ResourceType
{
    public class ToolViewModel : ResourceViewModel
    {
        public enum ToolType
        {
            [Display(Name = "Elektrisk håndverktøy")]
            ElectricHandTool,
            [Display(Name = "Måleutstyr")]
            MeasuringEquipment,
            [Display(Name = "Tungt utstyr")]
            HeavyEquipment,
            [Display(Name = "Manuelt verktøy")]
            ManualTool,
            [Display(Name = "Annet")]
            Other
        }

        public ToolType Type { get; set; } = ToolType.ElectricHandTool;

        public ToolViewModel()
        {
            Category = ResourceViewModel.ResourceType.Tool;
        }
    }
}