using MySkodaSharp.Enums;
using Newtonsoft.Json;

namespace MySkodaSharp.Models
{
    // Models for operation requests, returned by the MQTT broker

    public class OperationRequest
    {
        [JsonProperty("errorCode")]
        public string ErrorCode { get; set; }

        [JsonProperty("operation")]
        public OperationName Operation { get; set; }

        [JsonProperty("requestId")]
        public string RequestId { get; set; }

        [JsonProperty("status")]
        public OperationStatus Status { get; set; }

        [JsonProperty("traceId")]
        public string TraceId { get; set; }

        [JsonProperty("version")]
        public int Version { get; set; }
    }
}
