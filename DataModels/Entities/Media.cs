using System;

namespace Twity.DataModels.Entities
{
    [Serializable]
    public class Media
    {
        public int id;
        public int id_str;
        public string media_url;
        public string media_url_https;
        public string type;
        public Video_Info video_info;
    }
}
