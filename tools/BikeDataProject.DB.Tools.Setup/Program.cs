using System;
using System.Threading.Tasks;
using BikeDataProject.DB.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace BikeDataProject.DB.Tools.Setup
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // read configuration.
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddUserSecrets<SetupTask>()
                .Build();
            
            // setup serilog logging (from configuration).
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
            
            // get database connection.
            var connectionString = configuration["BikeDataProject:ConnectionString"];
            
            // setup DI.
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddSingleton<SetupTask>()
                .AddDbContext<BikeDataDbContext>(
                    options => options.UseNpgsql(connectionString))
                .BuildServiceProvider();

            // add serilog logger to DI provider.
            serviceProvider.GetRequiredService<ILoggerFactory>()
                .AddSerilog();
            
            // do the actual work here
            var task = serviceProvider.GetService<SetupTask>();
            await task.Run();
        }
    }
}
