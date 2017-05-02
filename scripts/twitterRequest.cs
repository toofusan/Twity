using System.Collections;
using System.Collections.Generic;
using System.Text;
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

            string requestURL = REQUEST_URL + "?" + GenerateRequestparams(parameters);
            UnityWebRequest request = UnityWebRequest.Get(requestURL);

            yield return SendRequest(request, parameters, "GET", REQUEST_URL, callback);
        }

        public static IEnumerator Post(string APIPath, Dictionary<string, string> APIParams, TwitterCallback callback)
        {
            string REQUEST_URL = "https://api.twitter.com/1.1/" + APIPath + ".json";
            SortedDictionary<string, string> parameters = Helper.ConvertToSortedDictionary(APIParams);

            WWWForm form = new WWWForm();
            foreach (KeyValuePair<string, string> parameter in APIParams)
            {
                form.AddField(parameter.Key, parameter.Value);
            }

            UnityWebRequest request = UnityWebRequest.Post(REQUEST_URL, form);
            yield return SendRequest(request, parameters, "POST", REQUEST_URL, callback);

        }

        #endregion

        #region RequestHelperMethods

        private static void SetAPIParams(SortedDictionary<string, string> parameters, Dictionary<string, string> APIParams)
        {
            foreach (KeyValuePair<string, string> APIParam in APIParams)
            {
                parameters.Add(APIParam.Key, APIParam.Value);
            }
        }

        private static string GenerateRequestparams(SortedDictionary<string, string> parameters)
        {
            StringBuilder requestParams = new StringBuilder();
            foreach (KeyValuePair<string, string> param in parameters)
            {
                requestParams.Append(Helper.UrlEncode(param.Key) + "=" + Helper.UrlEncode(param.Value) + "&");
            }
            requestParams.Length -= 1; // Remove "&" at the last of string
            return requestParams.ToString();
        }

        private static IEnumerator SendRequest(UnityWebRequest request, SortedDictionary<string, string> parameters, string method, string requestURL, TwitterCallback callback)
        {
            request.SetRequestHeader("ContentType", "application/x-www-form-urlencoded");
            request.SetRequestHeader("Authorization", Oauth.GenerateHeaderWithAccessToken(parameters, method, requestURL));
            yield return request.Send();

            if (request.isError)
            {
                callback(false, JsonHelper.ArrayToObject(request.error));
            }
            else
            {
                if (request.responseCode == 200)
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

