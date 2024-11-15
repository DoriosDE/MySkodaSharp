using MySkodaSharp.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MySkodaSharp.Models
{
    // Models for responses of api/v1/vehicle-health-report/warning-lights endpoint

    public class Health
    {
        [JsonProperty("capturedAt")]
        public DateTime? CapturedAt { get; set; }

        [JsonProperty("mileageInKm")]
        public int? MileageInKm { get; set; }

        [JsonProperty("warningLights")]
        public List<WarningLight> WarningLights { get; set; }
    }

    public class WarningLight
    {
        [JsonProperty("category")]
        public WarningLightCategory Category { get; set; }

        [JsonProperty("defects")]
        public List<object> Defects { get; set; }
    }
}
