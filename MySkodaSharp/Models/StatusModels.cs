using MySkodaSharp.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MySkodaSharp.Models
{
    // Models for responses of api/v2/vehicle-status/{vin} endpoint

    public class Detail
    {
        [JsonProperty("bonnet")]
        public OpenState Bonnet { get; set; }

        [JsonProperty("sunroof")]
        public OpenState Sunroof { get; set; }

        [JsonProperty("trunk")]
        public OpenState Trunk { get; set; }
    }

    public class Overall
    {
        [JsonProperty("doors")]
        public OpenState Doors { get; set; }

        [JsonProperty("doorsLocked")]
        public DoorLockedState DoorsLocked { get; set; }

        [JsonProperty("lights")]
        public OnOffState Lights { get; set; }

        [JsonProperty("locked")]
        public DoorLockedState Locked { get; set; }

        [JsonProperty("windows")]
        public OpenState Windows { get; set; }
    }

    public class RenderMode
    {
        [JsonProperty("oneAndHalfX")]
        public string OneAndHalfX { get; set; }

        [JsonProperty("oneX")]
        public string OneX { get; set; }
        [JsonProperty("threeX")]
        public string ThreeX { get; set; }

        [JsonProperty("twoX")]
        public string TwoX { get; set; }
    }

    public class Renders
    {
        [JsonProperty("darkMode")]
        public RenderMode DarkMode { get; set; }

        [JsonProperty("lightMode")]
        public RenderMode LightMode { get; set; }
    }

    public class Status
    {
        [JsonProperty("carCapturedTimestamp")]
        public DateTime? CarCapturedTimestamp { get; set; }

        [JsonProperty("detail")]
        public Detail Detail { get; set; }

        public DoorWindowState LeftBackDoor => GetDoorWindowState(CarBodyElements.LEFT_BACK_DOOR);

        public DoorWindowState LeftFrontDoor => GetDoorWindowState(CarBodyElements.LEFT_FRONT_DOOR);

        [JsonProperty("overall")]
        public Overall Overall { get; set; }

        [JsonProperty("renders")]
        public Renders Renders { get; set; }
        public DoorWindowState RightBackDoor => GetDoorWindowState(CarBodyElements.RIGHT_BACK_DOOR);

        public DoorWindowState RightFrontDoor => GetDoorWindowState(CarBodyElements.RIGHT_FRONT_DOOR);

        private List<int> ExtractWindowDoorStateListFromUrl()
        {
            try
            {
                var parsedUrl = new Uri(Renders.LightMode.OneX);
                var query = System.Web.HttpUtility.ParseQueryString(parsedUrl.Query);
                var vehicleStateValues = query["vehicleState"]?.Split('-');
                if (vehicleStateValues != null)
                {
                    var integerList = Array.ConvertAll(vehicleStateValues, int.Parse);
                    return new List<int>(integerList).GetRange(0, 4);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to deduce doors/windows state from vehicle status URL", ex);
            }
            return new List<int> { 1, 1, 1, 1 };
        }

        private DoorWindowState GetDoorWindowState(CarBodyElements element)
        {
            var doorStates = ExtractWindowDoorStateListFromUrl();
            var state = doorStates[(int)element];

            return state switch
            {
                (int)CarWindowDoorStates.ALL_CLOSED => DoorWindowState.CLOSED,
                (int)CarWindowDoorStates.DOOR_OPEN => DoorWindowState.DOOR_OPEN,
                (int)CarWindowDoorStates.WINDOW_OPEN => DoorWindowState.WINDOW_OPEN,
                _ => DoorWindowState.CLOSED // Default state if unknown
            };
        }
    }
}
