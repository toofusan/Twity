using System;

namespace Twity.DataModels.StreamMessages
{
    [Serializable]
    public class StatusWithheld
    {
        public long id;
        public long user_id;
        public string[] withheld_in_countries;
    }
}
