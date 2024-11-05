using System;
using System.Collections.Generic;
using System.Linq;

namespace MySkodaSharp.Api.VwIdentity.Models
{
    internal class RequestParams : Dictionary<string, string>
    {
        public RequestParams() { }

        public string ToQueryString()
        {
            return string.Join("&", this.Select(kvp => $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}"));
        }

        public void RetainKeys(params string[] keysToRetain)
        {
            foreach (var key in Keys.ToList())
            {
                if (!keysToRetain.Contains(key))
                {
                    Remove(key);
                }
            }
        }

        public bool RenameKey(string existingKey, string newKey)
        {
            if (!ContainsKey(existingKey))
            {
                return false;
            }

            this[newKey] = this[existingKey];
            Remove(existingKey);
            return true;
        }

        public static RequestParams ParseQueryString(string queryString)
        {
            var requestParams = new RequestParams();

            if (string.IsNullOrWhiteSpace(queryString))
                return requestParams;

            queryString = queryString.TrimStart('?');

            var pairs = queryString.Split('&', StringSplitOptions.RemoveEmptyEntries);

            foreach (var pair in pairs)
            {
                var keyValue = pair.Split('=', 2);

                string key = Uri.UnescapeDataString(keyValue[0]);
                string value = keyValue.Length > 1 ? Uri.UnescapeDataString(keyValue[1]) : string.Empty;

                requestParams[key] = value;
            }

            return requestParams;
        }
    }
}
