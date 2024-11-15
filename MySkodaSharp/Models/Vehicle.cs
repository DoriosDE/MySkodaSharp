using MySkodaSharp.Enums;
using System;

namespace MySkodaSharp.Models
{
    public class Vehicle
    {
        public Cache<Info> Info { get; }
        public Cache<Charging> Charging { get; set; }
        public Cache<Status> Status { get; set; }
        public Cache<AirConditioning> AirConditioning { get; set; }
        public Cache<Positions> Positions { get; set; }
        public Cache<DrivingRange> DrivingRange { get; set; }
        public Cache<TripStatistics> TripStatistics { get; set; }
        public Cache<Maintenance> Maintenance { get; }
        public Cache<Health> Health { get; set; }

        public Vehicle(Cache<Info> info, Cache<Maintenance> maintenance)
        {
            Info = info ?? throw new ArgumentNullException(nameof(info));
            Maintenance = maintenance ?? throw new ArgumentNullException(nameof(maintenance));
        }

        public bool HasCapability(CapabilityId capability)
            => Info.Result.HasCapability(capability);

        public bool IsCapabilityAvailable(CapabilityId capability)
            => Info.Result.IsCapabilityAvailable(capability);
    }
}
