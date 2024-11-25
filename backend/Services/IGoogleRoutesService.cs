using shared.Models;

namespace backend.Services;

public interface IGoogleRoutesService
{
    Task<(double distance, double duration, string polyline)> GetRouteDataAsync(List<Address> addresses);
    Task<Address> GeocodeAddressAsync(string address);
}