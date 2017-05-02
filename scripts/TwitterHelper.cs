using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Twitter
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

        public static string UrlEncode(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            value = Uri.EscapeDataString(value);
            value = Regex.Replace(value, "(%[0-9a-f][0-9a-f])", c => c.Value.ToUpper());
            value = value
                .Replace("(", "%28")
                .Replace(")", "%29")
                .Replace("$", "%24")
                .Replace("!", "%21")
                .Replace("*", "%2A")
                .Replace("'", "%27");
            value = value.Replace("%7E", "~");
            return value;

        }
    }

}
