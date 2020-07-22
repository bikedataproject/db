using System.Threading.Tasks;
using BDPDatabase;
using Microsoft.EntityFrameworkCore;

namespace BikeDataProject.DB.Tools.Setup
{
    public class SetupTask
    {
        private readonly BikeDataDbContext _dbContext;

        public SetupTask(BikeDataDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Run()
        {
            // first apply migrations.
            await _dbContext.Database.MigrateAsync();
        }
    }
}