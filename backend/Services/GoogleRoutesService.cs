
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using shared.Models;

namespace backend.Services;

public class GoogleRoutesService : IGoogleRoutesService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _cache;
        private readonly ILogger<GoogleRoutesService> _logger;

        public GoogleRoutesService(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IMemoryCache cache,
            ILogger<GoogleRoutesService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _cache = cache;
            _logger = logger;
        }

        public async Task<Address> GeocodeAddressAsync(string address)
        {
            if (_cache.TryGetValue(address, out Address cachedAddress))
            {
                _logger.LogDebug("cache hit for address: {Address}", address);
                return cachedAddress;
            }
            
            var client = _httpClientFactory.CreateClient("GoogleRoutesClient");
            var apiKey = _configuration["GoogleApiKey"];
            var requestUri = $"geocode/json?address={Uri.EscapeDataString(address)}&key={apiKey}";

            var response = await client.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(content);
            var location = json["results"]?.First()?["geometry"]?["location"];

            if (location == null)
            {
                _logger.LogError("unable to geocode address: {Address}", address);
                throw new Exception("unable to geocode address");
            }

            var lat = (double)location["lat"];
            var lng = (double)location["lng"];
            var result = new Address
            {
                Value = address,
                Latitude = lat,
                Longitude = lng
            };

            _cache.Set(address, result, TimeSpan.FromHours(1));
            _logger.LogDebug("Geocoded address cached: {Address}", address);
            return result;
        }

        public async Task<(double distance, double duration, string polyline)> GetRouteDataAsync(List<Address> addresses)
        {
            if (addresses == null || addresses.Count < 2)
            {
                _logger.LogError("GetRouteDataAsync called with invalid addresses");
                throw new ArgumentException("at least two addresses are required to get route data");
            }
            
            var client = _httpClientFactory.CreateClient("GoogleRoutesClient");
            var apiKey = _configuration["GoogleApiKey"];
            var waypoints = string.Join("|", addresses.Skip(1).Take(addresses.Count - 2).Select(a => $"{a.Latitude},{a.Longitude}"));

            var origin = addresses.First();
            var destination = addresses.Last();
            var intermediateWaypoints = string.Join("|", addresses.Skip(1).Take(addresses.Count - 2).Select(a => $"{a.Latitude},{a.Longitude}"));

            var requestUri = $"directions/json?origin={origin.Latitude},{origin.Longitude}&destination={destination.Latitude},{destination.Longitude}&waypoints={intermediateWaypoints}&key={apiKey}";

            var response = await client.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(content);
            var route = json["routes"]?.First();

            if (route == null)
            {
                _logger.LogError("unable to retrieve route data");
                throw new Exception("unable to get route data");
            }

            var legs = route["legs"];
            double totalDistance = legs.Sum(leg => (double)leg["distance"]["value"]);
            double totalDuration = legs.Sum(leg => (double)leg["duration"]["value"]);
            var polyline = (string)route["overview_polyline"]["points"];

            _logger.LogDebug("route data fetched: distance={Distance} meters, duration={Duration} seconds", totalDistance, totalDuration);
            return (totalDistance / 1000, totalDuration / 60, polyline);
        }
    }