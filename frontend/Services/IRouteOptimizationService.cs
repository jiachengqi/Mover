using shared.Models;

namespace frontend.Services;

public interface IRouteOptimizationService
{
    Task<RouteResult?> OptimizeRouteAsync(List<string> addresses);
}