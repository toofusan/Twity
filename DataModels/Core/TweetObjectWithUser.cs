using System;

namespace Twity.DataModels.Core
{
    [Serializable]
    public class TweetObjectWithUser : TweetObject
    {
        public TweetUser user;
    }
}
