namespace shared.Models;

public class RouteResult
{
    public List<Address> OptimizedAddresses { get; set; }
    public double TotalDistance { get; set; } // in kilometers
    public double EstimatedTime { get; set; } // in minutes
    public string Polyline { get; set; } // Encoded polyline for the route
}