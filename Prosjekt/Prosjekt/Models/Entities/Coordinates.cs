namespace Prosjekt.Models.Entities;

public readonly record struct Coordinates(double Latitude, double Longitude)
{
    // Validerer at verdiene er innenfor grensene for standard GPS (WGS 84)
    public bool IsValid() =>
        Latitude is >= -90.0 and <= 90.0 &&
        Longitude is >= -180.0 and <= 180.0;

    public override string ToString() => $"Lat: {Latitude}, Lng: {Longitude}";
}