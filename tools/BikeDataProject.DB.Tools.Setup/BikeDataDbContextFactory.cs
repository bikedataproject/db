using BDPDatabase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BikeDataProject.DB.Tools.Setup
{
    public class BikeDataDbContextFactory : IDesignTimeDbContextFactory<BikeDataDbContext>
    {
        public BikeDataDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BikeDataDbContext>();
            optionsBuilder.UseNpgsql("Host=127.0.0.1;Port=5433;Database=bikedata;Username=postgres;Password=mixbeton");

            return new BikeDataDbContext(optionsBuilder.Options);
        }
    }
}