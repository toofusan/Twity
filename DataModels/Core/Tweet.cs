using System;

namespace Twity.DataModels.Core
{
    [Serializable]
    public class Tweet : TweetObjectWithUser
    {
        public TweetObjectWithUser retweeted_status;
    }
}
