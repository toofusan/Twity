
# Twity
Twitter API Client for Unity C#. (ex-name. twitter-for-unity)

Inspired by [Let's Tweet In Unity](https://www.assetstore.unity3d.com/jp/#!/content/536).

## Environment
- Unity 2017.1.0f3
- Unity 2017.2.0f3
- Unity 2017.4.2f2
- Unity 2018.1.0f2

(On Unity 2017.3, Streaming API doesn't work)

## Available API Methods

### REST API

- GET  : Available
- POST : Available
-- Chunked POST "media/upload" (INIT/APPEND/FINALIZE) is not available yet (now in progress)

### Streaming API

- POST statuses/filter : Available(beta)
- GET  statuses/sample : Available(beta)
- UserStreams : Available(beta)

## Usage

### Authentication
If you have access_token and access_token_secret,
```C#
public class EventHandler : MonoBehaviour {
  void Start () {
    Twity.Oauth.consumerKey       = "...";
    Twity.Oauth.consumerSecret    = "...";
    Twity.Oauth.accessToken       = "...";
    Twity.Oauth.accessTokenSecret = "...";
  }  
}
```


You can use application-only authentication.
https://developer.twitter.com/en/docs/basics/authentication/overview/application-only
```C#
public class EventHandler : MonoBehaviour {
  void Start () {
    Twity.Oauth.consumerKey       = "...";
    Twity.Oauth.consumerSecret    = "...";
    
    StartCoroutine(Twity.Client.GetOauth2BearerToken(Callback));
  }

  void Callback(bool success) {
    if (!success) return;
    // you write some request with application-only authentication
  }
}
```


You can use PIN-Based Oauth.
https://developer.twitter.com/en/docs/basics/authentication/overview/pin-based-oauth
```C#
public class EventHandler : MonoBehaviour {
  void Start () {
    Twity.Oauth.consumerKey       = "...";
    Twity.Oauth.consumerSecret    = "...";
    
    StartCoroutine(Twity.Client.GenerateRequestToken(RequestTokenCallback));
  }

  void RequestCallback(bool success) {
    if (!success) return;
    // When request successes, you can display `Twity.Oauth.authorizeURL` to user so that they may use a web browser to access Twitter.
  }
  
  void GenerateAccessToken(string pin) {
    // pin is numbers displayed on web browser when user complete authorization.
    
    StartCoroutine(Twity.Client.GenerateAccessToken(pin, AccessTokenCallback));
  }
  
  void AccessTokenCallback(bool success) {
    if (!success) return;
    // When success, authorization is completed. You can make request to other endpoint.
    // User's screen_name is in '`Twity.Client.screenName`.
  }
}
```

### REST API

#### GET search/tweets

```C#
using Twity.DataModels.Responses;

void Start() {
  Dictionary<string, string> parameters = new Dictionary<string, string>();
  parameters ["q"] = "search word";
  parameters ["count"] = 30.ToString();;
  StartCoroutine (Twity.Client.Get ("search/tweets", parameters, Callback));
}

void Callback(bool success, string response) {
  if (success) {
    SearchTweetsResponse Response = JsonUtility.FromJson<SearchTweetsResponse> (response);
  } else {
    Debug.Log (response);
  }
}
```

#### GET statuses/home_timeline

```C#
using Twity.DataModels.Responses;

void Start() {
  Dictionary<string, string> parameters = new Dictionary<string, string>();
  parameters ["count"] = 30.ToString();;
  StartCoroutine (Twity.Client.Get ("statuses/home_timeline", parameters, Callback));
}

void Callback(bool success, string response) {
  if (success) {
    StatusesHomeTimelineResponse Response = JsonUtility.FromJson<StatusesHomeTimelineResponse> (response);
  } else {
    Debug.Log (response);
  }
}
```

#### POST statuses/update

```C#
using Twity.DataModels.Core;

void Start() {
  Dictionary<string, string> parameters = new Dictionary<string, string>();
  parameters ["status"] = "Tweet from Unity";
  StartCoroutine (Twity.Client.Post ("statuses/update", parameters, Callback));
}

void Callback(bool success, string response) {
  if (success) {
    Tweet tweet = JsonUtility.FromJson<Tweet> (response);
  } else {
    Debug.Log (response);
  }
}
```

#### POST statuses/retweet/:id
ex. search tweets with the word "Unity", and retweet 5 tweets.
```C#
using Twity.DataModels.Core;
using Twity.DataModels.Responses;

void start() {
  Dictionary<string, string> parameters = new Dictionary<string, string>();
  parameters ["q"] = "Unity";       // Search keywords
  parameters ["count"] = 5.ToString();   // Number of Tweets
  StartCoroutine (Twity.Client.Get ("search/tweets", parameters, Callback));
}

void Callback(bool success, string response) {
  if (success) {
    SearchTweetsResponse Response = JsonUtility.FromJson<SearchTweetsResponse> (response);
    foreach (Tweet tweet in Response.statuses) { Retweet (tweet); }
  } else {
    Debug.Log (response);
  }
}

void Retweet(Tweet tweet) {
  Dictionary<string, string> parameters = new Dictionary<string, string>();
  parameters ["id"] = tweet.id_str;
  StartCoroutine (Twity.Client.Post ("statuses/retweet/" + tweet.id_str, parameters, RetweetCallback));
}

void RetweetCallback(bool success, string response) {
  if (success) {
    Debug.Log ("Retweet Done");
  } else {
    Debug.Log (response);
  }
}
```

#### POST media/upload
```C#
using Twity.DataModels.Core;

void start() {
  byte[] imgBinary = File.ReadAllBytes(path/to/the/file);
  string imgbase64 = System.Convert.ToBase64String(imgBinary);

  Dictionary<string, string> parameters = new Dictionary<string, string>();
  parameters["media_data"] = imgbase64;
  parameters["additional_owners"] = "additional owner if you have";
  StartCoroutine (Twity.Client.Post ("media/upload", parameters, MediaUploadCallback));
}

void MediaUploadCallback(bool success, string response) {
  if (success) {
    UploadMedia media = JsonUtility.FromJson<UploadMedia>(response);

    Dictionary<string, string> parameters = new Dictionary<string, string>();
    parameters["media_ids"] = media.media_id.ToString();
    parameters["status"] = "Tweet text with image";
    StartCoroutine (Twity.Client.Post ("statuses/update", parameters, StatusesUpdateCallback));
  } else {
    Debug.Log (response);
  }
}

void StatusesUpdateCallback(bool success, string response) {
  if (success) {
    Tweet tweet = JsonUtility.FromJson<Tweet> (response);
  } else {
    Debug.Log (response);
  }
}
```

See https://dev.twitter.com/rest/reference for more Methods.


### Streaming API

#### POST statuses/filter
```C#
using Twity;
using Twity.DataModels.Core;

Stream stream;

void Start() {
  stream = new Stream(StreamType.PublicFilter);
  Dictionary<string, string> streamParameters = new Dictionary<string, string>();

  List<string> tracks = new List<string>();
  tracks.Add("Unity");
  tracks.Add("Twitter");
  Twity.FilterTrack filterTrack = new Twity.FilterTrack(tracks);
  streamParameters.Add(filterTrack.GetKey(), filterTrack.GetValue());
  StartCoroutine(stream.On(streamParameters, OnStream));
}

void OnStream(string response, StreamMessageType messageType) {
  try
  {
    if(messageType == StreamMessageType.Tweet)
    {
      Tweet tweet = JsonUtility.FromJson<Tweet>(response);
    }
  }
  catch (System.Exception e)
  {
    Debug.Log(e);
  }
}
```

#### ~~User Stream~~
User Stream is retired from August 23rd, 2018
https://developer.twitter.com/en/docs/accounts-and-users/subscribe-account-activity/api-reference/user-stream.html
```C#
using Twity;
using Twity.DataModels.Core;
using Twity.DataModels.StreamMessages;

Stream stream;

void Start() {
  stream = new Stream(StreamType.User);
  Dictionary<string, string> streamParameters = new Dictionary<string, string>();

  StartCoroutine(stream.On(streamParameters, OnStream));
}

void OnStream(string response, StreamMessageType messageType) {
  try
  {
    if(messageType == StreamMessageType.Tweet)
    {
      Twity.Tweet tweet = JsonUtility.FromJson<Tweet>(response);
    }
    else if(messageType == StreamMessageType.StreamEvent)
    {
      StreamEvent streamEvent = JsonUtility.FromJson<StreamEvent>(response);
      Debug.Log(streamEvent.event_name); // Response Key 'event' is replaced 'event_name' in this library.
    }
    else if(messageType == StreamMessageType.FriendsList)
    {
      FriendsList friendsList = JsonUtility.FromJson<FriendsList>(response);
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

### Response class
See `DataModels/Responses/`, and https://dev.twitter.com/overview/api/tweets , https://dev.twitter.com/overview/api/users , https://dev.twitter.com/overview/api/entities , https://dev.twitter.com/overview/api/entities-in-twitter-objects .

You can modify `DataModels/Responses/` to get a response item.


## License
MIT
