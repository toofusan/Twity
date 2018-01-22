using System;

namespace Twity.DataModels.Core
{
    [Serializable]
    public class List
    {
        public string slug;
        public string name;
        public string created_at;
        public string uri;
        public int subscriber_count;
        public long id;
        public string id_str;
        public int member_count;
        public string mode;
        public string full_name;
        public string description;
        public TweetUser user;
        public bool following;
    }
}
