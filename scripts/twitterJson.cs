using System;
using System.Collections.Generic;
using UnityEngine;

namespace twitter {

	// ==============================
	// Basic Class
	// ==============================
	[Serializable]
	public class Tweet {
		public int id;
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
//		public string retweeted_status = null;
		public bool favorited;
		public int favorite_count;
		public int quoted_status_id;
		public string quoted_status_id_str;
//		public string quoted_status;


		public TweetUser user;
		public Entities entities;

		public String lang;
	}

	[Serializable]
	public class TweetUser {
		public int id;
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

//		public Tweet status;

	}

	[Serializable]
	public class Retweet {
		public int id;
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
		public string retweeted_status = "";
		public bool favorited;
		public int favorite_count;
		public int quoted_status_id;
		public string quoted_status_id_str;

		public TweetUser user;
	}

	// ==============================
	// Entities Class
	// ==============================
	[Serializable]
    [Serializable]
    public class Entities
    {
        public Media[] media;
        public UserMention[] user_mentions;
        public HashTag[] hashtags;
    }
    [Serializable]
    public class Extended_Entities
    {
        public Media[] media;
    }
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
    [Serializable]
    public class UserMention
    {
        public int id;
        public string id_str;
        public string screen_name;
        public string name;
    }
    [Serializable]
    public class HashTag
    {
        public string text;
    }
    [Serializable]
    public class Symbol
    {
        public string text;
    }
    [Serializable]
    public class Video_Info
    {
        public long id;
        public string id_str;
        public string media_url;
        public string type;
        public Variant[] variants;
    }
    [Serializable]
    public class Variant
    {
        public int bitrate;
        public string content_type;
        public string url;
    }




    // ==============================
    // Response Class
    // ==============================
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
	public class Tweets {
		public Tweet[] items;
	}
		
}


