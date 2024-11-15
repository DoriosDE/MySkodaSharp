using MySkodaSharp.Api.Auth.Models;
using System;
using System.Text.RegularExpressions;
using YamlDotNet.Serialization;

namespace MySkodaSharp.Api.Auth
{
    internal class CsrfParser
    {
        private static readonly Regex ScriptTagRegex = new Regex(@"<script\b[^>]*>(.*?)</script>", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex JsonObjectRegex = new Regex(@"window\._IDK\s*=\s*((?:.|\n)*?)$", RegexOptions.Compiled);

        public static CsrfState ExtractCsrf(string htmlContent)
        {
            CsrfState csrfState = null;

            var scriptMatches = ScriptTagRegex.Matches(htmlContent);
            foreach (Match scriptMatch in scriptMatches)
            {
                var scriptContent = scriptMatch.Groups[1].Value;
                var jsonMatch = JsonObjectRegex.Match(scriptContent);

                if (jsonMatch.Success)
                {
                    var yamlData = jsonMatch.Groups[1].Value;

                    var deserializer = new DeserializerBuilder()
                        .IgnoreUnmatchedProperties()
                        .Build();
                    csrfState = deserializer.Deserialize<CsrfState>(yamlData);
                    break;
                }
            }


            if (csrfState == null)
                throw new Exception("Failed to parse the CSRF information from the website.");

            return csrfState;
        }
    }
}
