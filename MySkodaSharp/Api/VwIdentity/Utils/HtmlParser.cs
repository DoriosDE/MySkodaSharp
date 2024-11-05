using MySkodaSharp.Api.VwIdentity.Models;
using System.Text.RegularExpressions;

namespace MySkodaSharp.Api.VwIdentity.Utils
{
    internal static class HtmlParser
    {
        public static RequestParams ExtractParameters(string htmlContent, params string[] patterns)
        {
            var requestParams = new RequestParams();

            foreach (var pattern in patterns)
            {
                // Match all key-value pairs based on the provided regex pattern
                var matches = Regex.Matches(htmlContent, pattern);

                foreach (Match kvp in matches)
                {
                    if (kvp.Groups.Count == 3) // Ensure we have both key and value
                    {
                        string key = kvp.Groups[1].Value; // First group should be the key
                        string value = kvp.Groups[2].Value; // Second group should be the value

                        // Add the key-value pair to RequestParams
                        requestParams[key] = value != "null" ? value : null;
                    }
                }
            }

            return requestParams;
        }
    }
}
