using MySkodaSharp.Enums;
using System;
using System.Collections.Generic;

namespace MySkodaSharp.Models
{
    public class FixtureReportGet
    {
        public FixtureReportType Type { get; set; }
        public int VehicleId { get; set; }
        public bool Success { get; set; }
        public Endpoint Endpoint { get; set; }
        public string Raw { get; set; }
        public string Url { get; set; }
        public Dictionary<string, object> Result { get; set; }
        public string Error { get; set; }
    }

    public class FixtureVehicle
    {
        public int Id { get; set; }
        public string DevicePlatform { get; set; }
        public string SystemModelId { get; set; }
        public string Model { get; set; }
        public string ModelYear { get; set; }
        public string TrimLevel { get; set; }
        public string SoftwareVersion { get; set; }
        public List<Capability> Capabilities { get; set; }

        public static FixtureVehicle CreateFixtureVehicle(int id, Info info)
        {
            return new()
            {
                Id = id,
                DevicePlatform = info.DevicePlatform,
                SystemModelId = info.Specification.SystemModelId,
                Model = info.Specification.Model,
                ModelYear = info.Specification.ModelYear,
                TrimLevel = info.Specification.TrimLevel,
                SoftwareVersion = info.SoftwareVersion,
                Capabilities = info.Capabilities.CapabilityList
            };
        }
    }

    public class Fixture
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime GenerationTime { get; set; }
        public List<FixtureVehicle> Vehicles { get; set; }
        public List<FixtureReportGet> Reports { get; set; }
    }
}
