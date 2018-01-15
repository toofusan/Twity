using System;

namespace Twity.DataModels.Entities
{
    [Serializable]
    public class UserMention
    {
        public long id;
        public string id_str;
        public string screen_name;
        public string name;
    }
}
