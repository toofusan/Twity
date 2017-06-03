using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Twitter
{

    public delegate void TwitterStreamCallback(string response);

    public class Stream
    {

        private string REQUEST_URL;
        private UnityWebRequest request;

        public Stream(Type type)
        {
            string[] endpoints = { "statuses/filter", "statuses/sample", "user", "site" };

            if (type == Type.Filter || type == Type.Sample)
            {
                REQUEST_URL = "https://stream.twitter.com/1.1/" + endpoints[(int)type] + ".json";
            } else if (type == Type.User)
            {
                REQUEST_URL = "https://userstream.twitter.com/1.1/user.json";
            } else if (type == Type.Site)
            {
                REQUEST_URL = "https://sitestream.twitter.com/1.1/site.json";
            }

        }

        public IEnumerator On(Dictionary<string, string> APIParams, TwitterStreamCallback callback)
        {
            SortedDictionary<string, string> parameters = Helper.ConvertToSortedDictionary(APIParams);

            WWWForm form = new WWWForm();
            foreach (KeyValuePair<string, string> parameter in APIParams)
            {
                form.AddField(parameter.Key, parameter.Value);
            }

            request = UnityWebRequest.Post(REQUEST_URL, form);
            request.SetRequestHeader("ContentType", "application/x-www-form-urlencoded");
            request.SetRequestHeader("Authorization", Oauth.GenerateHeaderWithAccessToken(parameters, "POST", REQUEST_URL));
            request.downloadHandler = new StreamingDownloadHandler(callback);
            yield return request.Send();

        }

        public void Off()
        {
            Debug.Log("Connection Aborted");
            request.Abort();
        }


    }

    #region DownloadHandler

    public class StreamingDownloadHandler : DownloadHandlerScript
    {

        TwitterStreamCallback callback;

        public StreamingDownloadHandler(TwitterStreamCallback callback)
        {
            this.callback = callback;
        }

        protected override bool ReceiveData(byte[] data, int dataLength)
        {
            if (data == null || data.Length < 1)
            {
                Debug.Log("LoggingDownloadHandler :: ReceiveData - received a null/empty buffer");
                return false;
            }
            string response = Encoding.ASCII.GetString(data);
            try
            {
                callback(JsonHelper.ArrayToObject(response));
                return true;
            } catch (System.ArgumentException e)
            {
                //Debug.Log(e.ToString());
                return true;
            }
            
        }

    }
    #endregion

    public enum Type
    {
        Filter, // POST statuses/filter
        Sample, // GET statuses/sample
        User,   // GET user
        Site    // GET site
    }


    #region Parameters for statuses/filter
    public class FilterTrack
    {
        private List<string> tracks;

        public FilterTrack(string track)
        {
            tracks = new List<string>();
            tracks.Add(track);
        }
        public FilterTrack(List<string> tracks)
        {
            this.tracks = tracks;
        }
        public void AddTrack(string track)
        {
            tracks.Add(track);
        }
        public void AddTracks(List<string> tracks)
        {
            foreach (string track in tracks)
            {
                this.tracks.Add(track);
            }
        }
        public string GetKey()
        {
            return "track";
        }
        public string GetValue()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string track in tracks)
            {
                sb.Append(track + ",");
            }
            sb.Length -= 1;
            return sb.ToString();
        }
    }


    public class FilterLocations
    {
        private List<Coordinate> locations;

        public FilterLocations()
        {
            locations = new List<Coordinate>();
            locations.Add(new Coordinate(-180f, -90f));
            locations.Add(new Coordinate( 180f,  90f));
        }
        public FilterLocations(Coordinate southwest, Coordinate northeast)
        {
            locations = new List<Coordinate>();
            locations.Add(southwest);
            locations.Add(northeast);
        }
        public void AddLocation(Coordinate southwest, Coordinate northeast)
        {
            locations.Add(southwest);
            locations.Add(northeast);
        }
        public string GetKey()
        {
            return "locations";
        }
        public string GetValue()
        {
            StringBuilder sb = new StringBuilder();
            foreach(Coordinate location in locations)
            {
                sb.Append(location.lng.ToString("F1") + "," + location.lat.ToString("F1") + ",");
            }
            sb.Length -= 1;
            return sb.ToString();
        }
    }

    public class Coordinate
    {
        public float lng { get; set; }
        public float lat { get; set; }

        public Coordinate(float lng, float lat)
        {
            this.lng = lng;
            this.lat = lat;
        }
    }

    public class FilterFollow
    {
        private List<string> screen_names;
        private List<long> ids;

        public FilterFollow(List<string> screen_names)
        {
            this.screen_names = screen_names;
        }
        public FilterFollow(List<long> ids)
        {
            this.ids = ids;
        }
        public FilterFollow(long id)
        {
            ids = new List<long>();
            ids.Add(id);
        }
        public void AddId(long id)
        {
            ids.Add(id);
        }
        public void AddIds(List<long> ids)
        {
            foreach(long id in ids)
            {
                this.ids.Add(id);
            }
        }
        public string GetKey()
        {
            return "follow";
        }
        public string GetValue()
        {
            StringBuilder sb = new StringBuilder();
            if (ids.Count > 0)
            {
                foreach (long id in ids)
                {
                    sb.Append(id.ToString() + ",");
                }
            } else
            {
                foreach (string screen_name in screen_names)
                {
                    sb.Append(screen_name + ",");
                }
            }
            sb.Length -= 1;
            return sb.ToString();
        }
    }
    #endregion
}

