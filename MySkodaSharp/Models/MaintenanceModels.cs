using MySkodaSharp.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MySkodaSharp.Models
{
    // Models for responses of api/v3/vehicle-maintenance/vehicles/{vin} endpoint

    public class Contact
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }

    public class Maintenance
    {
        [JsonProperty("maintenanceReport")]
        public MaintenanceReport MaintenanceReport { get; set; }

        [JsonProperty("predictiveMaintenance")]
        public PredictiveMaintenance PredictiveMaintenance { get; set; }

        [JsonProperty("preferredServicePartner")]
        public ServicePartner PreferredServicePartner { get; set; }
    }

    public class MaintenanceReport
    {
        [JsonProperty("capturedAt")]
        public DateTime CapturedAt { get; set; }

        [JsonProperty("inspectionDueInDays")]
        public int InspectionDueInDays { get; set; }

        [JsonProperty("inspectionDueInKm")]
        public int? InspectionDueInKm { get; set; }

        [JsonProperty("mileageInKm")]
        public int? MileageInKm { get; set; }
        [JsonProperty("oilServiceDueInDays")]
        public int? OilServiceDueInDays { get; set; }

        [JsonProperty("oilServiceDueInKm")]
        public int? OilServiceDueInKm { get; set; }
    }
    public class OpeningHoursPeriod
    {
        [JsonProperty("openingTimes")]
        public List<TimeRange> OpeningTimes { get; set; }

        [JsonProperty("periodEnd")]
        public Weekday PeriodEnd { get; set; }

        [JsonProperty("periodStart")]
        public Weekday PeriodStart { get; set; }
    }

    public class PredictiveMaintenance
    {
        [JsonProperty("setting")]
        public PredictiveMaintenanceSettings Setting { get; set; }
    }

    public class PredictiveMaintenanceSettings
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("preferredChannel")]
        public CommunicationChannel? PreferredChannel { get; set; }

        [JsonProperty("serviceActivated")]
        public bool ServiceActivated { get; set; }
    }

    public class ServicePartner
    {
        [JsonProperty("address")]
        public Address Address { get; set; }

        [JsonProperty("brand")]
        public string Brand { get; set; }

        [JsonProperty("contact")]
        public Contact Contact { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("location")]
        public Coordinates Location { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("openingHours")]
        public List<OpeningHoursPeriod> OpeningHours { get; set; }

        [JsonProperty("partnerNumber")]
        public string PartnerNumber { get; set; }
    }

    public class TimeRange
    {
        [JsonProperty("to")]
        public TimeSpan End { get; set; }

        [JsonProperty("from")]
        public TimeSpan Start { get; set; }
    }
}
