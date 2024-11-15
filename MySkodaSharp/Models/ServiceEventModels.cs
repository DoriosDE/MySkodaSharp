using MySkodaSharp.Enums;
using Newtonsoft.Json;
using System;

namespace MySkodaSharp.Models
{
    // Models related to service events from the MQTT broker

    public class ServiceEvent<T> where T : ServiceEventData
    {
        public T Data { get; set; }
        public ServiceEventName Name { get; set; }
        public string Producer { get; set; }
        [JsonProperty("timestamp")]
        public DateTime? Timestamp { get; set; }

        [JsonProperty("traceId")]
        public string TraceId { get; set; }

        public int Version { get; set; }
    }

    public class ServiceEventCharging : ServiceEvent<ServiceEventChargingData> { }

    public class ServiceEventChargingData : ServiceEventData
    {
        [JsonProperty("chargedRange")]
        public int ChargedRange { get; set; }

        [JsonProperty("mode")]
        public ChargeMode Mode { get; set; }

        public int Soc { get; set; }

        [JsonProperty("state")]
        public ChargingState State { get; set; }

        [JsonProperty("timeToFinish")]
        public int? TimeToFinish { get; set; }
    }

    public class ServiceEventData
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("vin")]
        public string Vin { get; set; }
    }
}
