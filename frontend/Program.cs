using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using frontend;
using frontend.Services;
using GoogleMapsComponents;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5258/") });

builder.Services.AddScoped<IRouteOptimizationService, RouteOptimizationService>();

builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();

builder.Services.AddBlazorGoogleMaps("AIzaSyBSOJZyFm68yiNOwJmrdX4iksVGSNMeYno");
builder.Services.AddRadzenComponents();

await builder.Build().RunAsync();