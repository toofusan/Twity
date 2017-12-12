
Twitter API Client for Unity C#. (beta)

Inspired by [Let's Tweet In Unity](https://www.assetstore.unity3d.com/jp/#!/content/536).

# Environment

- Unity 2017.2.0f3

# Available API Methods

## REST API

- GET  : Available
- POST : Available
-- Chunked POST "media/upload" (INIT/APPEND/FINALIZE) is not available yet (now in progress)

## Streaming API

- POST statuses/filter : Available(beta)
- GET  statuses/sample : Available(beta)
- UserStreams : Available(beta)

# Usage

## Initialize

```C#
public class EventHandler : MonoBehaviour {
  void Start () {
    Twitter.Oauth.consumerKey       = "...";
    Twitter.Oauth.consumerSecret    = "...";
    Twitter.Oauth.accessToken       = "...";
    Twitter.Oauth.accessTokenSecret = "...";
  }  
}
```
## REST API

### GET search/tweets

```C#
void Start() {
  Dictionary<string, string> parameters = new Dictionary<string, string>();
  parameters ["q"] = "search word";
  parameters ["count"] = 30.ToString();;
  StartCoroutine (Twitter.Client.Get ("search/tweets", parameters, Callback));
}

void Callback(bool success, string response) {
  if (success) {
    Twitter.SearchTweetsResponse Response = JsonUtility.FromJson<Twitter.SearchTweetsResponse> (response);
  } else {
    Debug.Log (response);
  }
}
```

### GET statuses/home_timeline

```C#
void Start() {
  Dictionary<string, string> parameters = new Dictionary<string, string>();
  parameters ["count"] = 30.ToString();;
  StartCoroutine (Twitter.Client.Get ("statuses/home_timeline", parameters, Callback));
}

void Callback(bool success, string response) {
  if (success) {
    Twitter.StatusesHomeTimelineResponse Response = JsonUtility.FromJson<Twitter.StatusesHomeTimelineResponse> (response);
  } else {
    Debug.Log (response);
  }
}
```

### POST statuses/update

```C#
void Start() {
  Dictionary<string, string> parameters = new Dictionary<string, string>();
  parameters ["status"] = "Tweet from Unity";
  StartCoroutine (Twitter.Client.Post ("statuses/update", parameters, Callback));
}

void Callback(bool success, string response) {
  if (success) {
    Twitter.Tweet tweet = JsonUtility.FromJson<Twitter.Tweet> (response);
  } else {
    Debug.Log (response);
  }
}
```

### POST statuses/retweet/:id
ex. search tweets with the word "Unity", and retweet 5 tweets.
```C#
void start() {
  Dictionary<string, string> parameters = new Dictionary<string, string>();
  parameters ["q"] = "Unity";       // Search keywords
  parameters ["count"] = 5.ToString();   // Number of Tweets
  StartCoroutine (Twitter.Client.Get ("search/tweets", parameters, Callback));
}

void Callback(bool success, string response) {
  if (success) {
    Twitter.SearchTweetsResponse Response = JsonUtility.FromJson<Twitter.SearchTweetsResponse> (response);
    foreach (Twitter.Tweet tweet in Response.statuses) { Retweet (tweet); }
  } else {
    Debug.Log (response);
  }
}

void Retweet(Twitter.Tweet tweet) {
  Dictionary<string, string> parameters = new Dictionary<string, string>();
  parameters ["id"] = tweet.id_str;
  StartCoroutine (Twitter.Client.Post ("statuses/retweet/" + tweet.id_str, parameters, RetweetCallback));
}

void RetweetCallback(bool success, string response) {
  if (success) {
    Debug.Log ("Retweet Done");
  } else {
    Debug.Log (response);
  }
}
```

### POST media/upload
```C#
void start() {
  byte[] imgBinary = File.ReadAllBytes(path/to/the/file);
  string imgbase64 = System.Convert.ToBase64String(imgBinary);

  Dictionary<string, string> parameters = new Dictionary<string, string>();
  parameters["media_data"] = imgbase64;
  parameters["additional_owners"] = "additional owner if you have";
  StartCoroutine (Twitter.Client.Post ("media/upload", parameters, MediaUploadCallback));
}

void MediaUploadCallback(bool success, string response) {
  if (success) {
    Twitter.UploadMedia media = JsonUtility.FromJson<Twitter.UploadMedia>(response);

    Dictionary<string, string> parameters = new Dictionary<string, string>();
    parameters["media_ids"] = media.media_id.ToString();
    parameters["status"] = "Tweet text with image";
    StartCoroutine (Twitter.Client.Post ("statuses/update", parameters, StatusesUpdateCallback));
  } else {
    Debug.Log (response);
  }
}

void StatusesUpdateCallback(bool success, string response) {
  if (success) {
    Twitter.Tweet tweet = JsonUtility.FromJson<Twitter.Tweet> (response);
  } else {
    Debug.Log (response);
  }
}
```

See https://dev.twitter.com/rest/reference for more Methods.


## Streaming API

### POST statuses/filter
```C#
Twitter.Stream stream;

void Start() {
  stream = new Stream(Twitter.StreamType.PublicFilter);
  Dictionary<string, string> streamParameters = new Dictionary<string, string>();

  List<string> tracks = new List<string>();
  tracks.Add("Unity");
  tracks.Add("Twitter");
  Twitter.FilterTrack filterTrack = new Twitter.FilterTrack(tracks);
  streamParameters.Add(filterTrack.GetKey(), filterTrack.GetValue());
  StartCoroutine(stream.On(streamParameters, OnStream));
}

void OnStream(string response, Twitter.StreamMessageType messageType) {
  try
  {
    if(messageType == Twitter.StreamMessageType.Tweet)
    {
      Twitter.Tweet tweet = JsonUtility.FromJson<Twitter.Tweet>(response);
    }
  }
  catch (System.Exception e)
  {
    Debug.Log(e);
  }
}
```

### User Stream
```C#
Twitter.Stream stream;

void Start() {
  stream = new Stream(Twitter.StreamType.User);
  Dictionary<string, string> streamParameters = new Dictionary<string, string>();

  StartCoroutine(stream.On(streamParameters, OnStream));
}

void OnStream(string response, Twitter.StreamMessageType messageType) {
  try
  {
    if(messageType == Twitter.StreamMessageType.Tweet)
    {
      Twitter.Tweet tweet = JsonUtility.FromJson<Twitter.Tweet>(response);
    }
    else if(messageType == Twitter.StreamMessageType.StreamEvent)
    {
      Twitter.StreamEvent streamEvent = JsonUtility.FromJson<Twitter.StreamEvent>(response);
      Debug.Log(streamEvent.event_name); // Response Key 'event' is replaced 'event_name' in this library.
    }
    else if(messageType == Twitter.StreamMessageType.FriendsList)
    {
      Twitter.FriendsList friendsList = JsonUtility.FromJson<Twitter.FriendsList>(response);
    }
  }
  catch (System.Exception e)
  {
    Debug.Log(e);
  }
}
```
See `StreamType` and `StreamMessageType` at `TwitterStreamType.cs`. and https://dev.twitter.com/streaming/overview/messages-types .

See https://dev.twitter.com/streaming/reference for more Methods.

## Response class
See `TwitterJson.cs`, and https://dev.twitter.com/overview/api/tweets , https://dev.twitter.com/overview/api/users , https://dev.twitter.com/overview/api/entities , https://dev.twitter.com/overview/api/entities-in-twitter-objects .

You can modify `TwitterJson.cs` to get a response item.


# License
- There are some modified code from other library. Check "TwitterOauth.cs" and "TwitterHelper.cs".
- For other parts, MIT.
