using shared.Models;

namespace backend.Services;

public interface IRouteOptimizationService
{
    Task<RouteResult> OptimizeRouteAsync(List<string> addresses);
}