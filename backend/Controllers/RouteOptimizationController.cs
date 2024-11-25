using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RouteOptimizationController : ControllerBase
{
    private readonly IRouteOptimizationService _routeOptimizationService;
    private readonly ILogger<RouteOptimizationController> _logger;

    public RouteOptimizationController(
        IRouteOptimizationService routeOptimizationService,
        ILogger<RouteOptimizationController> logger)
    {
        _routeOptimizationService = routeOptimizationService;
        _logger = logger;
    }

    [HttpPost("optimize")]
    public async Task<IActionResult> OptimizeRoute([FromBody] List<string> addresses)
    {
        if (addresses == null || addresses.Count < 2)
        {
            return BadRequest("at least two addresses are required");
        }
        
        var result = await _routeOptimizationService.OptimizeRouteAsync(addresses);
        _logger.LogInformation("optimized route for {Count} addresses.", addresses.Count);
        return Ok(result);
    }
}