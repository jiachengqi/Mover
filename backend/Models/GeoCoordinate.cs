namespace backend.Models;

public class GeoCoordinate
{
    private const double EarthRadius = 6371e3; // in meters

    public double Latitude { get; }
    public double Longitude { get; }

    public GeoCoordinate(double latitude, double longitude)
    {
        Latitude = latitude * (Math.PI / 180.0);
        Longitude = longitude * (Math.PI / 180.0);
    }

    public double GetDistanceTo(GeoCoordinate other)
    {
        var dLat = other.Latitude - Latitude;
        var dLon = other.Longitude - Longitude;

        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(Latitude) * Math.Cos(other.Latitude) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        return EarthRadius * c; // in meters
    }
}