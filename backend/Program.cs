using backend.Middlewares;
using backend.Services;
using Microsoft.Extensions.Configuration.Yaml;
using Polly;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddYamlFile("appsettings.yaml", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<IGoogleRoutesService, GoogleRoutesService>();
builder.Services.AddScoped<IRouteOptimizationService, RouteOptimizationService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient("GoogleRoutesClient", client =>
    {
        client.BaseAddress = new Uri("https://maps.googleapis.com/maps/api/");
    })
    .AddTransientHttpErrorPolicy(policyBuilder => policyBuilder.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(500)));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI();
app.Run();