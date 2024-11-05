using MySkodaSharp.Api.VwIdentity.Models;
using System;
using System.Security.Cryptography;
using System.Text;

namespace MySkodaSharp.Api.VwIdentity.Utils
{
    internal static class CodeChallengeUtils
    {
        // Generates the code challenge and verifier, then applies them to the query parameters.
        public static Action<Models.RequestParams> ChallengeAndVerifier(Models.RequestParams queryParameters)
        {
            string codeVerifier = GenerateCodeVerifier();
            string codeChallenge = GenerateS256Challenge(codeVerifier);

            queryParameters["code_challenge_method"] = "S256";
            queryParameters["code_challenge"] = codeChallenge;

            // Returns a function that applies the code verifier to the final query parameters.
            return finalQueryParameters => finalQueryParameters["code_verifier"] = codeVerifier;
        }

        public static string GetNonce()
        {
            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            string hash = GenerateS256Challenge(timestamp.ToString());

            return hash.Substring(0, hash.Length - 1);  // Remove last character
        }

        // Generates a random code verifier string.
        private static string GenerateCodeVerifier()
        {
            const int Length = 43;
            const string Charset = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-._~";
            var random = new Random();
            var verifier = new char[Length];
            for (int i = 0; i < Length; i++)
            {
                verifier[i] = Charset[random.Next(Charset.Length)];
            }
            return new string(verifier);
        }

        // Encodes the code verifier using SHA-256 and returns the Base64 URL-encoded challenge.
        private static string GenerateS256Challenge(string codeVerifier)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(codeVerifier));

                return Convert.ToBase64String(hash)
                    .Replace("+", "-")
                    .Replace("/", "_")
                    .Replace("=", ""); // Base64 URL encode
            }
        }
    }
}
