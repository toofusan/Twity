using System;
using Twity.DataModels.Core;

namespace Twity.DataModels.DirectMessages
{
    [Serializable]
    public class DirectMessage
    {
        public string created_at;
        public Entities.Entities entities;
        public long id;
        public string id_str;
        public TweetUser recipient;
        public long recipient_id;
        public string recipient_screen_name;
        public TweetUser sender;
        public long sender_id;
        public string sender_screen_name;
        public string text;
    }
}
