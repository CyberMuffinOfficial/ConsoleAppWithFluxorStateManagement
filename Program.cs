global using Fluxor;
global using ConsoleAppWithFluxorStateManagement.Services;
global using ConsoleAppWithFluxorStateManagement.Shared;
using Microsoft.Extensions.DependencyInjection;
using ConsoleAppWithFluxorStateManagement.Store.Middlewares.Logging;

namespace ConsoleAppWithFluxorStateManagement
{
    public class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddScoped<App>();
            services.AddFluxor(o => o
                .ScanAssemblies(typeof(Program).Assembly)
                .AddMiddleware<LoggingMiddleware>());
            services.AddScoped<IWeatherForecastService, WeatherForecastService>();

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            var app = serviceProvider.GetRequiredService<App>();
            app.Run();
        }
    }
}