using System;

namespace Twity.DataModels.Core
{
    [Serializable]
    public class TweetUser
    {
        public long id;
        public string id_str;
        public string name;
        public string screen_name;
        public string location;
        public string description;
        public string url;
        public bool verified;

        public string profile_text_color;
        public bool profile_user_background_image;
        public string profile_background_image_url;
        public string profile_background_image_url_https;
        public string profile_background_color;
        public string profile_banner_url;
        public bool profile_background_tile;
        public string profile_image_url;

        public int statuses_count;
        public int friends_count;
        public int followers_count;
        public int favourites_count;
        public bool following;
        public bool follow_request_sent;
        public int listed_count;

        public TweetObject status;

    }
}
