using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BC21
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Create service collection and configure our services
            var services = ConfigureServices();

            // Generate a provider
            var serviceProvider = services.BuildServiceProvider();
            serviceProvider.GetService<ConsoleApplication>().Run();
        }

        private static IServiceCollection ConfigureServices()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            // Add the config to our DI container for later user
            IServiceCollection services = new ServiceCollection()
                .AddSingleton<IConfiguration>(builder)
                .AddSingleton<ConsoleApplication>()
                .AddSingleton<IBitCoinValidationService, BitCoinValidationService>()
                .AddOptions()
                .Configure<ConfigurationParams>(builder.GetSection("ConfigurationParam"))
                .Configure<ConfigurationString>(builder.GetSection("ConfigurationString"))
                .Configure<Block>(builder.GetSection("BlockInfo"))
                .AddAutoMapper(typeof(Program));
            return services;
        }
    }
}
