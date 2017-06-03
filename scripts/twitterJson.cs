using System;

namespace Twitter {

    #region Basic Class
    [Serializable]
    public class TweetObject
    {
        public long id;
        public string id_str;
        public string created_at;
        public string text;

        public string in_reply_to_screen_name;
        public int in_reply_to_status_id;
        public string in_reply_to_status_id_str;
        public int in_reply_to_user_id;
        public string in_reply_to_user_id_str;
        public bool retweeted;
        public int retweet_count;
        public bool favorited;
        public int favorite_count;
        public bool is_quote_status;
        public int quoted_status_id;
        public string quoted_status_id_str;

        public string lang;

        public Entities entities;
        public Extended_Entities extended_entities;
    }
    [Serializable]
    public class TweetObjectWithUser: TweetObject
    {
        public TweetUser user;
    }

    [Serializable]
    public class Tweet: TweetObjectWithUser
    {
        public TweetObjectWithUser retweeted_status;
    }

	[Serializable]
	public class TweetUser {
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

    #endregion


    # region Entities Class

    [Serializable]
	public class Entities {
		public Media[] media;
		public UserMention[] user_mentions;
		public HashTag[] hashtags;
	}
	[Serializable]
	public class Extended_Entities {
		public Media[] media;
	}
	[Serializable]
	public class Media {
		public int id;
		public int id_str;
		public string media_url;
		public string media_url_https;
		public string type;
		public Video_Info video_info;
	}
	[Serializable]
	public class UserMention {
		public long id;
		public string id_str;
		public string screen_name;
		public string name;
	}
	[Serializable]
	public class HashTag {
		public string text;
	}
	[Serializable]
	public class Symbol {
		public string text;
	}
	[Serializable]
	public class Video_Info {
		public long id;
		public string id_str;
		public string media_url;
		public string type;
		public Variant[] variants;
	}
	[Serializable]
	public class Variant {
		public int bitrate;
		public string content_type;
		public string url;
	}

    #endregion



    #region Response Class

    [Serializable]
	public class SearchTweetsResponse {
		public Tweet[] statuses; 
	}
	[Serializable]
	public class StatusesUserTimelineResponse {
		public Tweet[] items;
	}
	[Serializable]
	public class StatusesHomeTimelineResponse {
		public Tweet[] items;
	}

	[Serializable]
	public class FollowersListResponse {
		public TweetUser[] users;
	}

	[Serializable]
	public class FriendsListResponse {
		public TweetUser[] users;
	}


    [Serializable]
    public class FriendsidsResponse
    {
        public long[] ids;
    }

	[Serializable]
	public class Tweets {
		public Tweet[] items;
	}

    #endregion
}


