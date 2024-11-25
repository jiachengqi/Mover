using backend.Models;
using shared.Models;

namespace backend.Services;

public class RouteOptimizationService : IRouteOptimizationService
    {
        private readonly IGoogleRoutesService _googleRoutesService;
        private readonly ILogger<RouteOptimizationService> _logger;

        public RouteOptimizationService(
            IGoogleRoutesService googleRoutesService,
            ILogger<RouteOptimizationService> logger)
        {
            _googleRoutesService = googleRoutesService;
            _logger = logger;
        }

        public async Task<RouteResult> OptimizeRouteAsync(List<string> addresses)
        {
            var geocodedAddresses = new List<Address>();
            foreach (var address in addresses)
            {
                var geocoded = await _googleRoutesService.GeocodeAddressAsync(address);
                geocodedAddresses.Add(geocoded);
            }
            
            var optimizedAddresses = OptimizeUsingNearestNeighbor(geocodedAddresses);

            var (distance, duration, polyline) = await _googleRoutesService.GetRouteDataAsync(optimizedAddresses);
            _logger.LogInformation("route data retrieved: distance={Distance} km, duration={Duration} minutes", distance, duration);

            return new RouteResult
            {
                OptimizedAddresses = optimizedAddresses,
                TotalDistance = Math.Round(distance, 2),
                EstimatedTime = Math.Round(duration, 2),
                Polyline = polyline
            };
        }

        private List<Address> OptimizeUsingNearestNeighbor(List<Address> addresses)
        {
            var optimizedRoute = new List<Address>();
            var remainingAddresses = new List<Address>(addresses);
            var currentAddress = remainingAddresses.First();
            optimizedRoute.Add(currentAddress);
            remainingAddresses.RemoveAt(0);

            while (remainingAddresses.Any())
            {
                var nearest = FindNearestAddress(currentAddress, remainingAddresses);
                optimizedRoute.Add(nearest);
                remainingAddresses.Remove(nearest);
                currentAddress = nearest;
            }

            _logger.LogDebug("Optimized route: {Route}", string.Join(" -> ", optimizedRoute.Select(a => a.Value)));
            return optimizedRoute;
        }

        private Address FindNearestAddress(Address current, List<Address> addresses)
        {
            Address nearest = null;
            double shortestDistance = double.MaxValue;

            foreach (var address in addresses)
            {
                var distance = GetDistance(current, address);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    nearest = address;
                }
            }

            _logger.LogDebug("nearest address to {Current} is {Nearest} with distance {Distance} meters.", current.Value, nearest.Value, shortestDistance);
            return nearest;
        }

        private double GetDistance(Address a1, Address a2)
        {
            var sCoord = new GeoCoordinate(a1.Latitude, a1.Longitude);
            var eCoord = new GeoCoordinate(a2.Latitude, a2.Longitude);
            return sCoord.GetDistanceTo(eCoord);
        }
    }