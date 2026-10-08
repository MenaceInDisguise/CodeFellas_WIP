namespace Prosjekt.Models.Entities;

public readonly record struct Coordinates(double Latitude, double Longitude)
{
    /// <summary>
    /// Indicates whether Latitude and Longitude are within valid geographic ranges.
    /// </summary>
    /// <returns>true if Latitude is between -90.0 and 90.0 and Longitude is between -180.0 and 180.0; otherwise, false.</returns>
    public bool IsValid() =>
        Latitude is >= -90.0 and <= 90.0 &&
        Longitude is >= -180.0 and <= 180.0;

    public override string ToString() => $"Lat: {Latitude}, Lng: {Longitude}";
}