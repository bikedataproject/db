using System;
using System.Collections.Generic;

namespace BDPDatabase
{
    /// <summary>
    /// Represents a user.
    /// </summary>
    public class User
    {
        /// <summary>
        /// The id of the user in the db.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// A user identifier, used to identify the user in the outside world.
        /// </summary>
        public Guid UserIdentifier { get; set; }

        /// <summary>
        /// The provider string.
        /// </summary>
        /// <remarks>
        /// For example:
        /// - strava: this user if from strava
        /// - legacy\strava: migrated data.
        /// </remarks>
        public string Provider { get; set; }

        /// <summary>
        /// The user id relative to the provider.
        /// </summary>
        public string ProviderUser { get; set; }
        
        /// <summary>
        /// The access token for the provider.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// The refresh token for the provider.
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// Timestamp for when the token was created.
        /// </summary>
        public DateTime TokenCreationDate { get; set; }

        public int ExpiresAt { get; set; }

        public int ExpiresIn { get; set; }

        /// <summary>
        /// When true the users' history was fetched.
        /// </summary>
        public bool IsHistoryFetched {get; set; }

        /// <summary>
        /// The contributions associated with this user.
        /// </summary>
        public List<UserContribution> UserContributions { get; set; }

    }
}