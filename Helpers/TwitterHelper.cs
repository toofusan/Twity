using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Twity.Helpers
{
    public class Helper
    {
        public static SortedDictionary<string, string> ConvertToSortedDictionary(Dictionary<string, string> APIParams)
        {
            SortedDictionary<string, string> parameters = new SortedDictionary<string, string>();
            foreach (KeyValuePair<string, string> APIParam in APIParams)
            {
                parameters.Add(APIParam.Key, APIParam.Value);
            }
            return parameters;
        }

        public static string GenerateRequestparams(SortedDictionary<string, string> parameters)
        {
            StringBuilder requestParams = new StringBuilder();
            foreach (KeyValuePair<string, string> param in parameters)
            {
                requestParams.Append(Helper.UrlEncode(param.Key) + "=" + Helper.UrlEncode(param.Value) + "&");
            }
            requestParams.Length -= 1; // Remove "&" at the last of string
            return requestParams.ToString();
        }

        public static string UrlEncode(string s)
        {
            if (String.IsNullOrEmpty(s)) return "";

            string v = Uri.EscapeDataString(s);
            v = v.Replace("!", "%21").Replace("(", "%28").Replace(")", "%29").Replace("*", "%2A").Replace("'", "%27");
            return v;
        }
        
    }

}
