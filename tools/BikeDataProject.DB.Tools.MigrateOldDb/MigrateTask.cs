using System;
using System.Linq;
using System.Threading.Tasks;
using BDPDatabase;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace BikeDataProject.DB.Tools.MigrateOldDb
{
    public class MigrateTask
    {
        private readonly ILogger<MigrateTask> _logger;
        private readonly BikeDataDbContext _dbContext;
        private readonly MigrateTaskConfiguration _configuration;
        private readonly NpgsqlConnection _connection;
        
        public MigrateTask(BikeDataDbContext dbContext, MigrateTaskConfiguration configuration, ILogger<MigrateTask> logger)
        {
            _configuration = configuration;
            _dbContext = dbContext;
            _logger = logger;
            
            _connection = new NpgsqlConnection(configuration.LegacyDbConnectionString);
        }
        
        public async Task Run()
        {
            // open db connection.
            _connection.Open();
            _connection.TypeMapper.UseNetTopologySuite();
            
            // migrate users.
            await this.MigrateUsers();
            
            // migrate contributions.
            
        }

        private async Task MigrateUsers()
        {
            var getUsersSql =
                "select user_id, provider, provider_id from credentials";
            
            var cmd = _connection.CreateCommand();
            cmd.CommandText = getUsersSql;
            await using var reader = await cmd.ExecuteReaderAsync();
            while (reader.Read())
            {
                var userGuid = reader.GetGuid(0);

                var user = await _dbContext.Users.Where(x => x.UserIdentifier == userGuid).FirstOrDefaultAsync();
                if (user != null) continue; // user already exists, presumably already migrated.

                var provider = reader["provider"] as string;
                var providerUser = reader["provider_id"] as string;
                
                user = new User()
                {
                    UserIdentifier = userGuid,
                    Provider = $"legacy\\{provider}",
                    ProviderUser = providerUser
                };
                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();
                
                _logger.LogInformation($"Migrated user {userGuid}: {user.Provider}");
            }
        }
    }
}