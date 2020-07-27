using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BDPDatabase;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
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
            await this.MigrateContributions();
        }

        private async Task MigrateUsers()
        {
            var getUsersSql =
                "select users.user_id, credentials.provider, credentials.provider_id from users left join credentials on users.user_id = credentials.user_id order by users.user_id";
            
            var cmd = _connection.CreateCommand();
            cmd.CommandText = getUsersSql;
            await using var reader = await cmd.ExecuteReaderAsync();
            while (reader.Read())
            {
                var userGuid = reader.GetGuid(0);

                var user = await _dbContext.Users.Where(x => x.UserIdentifier == userGuid).FirstOrDefaultAsync();
                if (user != null) 
                {
                    _logger.LogInformation($"User exists {userGuid}: {user.Provider}");
                    continue; // user already exists, presumably already migrated.
                }

                var provider = reader["provider"] as string;
                if (!string.IsNullOrWhiteSpace(provider)) provider = $"legacy\\{provider}";
                var providerUser = reader["provider_id"] as string;
                
                user = new User
                {
                    UserIdentifier = userGuid,
                    Provider = provider,
                    ProviderUser = providerUser
                };
                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();
                
                _logger.LogInformation($"Migrated user {userGuid}: {user.Provider}");
            }
        }

        private async Task MigrateContributions()
        {
            var postGisWriter = new PostGisWriter();
            
            var sql =
                "select user_id, points_geom, points_time, distance, duration from contributions order by user_id";
            
            var cmd = _connection.CreateCommand();
            cmd.CommandText = sql;
            await using var reader = await cmd.ExecuteReaderAsync();
            var values = new object[reader.FieldCount];
            (User user, List<Contribution> contributions, bool exists) currentUser = (null, new List<Contribution>(), false);
            while (reader.Read())
            {
                var userGuid = reader.GetGuid(0);

                if (currentUser.user == null ||
                    currentUser.user.UserIdentifier != userGuid)
                {
                    // flush queue here if any data.
                    var count = currentUser.contributions.Count;
                    await this.Flush(currentUser);
                    if (count > 0 && currentUser.user != null) _logger.LogInformation($"Migrated contributions for user {currentUser.user.UserIdentifier}: {count}");

                    // start new user.
                    currentUser = (null, currentUser.contributions, false);
                    var user = await _dbContext.Users.Where(x => x.UserIdentifier == userGuid).FirstOrDefaultAsync();
                    if (user == null) continue; // user not migrated (?).
                    
                    // check if there are contributions.
                    var existing = await _dbContext.UserContributions.Where(x => x.UserId == user.Id).FirstOrDefaultAsync();
                    currentUser = (user, currentUser.contributions,  existing != null);
                }
                
                // if user exists, skip the contribution.
                if (currentUser.exists) continue;

                // get next contribution.
                reader.GetValues(values);
                if (!(values[1] is LineString pointsGeometry)) throw new Exception("Invalid geometry.");
                if (!(values[2] is DateTime[] pointsTimeStamp) || pointsTimeStamp.Length == 0) throw new Exception("Invalid timestamp array.");
                if (!(values[3] is int distance)) throw new Exception("Invalid distance");
                if (!(values[4] is int duration)) throw new Exception("Invalid duration");
                
                var pointsTime = new DateTime[pointsTimeStamp.Length];
                for (var p = 0; p < pointsTimeStamp.Length; p++)
                {
                    pointsTime[p] = pointsTimeStamp[p];
                }

                var contribution = new Contribution()
                {
                    Distance = distance,
                    Duration = duration,
                    PointsGeom = postGisWriter.Write(pointsGeometry),
                    PointsTime = pointsTime,
                    TimeStampStart = pointsTime[0],
                    TimeStampStop = pointsTime[^1],
                    UserAgent = string.Empty
                };
                currentUser.contributions.Add(contribution);
            }
            
            // flush queue here if any data.
            await this.Flush(currentUser);
        }

        private async Task Flush((User user, List<Contribution> contributions, bool exists) currentUser)
        {                    
            // flush queue here if any data.
            if (currentUser.contributions.Count > 0 &&
                currentUser.user != null)
            {
                foreach (var contribution in currentUser.contributions)
                {
                    await _dbContext.Contributions.AddAsync(contribution);
                }
                await _dbContext.SaveChangesAsync();
                        
                // add user-contributions.
                foreach (var c in currentUser.contributions)
                {
                    await _dbContext.UserContributions.AddAsync(new UserContribution()
                    {
                        UserId = currentUser.user.Id,
                        ContributionId = c.ContributionId
                    });
                }
                await _dbContext.SaveChangesAsync();

                // flush queue.
                currentUser.contributions.Clear();
            }
        }
    }
}