using System.Net.Http.Json;
using shared.Models;

namespace frontend.Services;

public class RouteOptimizationService : IRouteOptimizationService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<RouteOptimizationService> _logger;

    public RouteOptimizationService(HttpClient httpClient, ILogger<RouteOptimizationService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<RouteResult?> OptimizeRouteAsync(List<string> addresses)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/RouteOptimization/optimize", addresses);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<RouteResult>();
                _logger.LogInformation("get route request successful.");
                return result;
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"API Error: {error}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "exception while getting optimized route.");
            throw;
        }
    }
}