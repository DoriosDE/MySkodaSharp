using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MySkodaSharp.Api.Clients
{
    internal abstract class BaseClient
    {
        private const string JSON_MEDIA_TYPE = "application/json";

        protected HttpClient Client { get; set; }

        protected BaseClient(HttpClient client = null)
        {
            Client = client ?? new HttpClient();
        }

        protected async Task<T> GetAsync<T>(string path) where T : class
        {
            var resp = await Client.GetAsync(path);
            resp.EnsureSuccessStatusCode();

            if (typeof(T).IsArray && typeof(T).GetElementType().IsAssignableFrom(typeof(byte)))
            {
                // byte[]
                return (await resp.Content.ReadAsByteArrayAsync()) as T;
            }

            var respContent = await resp.Content?.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(respContent))
            {
                throw new Exception($"Response content from endpoint {path} was empty.");
            }

            if (typeof(T).IsAssignableFrom(typeof(string)))
            {
                // string
                return respContent as T;
            }

            return JsonConvert.DeserializeObject<T>(respContent);
        }

        protected async Task<T> PostAsync<T>(string path, object body, bool isRawBody = false) where T : class
        {
            var content = body is HttpContent
                ? (HttpContent)body
                : new StringContent(isRawBody ? (body?.ToString() ?? "") : JsonConvert.SerializeObject(body ?? ""), Encoding.UTF8, JSON_MEDIA_TYPE);

            var resp = await Client.PostAsync(path, content);
            resp.EnsureSuccessStatusCode();

            var respContent = await resp.Content?.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(respContent))
            {
                throw new Exception($"Response content from endpoint {path} was empty.");
            }

            if (typeof(T).IsAssignableFrom(typeof(string)))
            {
                return respContent as T;
            }

            return JsonConvert.DeserializeObject<T>(respContent);
        }

        protected async Task<T> DeleteAsync<T>(string path) where T : class
        {
            var resp = await Client.DeleteAsync(path);
            resp.EnsureSuccessStatusCode();

            var respContent = await resp.Content?.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(respContent))
            {
                throw new Exception($"Response content from endpoint {path} was empty.");
            }

            if (typeof(T).IsAssignableFrom(typeof(string)))
            {
                return respContent as T;
            }

            return JsonConvert.DeserializeObject<T>(respContent);
        }

        protected async Task DeleteAsync(string path)
        {
            var resp = await Client.DeleteAsync(path);
            resp.EnsureSuccessStatusCode();
        }
    }
}
