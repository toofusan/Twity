using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Twity.Helpers;
using UnityEngine;
using UnityEngine.Networking;

namespace Twity
{
    public delegate void TwitterCallback(bool success, string response);
    public delegate void TwitterAuthenticationCallback(bool success);

    public class Client
    {

        public static string screenName;

        #region API Methods

        public static IEnumerator Get(string APIPath, Dictionary<string, string> APIParams, TwitterCallback callback)
        {
            string REQUEST_URL = "https://api.twitter.com/1.1/" + APIPath + ".json";
            SortedDictionary<string, string> parameters = Helper.ConvertToSortedDictionary(APIParams);

            string requestURL = REQUEST_URL + "?" + Helper.GenerateRequestparams(parameters);
            UnityWebRequest request = UnityWebRequest.Get(requestURL);
            request.SetRequestHeader("ContentType", "application/x-www-form-urlencoded");

            yield return SendRequest(request, parameters, "GET", REQUEST_URL, callback);
        }

        public static IEnumerator Post(string APIPath, Dictionary<string, string> APIParams, TwitterCallback callback)
        {
            List<string> endpointForFormdata = new List<string>
            {
                "media/upload",
                "account/update_profile_image",
                "account/update_profile_banner",
                "account/update_profile_background_image"
            };

            string REQUEST_URL = "";
            if (APIPath.Contains("media/"))
            {
                REQUEST_URL = "https://upload.twitter.com/1.1/" + APIPath + ".json";
            } else
            {
                REQUEST_URL = "https://api.twitter.com/1.1/" + APIPath + ".json";
            }
            Debug.Log(REQUEST_URL);

            WWWForm form = new WWWForm();
            SortedDictionary<string, string> parameters = new SortedDictionary<string, string>();

            if (endpointForFormdata.IndexOf(APIPath) != -1)
            {
                // multipart/form-data

                foreach (KeyValuePair<string, string> parameter in APIParams)
                {
                    if (parameter.Key.Contains("media"))
                    {
                        form.AddBinaryData("media", Convert.FromBase64String(parameter.Value), "", "");
                    }
                    else if (parameter.Key == "image")
                    {
                        form.AddBinaryData("image", Convert.FromBase64String(parameter.Value), "", "");
                    }
                    else if (parameter.Key == "banner")
                    {
                        form.AddBinaryData("banner", Convert.FromBase64String(parameter.Value), "", "");
                    }
                    else
                    {
                        form.AddField(parameter.Key, parameter.Value);
                    }
                }

                
                UnityWebRequest request = UnityWebRequest.Post(REQUEST_URL, form);
                yield return SendRequest(request, parameters, "POST", REQUEST_URL, callback);
            }
            else if (APIPath == "media/metadata/createa")
            {
                parameters = Helper.ConvertToSortedDictionary(APIParams);
                foreach (KeyValuePair<string, string> parameter in APIParams)
                {
                    form.AddField(parameter.Key, parameter.Value);
                }

                UnityWebRequest request = UnityWebRequest.Post(REQUEST_URL, form);
                request.SetRequestHeader("ContentType", "text/plain; charset=UTF-8");
                yield return SendRequest(request, parameters, "POST", REQUEST_URL, callback);
            }
            else
            {
                // application/x-www-form-urlencoded

                parameters = Helper.ConvertToSortedDictionary(APIParams);
                foreach (KeyValuePair<string, string> parameter in APIParams)
                {
                    form.AddField(parameter.Key, parameter.Value);
                }

                UnityWebRequest request = UnityWebRequest.Post(REQUEST_URL, form);
                request.SetRequestHeader("ContentType", "application/x-www-form-urlencoded");
                yield return SendRequest(request, parameters, "POST", REQUEST_URL, callback);

            }
        }

        public static IEnumerator GetOauth2BearerToken(TwitterAuthenticationCallback callback)
        {
            string url = "https://api.twitter.com/oauth2/token";

            string credential = Helper.UrlEncode(Oauth.consumerKey) + ":" + Helper.UrlEncode(Oauth.consumerSecret);
            credential = Convert.ToBase64String(Encoding.UTF8.GetBytes(credential));

            WWWForm form = new WWWForm();
            form.AddField("grant_type", "client_credentials");

            UnityWebRequest request = UnityWebRequest.Post(url, form);
            request.SetRequestHeader("ContentType", "application/x-www-form-urlencoded;charset=UTF-8");
            request.SetRequestHeader("Authorization", "Basic " + credential);

            #if UNITY_2017_1
                    yield return request.Send();
            #endif
            #if UNITY_2017_2_OR_NEWER
                    yield return request.SendWebRequest();
            #endif

            if (request.isNetworkError) callback(false);

            if (request.responseCode == 200 || request.responseCode == 201)
            {
                Twity.Oauth.bearerToken = JsonUtility.FromJson<Twity.DataModels.Oauth.BearerToken>(request.downloadHandler.text).access_token;
                callback(true);
            }
            else
            {
                callback(false);
            }
        }

        public static IEnumerator GenerateRequestToken(TwitterAuthenticationCallback callback)
        {
            yield return GenerateRequestToken(callback, "oob");
        }

        public static IEnumerator GenerateRequestToken(TwitterAuthenticationCallback callback, string callbackURL)
        {
            string url = "https://api.twitter.com/oauth/request_token";
            ClearTokens();

            SortedDictionary<string, string> p = new SortedDictionary<string, string>();
            p.Add("oauth_callback", callbackURL);

            WWWForm form = new WWWForm();

            UnityWebRequest request = UnityWebRequest.Post(url, form);
            request.SetRequestHeader("Authorization", Oauth.GenerateHeaderWithAccessToken(p, "POST", url));
            
            #if UNITY_2017_1
                    yield return request.Send();
            #endif
            #if UNITY_2017_2_OR_NEWER
                    yield return request.SendWebRequest();
            #endif

            if (request.isNetworkError)
            {
                callback(false);
            }
            else
            {
                if (request.responseCode == 200 || request.responseCode == 201)
                {
                    string[] arr = request.downloadHandler.text.Split("&"[0]);
                    Dictionary<string, string> d = new Dictionary<string, string>();
                    foreach(string s in arr)
                    {
                        string k = s.Split("="[0])[0];
                        string v = s.Split("="[0])[1];
                        d[k] = v;
                    }
                    Oauth.requestToken = d["oauth_token"];
                    Oauth.requestTokenSecret = d["oauth_token_secret"];
                    Oauth.authorizeURL = "https://api.twitter.com/oauth/authorize?oauth_token=" + Oauth.requestToken;
                    callback(true);
                }
                else
                {
                    callback(false);
                }
            }
        }

        public static IEnumerator GenerateAccessToken(string pin, TwitterAuthenticationCallback callback)
        {
            string url = "https://api.twitter.com/oauth/access_token";

            SortedDictionary<string, string> p = new SortedDictionary<string, string>();
            p.Add("oauth_verifier", pin);

            WWWForm form = new WWWForm();
            form.AddField("oauth_verifier", pin);

            UnityWebRequest request = UnityWebRequest.Post(url, form);
            request.SetRequestHeader("Authorization", Oauth.GenerateHeaderWithAccessToken(p, "POST", url));
            
            #if UNITY_2017_1
                    yield return request.Send();
            #endif
            #if UNITY_2017_2_OR_NEWER
                    yield return request.SendWebRequest();
            #endif

            if (request.isNetworkError)
            {
                callback(false);
            }
            else
            {
                if (request.responseCode == 200 || request.responseCode == 201)
                {
                    string[] arr = request.downloadHandler.text.Split("&"[0]);
                    Dictionary<string, string> d = new Dictionary<string, string>();
                    foreach(string s in arr)
                    {
                        string k = s.Split("="[0])[0];
                        string v = s.Split("="[0])[1];
                        d[k] = v;
                    }
                    Oauth.accessToken = d["oauth_token"];
                    Oauth.accessTokenSecret = d["oauth_token_secret"];
                    screenName = d["screen_name"];
                    callback(true);
                }
                else
                {
                    callback(false);
                }
            }
        }

        #endregion

        #region RequestHelperMethods

        private static IEnumerator SendRequest(UnityWebRequest request, SortedDictionary<string, string> parameters, string method, string requestURL, TwitterCallback callback)
        {
            if (!string.IsNullOrEmpty(Oauth.accessToken))
            {
                request.SetRequestHeader("Authorization", Oauth.GenerateHeaderWithAccessToken(parameters, method, requestURL));
            }
            else if (!string.IsNullOrEmpty(Oauth.bearerToken))
            {
                request.SetRequestHeader("Authorization", "Bearer " + Oauth.bearerToken);
            } 
            
            #if UNITY_2017_1
                    yield return request.Send();
            #endif
            #if UNITY_2017_2_OR_NEWER
                    yield return request.SendWebRequest();
            #endif

            if (request.isNetworkError)
            {
                callback(false, JsonHelper.ArrayToObject(request.error));
            }
            else
            {
                if (request.responseCode == 200 || request.responseCode == 201)
                {
                    callback(true, JsonHelper.ArrayToObject(request.downloadHandler.text));
                }
                else
                {
                    callback(false, JsonHelper.ArrayToObject(request.downloadHandler.text));
                }
            }
        }

        private static void ClearTokens() {
            Oauth.requestToken = String.Empty;
            Oauth.requestTokenSecret = String.Empty;
            Oauth.accessToken = String.Empty;
            Oauth.accessTokenSecret = String.Empty;
        }

        #endregion

    }

}

