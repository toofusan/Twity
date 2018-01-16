using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twity.DataModels.Core;

namespace Twity.DataModels.Responses
{
    [Serializable]
    public class FollowersListResponse
    {
        public TweetUser[] users;
    }
}
