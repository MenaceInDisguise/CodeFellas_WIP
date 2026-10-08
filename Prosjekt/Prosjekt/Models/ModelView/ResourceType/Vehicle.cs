using System.ComponentModel.DataAnnotations;

namespace Prosjekt.Models.ModelView.ResourceType
{
    public class VehicleViewModel : ResourceViewModel
    {
        public enum VehicleType
        {
            [Display(Name = "Bil")]
            Car,
            [Display(Name = "Buss")]
            Bus,
            [Display(Name = "Sykkel")]
            Bicycle,
            [Display(Name = "Motorcykel")]
            Motorcycle,
            [Display(Name = "Traktor")]
            Tractor,
            [Display(Name = "Lastebil")]
            Truck,
            [Display(Name = "Dumper")]
            Dozer,
            [Display(Name = "Kran")]
            Excavator,
            [Display(Name = "Annet")]
            Other
        }

        public VehicleType Type { get; set; } = VehicleType.Other;
        public string LicensePlate { get; set; } = string.Empty;

        public VehicleViewModel()
        {
            Category = ResourceViewModel.ResourceType.Vehicle;
        }
    }
}