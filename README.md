
Twitter API Client for Unity C#. (beta)

Inspired by [Let's Tweet In Unity](https://www.assetstore.unity3d.com/jp/#!/content/536).

## Environment

- Unity 5.5.0f3

## Available API Methods

### REST API

- GET  : Available (Except Media Upload)
- POST : Available (Except Media Upload)

### Stream API

Not Available (now in progress)

## Usage

### Initialize

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
### GET search/tweets

```C#
void start() {
  Dictionary<string, string> parameters = new Dictionary<string, string>();
  parameters ["q"] = "search word";
  parameters ["count"] = 30.ToString();;
  StartCoroutine (twitter.Client.Get ("search/tweets", parameters, new twitter.TwitterCallback (this.Callback)));
}

void Callback(bool success, string response) {
  if (success) {
		Debug.Log (response);
	} else {
		Debug.Log (response);
	}
}
```
### GET statuses/home_timeline

```C#
void start() {
  Dictionary<string, string> parameters = new Dictionary<string, string>();
  parameters ["count"] = 30.ToString();;
  StartCoroutine (twitter.Client.Get ("statuses/home_timeline", parameters, new twitter.TwitterCallback (this.Callback)));
}

void Callback(bool success, string response) {
  if (success) {
		Debug.Log (response);
	} else {
		Debug.Log (response);
	}
}
```

### POST statuses/update

```C#
void start() {
  Dictionary<string, string> parameters = new Dictionary<string, string>();
  parameters ["status"] = "Tweet from Unity";
  StartCoroutine (twitter.Client.Get ("statuses/update", parameters, new twitter.TwitterCallback (this.Callback)));
}

void Callback(bool success, string response) {
  if (success) {
		Debug.Log (response);
	} else {
		Debug.Log (response);
	}
}
```

See https://dev.twitter.com/rest/reference for more Methods.
