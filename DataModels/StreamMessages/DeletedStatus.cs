using System;

namespace Twity.DataModels.StreamMessages
{
    [Serializable]
    public class DeletedStatus
    {
        public long id;
        public string id_str;
        public long user_id;
        public string user_id_str;
    }
}
