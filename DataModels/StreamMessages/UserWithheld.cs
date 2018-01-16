using System;

namespace Twity.DataModels.StreamMessages
{
    [Serializable]
    public class UserWithheld
    {
        public long id;
        public string[] withheld_in_countries;
    }
}
