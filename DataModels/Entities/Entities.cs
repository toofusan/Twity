using System;

namespace Twity.DataModels.Entities
{
    [Serializable]
    public class Entities
    {
        public Media[] media;
        public UserMention[] user_mentions;
        public HashTag[] hashtags;
        public Url[] urls;
    }
}
