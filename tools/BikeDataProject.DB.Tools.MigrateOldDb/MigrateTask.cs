using System.Threading.Tasks;
using BDPDatabase;
using Microsoft.Extensions.Logging;

namespace BikeDataProject.DB.Tools.MigrateOldDb
{
    public class MigrateTask
    {
        private readonly ILogger<MigrateTask> _logger;
        private readonly BikeDataDbContext _dbContext;
        private readonly MigrateTaskConfiguration _configuration;
        
        public MigrateTask(BikeDataDbContext dbContext, MigrateTaskConfiguration configuration, ILogger<MigrateTask> logger)
        {
            _configuration = configuration;
            _dbContext = dbContext;
            _logger = logger;
        }
        
        public async Task Run()
        {
            // TODO: do stuff!
        }
    }
}