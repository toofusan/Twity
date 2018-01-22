using System;
using Twity.DataModels.Core;

namespace Twity.DataModels.StreamMessages
{
    [Serializable]
    public class StreamEvent
    {
        public string event_name;
        public string created_at;
        public TweetUser target;
        public TweetUser source;
        public string target_object;
    }
}
