using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Twitter
{
    public class Oauth
    {

        #region Tokens
        public static string consumerKey { get; set; }
        public static string consumerSecret { get; set; }
        public static string accessToken { get; set; }
        public static string accessTokenSecret { get; set; }
        #endregion

        #region Public Method
        public static string GenerateHeaderWithAccessToken(SortedDictionary<string, string> parameters, string requestMethod, string requestURL)
        {
            string signature = GenerateSignatureWithAccessToken(parameters, requestMethod, requestURL);

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

        private static string GenerateSignatureWithAccessToken(SortedDictionary<string, string> parameters, string requestMethod, string requestURL)
        {
            AddDefaultOauthParams(parameters, consumerKey);
            parameters.Add("oauth_token", accessToken);

            StringBuilder paramString = new StringBuilder();
            foreach (KeyValuePair<string, string> param in parameters)
            {
                paramString.Append(Helper.UrlEncode(param.Key) + "=" + Helper.UrlEncode(param.Value) + "&");
            }
            paramString.Length -= 1; // Remove "&" at the last of string


            string requestHeader = Helper.UrlEncode(requestMethod) + "&" + Helper.UrlEncode(requestURL);
            string signatureData = requestHeader + "&" + Helper.UrlEncode(paramString.ToString());

            string signatureKey = Helper.UrlEncode(consumerSecret) + "&" + Helper.UrlEncode(accessTokenSecret);
            HMACSHA1 hmacsha1 = new HMACSHA1(Encoding.ASCII.GetBytes(signatureKey));
            byte[] signatureBytes = hmacsha1.ComputeHash(Encoding.ASCII.GetBytes(signatureData));
            return Convert.ToBase64String(signatureBytes);
        }

        // The below helper methods are modified from "WebRequestBuilder.cs" in Twitterizer(http://www.twitterizer.net/).
        // Here is its license.

        //-----------------------------------------------------------------------
        // <copyright file="WebRequestBuilder.cs" company="Patrick 'Ricky' Smith">
        //  This file is part of the Twitterizer library (http://www.twitterizer.net/)
        //
        //  Copyright (c) 2010, Patrick "Ricky" Smith (ricky@digitally-born.com)
        //  All rights reserved.
        //
        //  Redistribution and use in source and binary forms, with or without modification, are
        //  permitted provided that the following conditions are met:
        //
        //  - Redistributions of source code must retain the above copyright notice, this list
        //    of conditions and the following disclaimer.
        //  - Redistributions in binary form must reproduce the above copyright notice, this list
        //    of conditions and the following disclaimer in the documentation and/or other
        //    materials provided with the distribution.
        //  - Neither the name of the Twitterizer nor the names of its contributors may be
        //    used to endorse or promote products derived from this software without specific
        //    prior written permission.
        //
        //  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
        //  ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
        //  WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
        //  IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT,
        //  INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
        //  NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
        //  PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
        //  WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
        //  ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
        //  POSSIBILITY OF SUCH DAMAGE.
        // </copyright>
        // <author>Ricky Smith</author>
        // <summary>Provides the means of preparing and executing Anonymous and OAuth signed web requests.</summary>
        //-----------------------------------------------------------------------

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
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        private static string GenerateNonce()
        {
            return new System.Random().Next(123400, int.MaxValue).ToString("X");
        }

        #endregion

    }



}

