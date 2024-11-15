using MySkodaSharp.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MySkodaSharp.Models
{
    // Models for responses of api/v2/garage/vehicles/{vin}

    public class BatteryInfo
    {
        [JsonProperty("capacityInKWh")]
        public int Capacity { get; set; }
    }

    public class Capabilities
    {
        [JsonProperty("capabilities")]
        public List<Capability> CapabilityList { get; set; }
    }

    public class Capability
    {
        [JsonProperty("id")]
        public CapabilityId Id { get; set; }

        [JsonProperty("statuses")]
        public List<CapabilityStatus> Statuses { get; set; }

        public bool IsAvailable()
        {
            return Statuses.Count == 0;
        }
    }
    public class CompositeRender
    {
        [JsonProperty("layers")]
        public List<Render> Layers { get; set; }

        [JsonProperty("viewType")]
        public ViewType ViewType { get; set; }
    }

    public class Engine
    {
        [JsonProperty("capacityInLiters")]
        public float? CapacityInLiters { get; set; }

        [JsonProperty("powerInKW")]
        public int Power { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class ErrorInfo
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("type")]
        public ErrorType Type { get; set; }
    }

    public class Gearbox
    {
        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class Info
    {
        [JsonProperty("capabilities")]
        public Capabilities Capabilities { get; set; }

        [JsonProperty("compositeRenders")]
        public List<CompositeRender> CompositeRenders { get; set; }

        [JsonProperty("devicePlatform")]
        public string DevicePlatform { get; set; }

        [JsonProperty("errors")]
        public List<ErrorInfo> Errors { get; set; }

        [JsonProperty("licensePlate")]
        public string LicensePlate { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("renders")]
        public List<Render> Renders { get; set; }

        [JsonProperty("servicePartner")]
        public ServicePartnerInfo ServicePartner { get; set; }

        [JsonProperty("softwareVersion")]
        public string SoftwareVersion { get; set; }

        [JsonProperty("specification")]
        public Specification Specification { get; set; }

        [JsonProperty("state")]
        public VehicleState State { get; set; }
        [JsonProperty("vin")]
        public string Vin { get; set; }
        [JsonProperty("workshopModeEnabled")]
        public bool WorkshopModeEnabled { get; set; }

        public string GetModelName()
        {
            return $"{Specification.Model} {Specification.Engine.Type} {Specification.ModelYear} ({Specification.SystemModelId})";
        }

        public bool HasCapability(CapabilityId cap)
        {
            return Capabilities.CapabilityList.Exists(c => c.Id == cap);
        }

        public bool IsCapabilityAvailable(CapabilityId cap)
        {
            var capability = Capabilities.CapabilityList.Find(c => c.Id == cap);
            return capability != null && capability.IsAvailable();
        }
    }

    public class Render
    {
        [JsonProperty("order")]
        public int Order { get; set; }

        [JsonProperty("type")]
        public RenderType Type { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("viewPoint")]
        public string ViewPoint { get; set; }
    }

    public class ServicePartnerInfo
    {
        [JsonProperty("servicePartnerId")]
        public string Id { get; set; }
    }

    public class Specification
    {
        [JsonProperty("battery")]
        public BatteryInfo Battery { get; set; }

        [JsonProperty("body")]
        public BodyType Body { get; set; }

        [JsonProperty("engine")]
        public Engine Engine { get; set; }

        [JsonProperty("manufacturingDate")]
        public DateTime ManufacturingDate { get; set; }

        [JsonProperty("maxChargingPowerInKW")]
        public int? MaxChargingPower { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("modelYear")]
        public string ModelYear { get; set; }

        [JsonProperty("systemCode")]
        public string SystemCode { get; set; }

        [JsonProperty("systemModelId")]
        public string SystemModelId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("trimLevel")]
        public string TrimLevel { get; set; }
    }
}
