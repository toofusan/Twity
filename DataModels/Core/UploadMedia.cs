using System;

namespace Twity.DataModels.Core
{
    [Serializable]
    public class UploadMedia
    {
        public long media_id;
        public string media_id_string;
        public int size;
        public int expires_after_secs;
        public UploadMediaImage image;
    }
}
