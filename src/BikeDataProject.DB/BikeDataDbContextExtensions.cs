using System;
using System.Collections.Generic;
using System.Linq;

namespace BikeDataProject.DB
{
    public static class BikeDataDbContextExtensions
    {
        public static int GetTotalDistance(this BikeDataDbContext dbContext)
        {
            return dbContext.Contributions.Select(c => c.Distance).Sum();
        }

        public static int GetUserId(this BikeDataDbContext dbContext, Guid userId)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.UserIdentifier == userId);
            return user != null ? user.Id : 0;
        }

        public static void AddContribution(this BikeDataDbContext dbContext, Contribution contribution)
        {
            dbContext.Contributions.Add(contribution);
        }

        public static void AddUserContribution(this BikeDataDbContext dbContext, UserContribution userContribution)
        {
            dbContext.UserContributions.Add(userContribution);
        }

        public static User ContainsProviderUser(this BikeDataDbContext dbContext, string providerUser)
        {
            return dbContext.Users.FirstOrDefault(u => u.ProviderUser == providerUser);
        }

        public static void AddUser(this BikeDataDbContext dbContext, User user)
        {
            dbContext.Users.Add(user);
        }

        public static IEnumerable<int> GetContributionsIds(this BikeDataDbContext dbContext, int userIdentifier)
        {
            return dbContext.UserContributions.Where(uc => uc.UserId == userIdentifier).Select(uc => uc.ContributionId);
        }

        public static void DeleteContributions(this BikeDataDbContext dbContext, List<Contribution> contributions, List<UserContribution> userContributions)
        {
            dbContext.UserContributions.RemoveRange(userContributions);
            dbContext.Contributions.RemoveRange(contributions);

            dbContext.SaveChanges();
        }

        public static IEnumerable<UserContribution> GetUserContributionsByContributionIds(this BikeDataDbContext dbContext, IEnumerable<int> contributionIds)
        {
            return dbContext.UserContributions.Where(uc => contributionIds.Contains(uc.ContributionId));
        }

        public static void SaveChanges(this BikeDataDbContext dbContext) => dbContext.SaveChanges();
    }
}