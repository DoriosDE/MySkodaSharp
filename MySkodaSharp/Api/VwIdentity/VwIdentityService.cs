using Microsoft.Extensions.Logging;
using MySkodaSharp.Api.Clients;
using MySkodaSharp.Api.VwIdentity.Models;
using MySkodaSharp.Api.VwIdentity.Utils;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Net;
using System.Threading.Tasks;

namespace MySkodaSharp.Api.VwIdentity
{
    internal class VwIdentityService
    {
        private const string BASE_URL = "https://identity.vwgroup.io";
        private const string AUTH_URL = BASE_URL + "/oidc/v1/authorize";

        private readonly ILogger _logger;
        private readonly VwIdentityClient _vwIdentityClient;

        public VwIdentityService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<VwIdentityService>();

            _vwIdentityClient = new VwIdentityClient();
        }

        public async Task<RequestParams> LoginAsync(RequestParams queryParams, string user, string password)
        {
            Action<RequestParams> verify = null;

            // Add code challenge if response type contains "code"
            if (queryParams["response_type"] != null && queryParams["response_type"].Contains("code"))
            {
                verify = CodeChallengeUtils.ChallengeAndVerifier(queryParams);
            }

            // Authorize
            var (authorizeParams, authorizeAction) = await Authorize($"{AUTH_URL}?{queryParams.ToQueryString()}");

            // Identify
            var identifyUrl = $"{BASE_URL}{authorizeAction}";
            authorizeParams.Add("email", user);

            var (identifyParams, postAction, identifierUrl, identifyError) = await Identify(identifyUrl, authorizeParams);

            if (!string.IsNullOrEmpty(identifyError))
                throw new Exception(identifyError);

            // Authenticate
            identifyParams.RenameKey("csrf_token", "_csrf");
            identifyParams.Add("email", user);
            identifyParams.Add("password", password);

            var authenticateUrl = identifyUrl.Replace(identifierUrl, postAction);
            var (authenticateParams, needsTermsAndConditions) = await Authenticate(authenticateUrl, identifyParams);

            if (needsTermsAndConditions)
            {
                // Terms and Conditions
                throw new Exception("Terms and conditions need to be accepted");

                var termsUrl = authenticateUrl.Replace(postAction, "terms-and-conditions");
                await AcceptTermsAndConditions(termsUrl, authenticateParams);
            }

            verify?.Invoke(authenticateParams);

            return authenticateParams;
        }

        private async Task<(RequestParams, string)> Authorize(string url)
        {
            // Add nonce and state to query parameters
            var nonce = CodeChallengeUtils.GetNonce();
            var state = Guid.NewGuid().ToString();
            url = $"{url}&nonce={nonce}&state={state}";

            var responseBody = await _vwIdentityClient.Authorize(url);
            // modify responseBody for better parameter extraction
            responseBody = responseBody.Replace("action=", "name=\"action\" value=");

            string inputPattern = @"<input[^>]+name=['""]?([^'""]+)['""]?[^>]*value=['""]?([^'""]+)['""]?";
            string actionPattern = @"<form[^>]+name=['""]?([^'""]+)['""]?[^>]*value=['""]?([^'""]+)['""]?";

            var responseParams = HtmlParser.ExtractParameters(responseBody, inputPattern, actionPattern);

            var action = responseParams["action"];

            // only retain specified keys
            responseParams.RetainKeys("_csrf", "relayState", "hmac");

            return (responseParams, action);
        }

        private async Task<(RequestParams, string , string, string)> Identify(string url, RequestParams requestParams)
        {
            var responseBody = await _vwIdentityClient.Identify(url, requestParams);

            var jsonPattern = @"(?<key>csrf_token|hmac|relayState|postAction|identifierUrl|error)""*:[ ]*['"" ]*?(?<value>[^""',]+)[^""',]?";
            var responseParams = HtmlParser.ExtractParameters(responseBody, jsonPattern);

            var postAction = responseParams["postAction"];
            var identifierUrl = responseParams["identifierUrl"];
            var error = responseParams["error"];

            // only retain specified keys
            responseParams.RetainKeys("csrf_token", "relayState", "hmac");

            return (responseParams, postAction, identifierUrl, error);
        }

        public async Task<(RequestParams, bool)> Authenticate(string url, RequestParams requestParams)
        {
            var response = await _vwIdentityClient.Authenticate(url, requestParams);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (response.StatusCode >= HttpStatusCode.BadRequest)
                throw new Exception(response.ReasonPhrase);

            if (responseBody.Contains("content=\"termsAndConditions\""))
            {
                var jsonPattern = @"(?<key>window._IDK) = (?<value>[^<]+)";
                var responseParams = HtmlParser.ExtractParameters(responseBody, jsonPattern);
                var windowIdk = JsonConvert.DeserializeObject<dynamic>(responseParams["window._IDK"]);

                var csrf_parameterName = windowIdk.csrf_parameterName.ToString();
                var csrf_token = windowIdk.csrf_token.ToString();

                var templateModel = windowIdk.templateModel;
                var relayState = templateModel.relayState.ToString();
                var hmac = templateModel.hmac.ToString();

                var legalDocuments = templateModel.legalDocuments;

                responseParams = new RequestParams
                {
                    { csrf_parameterName, windowIdk.csrf_token.ToString() },
                    { "relayState", relayState },
                    { "hmac", hmac },
                };

                for (int i = 0; i < ((IList)legalDocuments).Count; i++)
                {
                    responseParams.Add($"legalDocuments[{i}].name", legalDocuments[i].name.ToString());
                    responseParams.Add($"legalDocuments[{i}].language", legalDocuments[i].language.ToString());
                    responseParams.Add($"legalDocuments[{i}].version", legalDocuments[i].version.ToString());
                    responseParams.Add($"legalDocuments[{i}].updated", legalDocuments[i].updated.ToString());
                    responseParams.Add($"legalDocuments[{i}].countryCode", legalDocuments[i].countryCode.ToString());
                    responseParams.Add($"legalDocuments[{i}].skippable", legalDocuments[i].skippable.ToString());
                    responseParams.Add($"legalDocuments[{i}].declinable", legalDocuments[i].declinable.ToString());
                }

                return (responseParams, true);
            }
            else
            {
                var location = response.Headers.Location;
                if (location == null)
                    throw new Exception("Redirect location not found");

                location = new Uri(location.ToString().Replace("#", "?"));
                var responseParams = RequestParams.ParseQueryString(location.Query);

                return (responseParams, false);
            }
        }

        public async Task<RequestParams> AcceptTermsAndConditions(string url, RequestParams requestParams)
        {
            var response = await _vwIdentityClient.AcceptTermsAndConditions(url, requestParams);
            var responseBody = await response.Content.ReadAsStringAsync();

            var jsonPattern = @"(?<key>csrf_token|hmac|relayState)""*:[ ]*['"" ]*?(?<value>[^""',]+)[^""',]?";
            var responseParams = HtmlParser.ExtractParameters(responseBody, jsonPattern);

            return responseParams;
        }
    }
}
