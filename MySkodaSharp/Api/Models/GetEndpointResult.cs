namespace MySkodaSharp.Api.Models
{
    public class GetEndpointResult<T>
    {
        public string Url { get; set; }
        public string Raw { get; set; }
        public T Result { get; set; }

        public GetEndpointResult(string url, string raw, T result)
        {
            Url = url;
            Raw = raw;
            Result = result;
        }
    }
}
