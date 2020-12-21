using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BikeDataProject.DB
{
    class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        // EF Core uses this method at design time to access the DbContext
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(x => x.AddJsonFile("appsettings.json"))
                .ConfigureServices((hc, s) =>
                {
                    var connectionString = File.ReadAllText(hc.Configuration["DB"]);
                    
                    s.AddDbContext<BikeDataDbContext>(o => o.UseNpgsql(connectionString),
                        ServiceLifetime.Singleton);
                    
                    s.AddHostedService<Startup>();
                });
        }
        
        public class Startup : IHostedService
        {
            private readonly BikeDataDbContext _db;
            
            public Startup(BikeDataDbContext db)
            {
                _db = db;
            }
            
            public async Task StartAsync(CancellationToken cancellationToken)
            {
                await _db.Contributions.FirstOrDefaultAsync(cancellationToken: cancellationToken);

                return;
            }

            public Task StopAsync(CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }
        }
    }
}