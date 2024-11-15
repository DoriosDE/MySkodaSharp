using MySkodaSharp.Models;
using Newtonsoft.Json;

namespace MySkodaSharp.Api.Utils
{
    internal class StubData
    {
        public static string GarageJson
            => JsonConvert.SerializeObject(new Garage
            {
                Errors = new(),
                Vehicles = new()
                {
                    new GarageEntry
                    {
                        Name = "Test123",
                        Vin = "VIN123"
                    }
                }
            });
    }
}
