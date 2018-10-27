using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using Twity.Helpers;

namespace Twity
{
    public class Oauth
    {

        #region Tokens
        public static string consumerKey { get; set; }
        public static string consumerSecret { get; set; }
        public static string accessToken { get; set; }
        public static string accessTokenSecret { get; set; }

        public static string bearerToken {get; set;}

        public static string requestToken {get; set;}
        public static string requestTokenSecret {get; set;}
        public static string authorizeURL { get; set; }
        #endregion

        #region Public Method
        public static string GenerateHeaderWithAccessToken(SortedDictionary<string, string> parameters, string requestMethod, string requestURL)
        {
            string signature = GenerateSignature(parameters, requestMethod, requestURL);

            StringBuilder requestParamsString = new StringBuilder();
            foreach (KeyValuePair<string, string> param in parameters)
            {
                requestParamsString.Append(String.Format("{0}=\"{1}\",", Helper.UrlEncode(param.Key), Helper.UrlEncode(param.Value)));
            }

            string authHeader = "OAuth realm=\"Twitter API\",";
            string requestSignature = String.Format("oauth_signature=\"{0}\"", Helper.UrlEncode(signature));
            authHeader += requestParamsString.ToString() + requestSignature;
            return authHeader;
        }
        #endregion

        #region HelperMethods
        private static string GenerateSignature(SortedDictionary<string, string> parameters, string requestMethod, string requestURL)
        {
            string oauth_token = "";
            string oauth_token_secret = "";
            if (!string.IsNullOrEmpty(accessToken)) 
            {
                oauth_token = accessToken;
                oauth_token_secret = accessTokenSecret;
            }
            else if (!string.IsNullOrEmpty(requestToken))
            {
                oauth_token = requestToken;
                oauth_token_secret = requestTokenSecret;
            }
            
            AddDefaultOauthParams(parameters, consumerKey);
            parameters.Add("oauth_token", oauth_token);

            StringBuilder paramString = new StringBuilder();
            foreach (KeyValuePair<string, string> param in parameters)
            {
                paramString.Append(Helper.UrlEncode(param.Key) + "=" + Helper.UrlEncode(param.Value) + "&");
            }
            paramString.Length -= 1; // Remove "&" at the last of string


            string requestHeader = Helper.UrlEncode(requestMethod) + "&" + Helper.UrlEncode(requestURL);
            string signatureData = requestHeader + "&" + Helper.UrlEncode(paramString.ToString());

            string signatureKey = Helper.UrlEncode(consumerSecret) + "&" + Helper.UrlEncode(oauth_token_secret);
            HMACSHA1 hmacsha1 = new HMACSHA1(Encoding.ASCII.GetBytes(signatureKey));
            byte[] signatureBytes = hmacsha1.ComputeHash(Encoding.ASCII.GetBytes(signatureData));
            return Convert.ToBase64String(signatureBytes);
        }

        private static void AddDefaultOauthParams(SortedDictionary<string, string> parameters, string consumerKey)
        {
            parameters.Add("oauth_consumer_key", consumerKey);
            parameters.Add("oauth_signature_method", "HMAC-SHA1");
            parameters.Add("oauth_timestamp", GenerateTimeStamp());
            parameters.Add("oauth_nonce", GenerateNonce());
            parameters.Add("oauth_version", "1.0");
        }

        private static string GenerateTimeStamp()
        {
            DateTimeOffset baseDt = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);
            return ((DateTimeOffset.Now - baseDt).Ticks/10000000).ToString();
        }

        private static string GenerateNonce()
        {
            return Guid.NewGuid().ToString("N");
        }

        #endregion

    }



}

