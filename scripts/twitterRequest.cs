using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Twitter
{
    public delegate void TwitterCallback(bool success, string response);

    public class Client
    {

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
        #endregion

        #region RequestHelperMethods

        private static IEnumerator SendRequest(UnityWebRequest request, SortedDictionary<string, string> parameters, string method, string requestURL, TwitterCallback callback)
        {
            request.SetRequestHeader("Authorization", Oauth.GenerateHeaderWithAccessToken(parameters, method, requestURL));
            yield return request.Send();

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
                    Debug.Log(request.responseCode);
                    callback(false, JsonHelper.ArrayToObject(request.downloadHandler.text));
                }
            }
        }

        #endregion

    }

}

