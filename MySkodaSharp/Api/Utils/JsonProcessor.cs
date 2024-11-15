using Newtonsoft.Json;
using System;

namespace MySkodaSharp.Api.Utils
{
    internal class JsonProcessor
    {
        public static string ProcessJson<T>(string data, bool anonymize, Func<T, T> anonymizationFn)
        {
            if (!anonymize) return data;
            var parsed = JsonConvert.DeserializeObject<T>(data);
            var anonymized = anonymizationFn(parsed);
            return JsonConvert.SerializeObject(anonymized);
        }
    }
}
