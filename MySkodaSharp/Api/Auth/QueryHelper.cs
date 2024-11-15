using System;
using System.Collections.Generic;
using System.Linq;

namespace MySkodaSharp.Api.Auth
{
    internal class QueryHelper
    {
        public static string AddParameters(string url, Dictionary<string, string> parameters)
        {
            return $"{url}?" + string.Join("&", parameters.Select(kvp => $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}"));
        }

        public static Dictionary<string, string> ParseQuery(string queryString)
        {
            var parameters = new Dictionary<string, string>();

            if (string.IsNullOrWhiteSpace(queryString))
                return parameters;

            queryString = queryString.TrimStart('?');

            var pairs = queryString.Split('&', StringSplitOptions.RemoveEmptyEntries);

            foreach (var pair in pairs)
            {
                var keyValue = pair.Split('=', 2);

                string key = Uri.UnescapeDataString(keyValue[0]);
                string value = keyValue.Length > 1 ? Uri.UnescapeDataString(keyValue[1]) : string.Empty;

                parameters[key] = value;
            }

            return parameters;
        }
    }
}
