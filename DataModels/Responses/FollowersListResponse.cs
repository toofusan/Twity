using System;
using Twity.DataModels.Core;

namespace Twity.DataModels.Responses
{
    [Serializable]
    public class FollowersListResponse
    {
        public TweetUser[] users;
    }
}
