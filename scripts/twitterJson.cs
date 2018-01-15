using System;
using Twity.DataModels.Core;

namespace Twity {

    # region Entities Class

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
		public long id;
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

    #endregion

    #region Rest API Response Class

    [Serializable]
	public class SearchTweetsResponse
    {
		public Tweet[] statuses; 
	}
	[Serializable]
	public class StatusesUserTimelineResponse
    {
		public Tweet[] items;
	}
	[Serializable]
	public class StatusesHomeTimelineResponse
    {
		public Tweet[] items;
	}

	[Serializable]
	public class FollowersListResponse
    {
		public TweetUser[] users;
	}

	[Serializable]
	public class FriendsListResponse
    {
		public TweetUser[] users;
	}


    [Serializable]
    public class FriendsidsResponse
    {
        public long[] ids;
    }

	[Serializable]
	public class Tweets
    {
        public Tweet[] items;
	}

    #endregion

    #region Direct Messages
    [Serializable]
    public class DirectMessages
    {
        DirectMessage[] items;
    }

    [Serializable]
    public class DirectMessage
    {
        public string created_at;
        public Entities entities;
        public long id;
        public string id_str;
        public TweetUser recipient;
        public long recipient_id;
        public string recipient_screen_name;
        public TweetUser sender;
        public long sender_id;
        public string sender_screen_name;
        public string text;
    }

    #endregion

    #region Stream Messages

    [Serializable]
    public class StatusDeletionNotice
    {
        public StatusDelete delete;
    }

    [Serializable]
    public class StatusDelete
    {
        public DeletedStatus status;
    }

    [Serializable]
    public class DeletedStatus
    {
        public long id;
        public string id_str;
        public long user_id;
        public string user_id_str;
    }

    [Serializable]
    public class LocationDeletionNotice
    {
        public ScrubGeo scrub_geo;
    }

    [Serializable]
    public class ScrubGeo
    {
        public long user_id;
        public string user_id_str;
        public long up_to_status_id;
        public string up_to_status_id_str;
    }

    [Serializable]
    public class LimitNotice
    {
        public TrackLimit limit;
    }

    [Serializable]
    public class TrackLimit
    {
        public int track;
    }

    [Serializable]
    public class WithheldContentNotice
    {
        public StatusWithheld status_withheld;
        public UserWithheld user_withheld;
    }

    [Serializable]
    public class StatusWithheld
    {
        public long id;
        public long user_id;
        public string[] withheld_in_countries;
    }

    [Serializable]
    public class UserWithheld
    {
        public long id;
        public string[] withheld_in_countries;
    }

    [Serializable]
    public class DisconnectMessage
    {
        public Disconnect disconnect;
    }

    [Serializable]
    public class Disconnect
    {
        public int code;
        public string stream_name;
        public string reason;
    }

    [Serializable]
    public class StallWarning
    {
        public Warning warning;
    }

    [Serializable]
    public class Warning
    {
        public string code;
        public string message;
        public int percent_full;
        public int user_id;
    }

    [Serializable]
    public class FriendsList
    {
        public long[] friends;
    }

    [Serializable]
    public class StringifyFriendsList
    {
        public string[] friends_str;
    }

    [Serializable]
    public class StreamEvent
    {
        public string event_name;
        public string created_at;
        public TweetUser target;
        public TweetUser source;
        public string target_object;
    }


    #endregion
}


