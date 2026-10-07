namespace Prosjekt.Models.ModelView.ResourceType
{
    public class VehicleViewModel : ResourceViewModel
    {
        public enum VehicleType
        {
            Car,
            Bus,
            Bicycle,
            Motorcycle, Tractor,
            Truck,
            Dozer,
            Excavator,
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