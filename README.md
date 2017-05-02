
Twitter API Client for Unity C#. (beta)

Inspired by [Let's Tweet In Unity](https://www.assetstore.unity3d.com/jp/#!/content/536).

# Environment

- Unity 5.6.0f3

# Available API Methods

## REST API

- GET  : Available (Except Media Upload)
- POST : Available (Except Media Upload)

## Stream API

- POST statuses/filter : Available
- GET  statuses/sample : Available
- UserStreams : Not Available (now in progress)

# Usage

## Initialize

```C#
using twitter;

public class EventHandler : MonoBehaviour {
  void Start () {
    twitter.Client.consumerKey       = "...";
    twitter.Client.consumerSecret    = "...";
    twitter.Client.accessToken       = "...";
    twitter.Client.accessTokenSecret = "...";
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
  StartCoroutine (twitter.Client.Get ("search/tweets", parameters, new twitter.TwitterCallback (this.Callback)));
}

void Callback(bool success, string response) {
  if (success) {
    SearchTweetsResponse Response = JsonUtility.FromJson<SearchTweetsResponse> (response);
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
  StartCoroutine (twitter.Client.Get ("statuses/home_timeline", parameters, new twitter.TwitterCallback (this.Callback)));
}

void Callback(bool success, string response) {
  if (success) {
    StatusesHomeTimelineResponse Response = JsonUtility.FromJson<StatusesHomeTimelineResponse> (response);
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
  StartCoroutine (twitter.Client.Post ("statuses/update", parameters, new twitter.TwitterCallback (this.Callback)));
}

void Callback(bool success, string response) {
  if (success) {
    Tweet tweet = JsonUtility.FromJson<Tweet> (response);
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
  StartCoroutine (twitter.Client.Get ("search/tweets", parameters, new twitter.TwitterCallback (this.Callback)));
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
  StartCoroutine (twitter.Client.Post ("statuses/retweet/" + tweet.id_str, parameters, new twitter.TwitterCallback (this.RetweetCallback)));
}

void RetweetCallback(bool success, string response) {
  if (success) {
    Debug.Log ("Retweet Done");
  } else {
    Debug.Log (response);
  }
}
```
See https://dev.twitter.com/rest/reference for more Methods.


## Streaming API

### POST statuses/filter
```C#
using Twitter;

Stream stream;

void Start() {
  stream = new Stream(Type.Filter);
  Dictionary<string, string> streamParameters = new Dictionary<string, string>();
  streamParameters.Add("track", "iPhone");
  StartCoroutine(stream.On(streamParameters, this.OnStream));
}

void Callback(string response) {
  try
    {
      GenerateTweetCard(JsonUtility.FromJson<Tweet>(response));
  } catch (System.ArgumentException e)
  {
    Debug.Log("Invalid Response");
  }
}
```
See https://dev.twitter.com/streaming/reference for more Methods.

## Response class
See `TwitterJson.cs`, and https://dev.twitter.com/overview/api/tweets , https://dev.twitter.com/overview/api/users , https://dev.twitter.com/overview/api/entities , https://dev.twitter.com/overview/api/entities-in-twitter-objects .
You can modify `TwitterJson.cs` to get a response item.
