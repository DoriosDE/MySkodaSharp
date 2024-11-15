using System;
using System.Linq;

namespace MySkodaSharp.Api.Auth
{
    internal class Utils
    {
        public static string GenerateNonce()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new(Enumerable
                .Repeat(chars, 16)
                .Select(s => s[random.Next(s.Length)])
                .ToArray());
        }
    }
}
