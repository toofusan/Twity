using System;

namespace Twity.DataModels.Entities
{
    [Serializable]
    public class Video_Info
    {
        public long id;
        public string id_str;
        public string media_url;
        public string type;
        public Variant[] variants;
    }
}
