using System;
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
                var c1 = await _db.Contributions.FirstOrDefaultAsync(cancellationToken: cancellationToken);

                if (c1 == null)
                {
                    await _db.Contributions.AddAsync(new Contribution()
                    {
                        Distance = 100,
                        Duration = 100,
                        PointsGeom = Array.Empty<byte>(),
                        PointsTime = Array.Empty<DateTime>(),
                        UserAgent = "nothing",
                        TimeStampStart = DateTime.Now.AddDays(-1),
                        TimeStampStop = DateTime.Now
                    });
                    await _db.SaveChangesAsync();
                }

                return;
            }

            public Task StopAsync(CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }
        }
    }
}