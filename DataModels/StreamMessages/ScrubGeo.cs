using System;

namespace Twity.DataModels.StreamMessages
{
    [Serializable]
    public class ScrubGeo
    {
        public long user_id;
        public string user_id_str;
        public long up_to_status_id;
        public string up_to_status_id_str;
    }
}
