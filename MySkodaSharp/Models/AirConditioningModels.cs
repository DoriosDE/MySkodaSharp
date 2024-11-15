using MySkodaSharp.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MySkodaSharp.Models
{
    // Models for responses of api/v2/air-conditioning endpoint

    public class AirConditioning
    {
        [JsonProperty("airConditioningAtUnlock")]
        public bool? AirConditioningAtUnlock { get; set; }

        [JsonProperty("airConditioningWithoutExternalPower")]
        public bool? AirConditioningWithoutExternalPower { get; set; }

        [JsonProperty("carCapturedTimestamp")]
        public DateTime? CarCapturedTimestamp { get; set; }

        [JsonProperty("chargerConnectionState")]
        public ConnectionState? ChargerConnectionState { get; set; }

        [JsonProperty("chargerLockState")]
        public ChargerLockedState? ChargerLockState { get; set; }

        [JsonProperty("errors")]
        public List<object> Errors { get; set; }

        [JsonProperty("estimatedDateTimeToReachTargetTemperature")]
        public DateTime? EstimatedDateTimeToReachTargetTemperature { get; set; }

        [JsonProperty("heaterSource")]
        public HeaterSource? HeaterSource { get; set; }

        [JsonProperty("seatHeatingActivated")]
        public SeatHeating SeatHeatingActivated { get; set; }

        [JsonProperty("state")]
        public AirConditioningState State { get; set; }

        [JsonProperty("steeringWheelPosition")]
        public Side SteeringWheelPosition { get; set; }

        [JsonProperty("targetTemperature")]
        public TargetTemperature TargetTemperature { get; set; }

        [JsonProperty("timers")]
        public List<Timer> Timers { get; set; }
        [JsonProperty("windowHeatingEnabled")]
        public bool? WindowHeatingEnabled { get; set; }

        [JsonProperty("windowHeatingState")]
        public WindowHeatingState WindowHeatingState { get; set; }
    }

    public class SeatHeating
    {
        [JsonProperty("frontLeft")]
        public bool FrontLeft { get; set; }

        [JsonProperty("frontRight")]
        public bool FrontRight { get; set; }
    }

    public class TargetTemperature
    {
        [JsonProperty("temperatureValue")]
        public double TemperatureValue { get; set; }

        [JsonProperty("unitInCar")]
        public TemperatureUnit UnitInCar { get; set; }
    }

    public class Timer
    {
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("selectedDays")]
        public List<Weekday> SelectedDays { get; set; }

        [JsonProperty("time")]
        public TimeSpan Time { get; set; }

        [JsonProperty("type")]
        public TimerMode Type { get; set; }
    }
    public class WindowHeatingState
    {
        [JsonProperty("front")]
        public OnOffState Front { get; set; }

        [JsonProperty("rear")]
        public OnOffState Rear { get; set; }

        [JsonProperty("unspecified")]
        public object Unspecified { get; set; }
    }
}
