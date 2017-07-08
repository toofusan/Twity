namespace Twitter
{

    public enum StreamMessageType
    {
        // See detail at https://dev.twitter.com/streaming/overview/messages-types

        Tweet,                  // A Tweet has been posted.
        StatusDeletionNotice,   // A given Tweet has been deleted.
        LocationDeletionNotice, // Geolocated data must be stripped from a range of Tweets.
        LimitNotice,            // A filtered stream has matched more Tweets than its current rate limit allows to be delivered.
        WithheldContentNotice,  // Either the indicated Tweet or indicated user has had their content withheld.
        DisconnectMessage,      // Streams may be shut down for a variety of reasons.
        StallWarning,           // current health of the connection (required "stall_warnings" parameter)
        FriendsList,            // Upon establishing a User Stream connection
        FriendsListStr,
        DirectMessage,          // send or receive Direct Messages
        StreamEvent,            // Notifications about non-Tweet events. Check "event_name".
        None                    // Error or no response
    }

    public enum StreamType
    {
        PublicFilter, // POST statuses/filter
        PublicSample, // GET statuses/sample
        User,   // GET user
        Site    // GET site
    }
}