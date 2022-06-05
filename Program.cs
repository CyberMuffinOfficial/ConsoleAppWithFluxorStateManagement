global using Fluxor;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleAppWithFluxorStateManagement
{
    public class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddScoped<App>();
            services.AddFluxor(o => o
              .ScanAssemblies(typeof(Program).Assembly));

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            var app = serviceProvider.GetRequiredService<App>();
            app.Run();
        }
    }
}