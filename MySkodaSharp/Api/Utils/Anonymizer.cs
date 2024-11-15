using MySkodaSharp.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace MySkodaSharp.Api.Utils
{
    public static class Anonymizer
    {
        private const string ACCESS_TOKEN = "eyJ0eXAiOiI0ODEyODgzZi05Y2FiLTQwMWMtYTI5OC0wZmEyMTA5Y2ViY2EiLCJhbGciOiJSUzI1NiJ9"; // Mock access token
        private const string DATE_OF_BIRTH = "2000-01-01";
        private const string EMAIL = "user@example.com";
        private const string FIRST_NAME = "John";
        private const string LAST_NAME = "Dough";
        private const string LICENSE_PLATE = "HH AA 1234";
        private const string NICKNAME = "Johnny D.";
        private const string PARTNER_NAME = "Example Service Partner";
        private const string PARTNER_NUMBER = "1111";
        private const string PHONE = "+49 1234 567890";
        private const string PROFILE_PICTURE_URL = "https://example.com/profile.jpg";
        private const string SERVICE_PARTNER_ID = "DEU11111";
        private const string URL = "https://example.com";
        private const string USER_ID = "b8bc126c-ee36-402b-8723-2c1c3dff8dec";
        private const string VEHICLE_NAME = "Example Car";
        private const string VIN = "TMOCKAA0AA000000";

        private static readonly Address ADDRESS = new()
        {
            City = "Example City",
            Street  = "Example Avenue",
            HouseNumber = "15",
            ZipCode = "54321",
            CountryCode = "DEU"
        };

        private static readonly Coordinates LOCATION = new()
        {
            Latitude = 53.470636 ,
            Longitude = 9.689872
        };

        private static readonly Regex VinRegex = new(@"TMB\w{14}");

        public static AirConditioning AnonymizeAirConditioning(AirConditioning airConditioning)
        {
            return airConditioning;
        }

        public static Charging AnonymizeCharging(Charging charging)
        {
            return charging;
        }

        public static DrivingRange AnonymizeDrivingRange(DrivingRange drivingRange)
        {
            return drivingRange;
        }

        public static Garage AnonymizeGarage(Garage garage)
        {
            garage.Vehicles = garage.Vehicles?.Select(AnonymizeGarageEntry).ToList();
            return garage;
        }

        public static GarageEntry AnonymizeGarageEntry(GarageEntry garageEntry)
        {
            garageEntry.Vin = VIN;
            garageEntry.Name = VEHICLE_NAME;
            return garageEntry;
        }

        public static Health AnonymizeHealth(Health health)
        {
            return health;
        }

        public static Info AnonymizeInfo(Info info)
        {
            info.Vin = VIN;
            info.Name = VEHICLE_NAME;
            info.LicensePlate = info.LicensePlate != null ? LICENSE_PLATE : null;
            info.ServicePartner.Id = info.ServicePartner?.Id != null ? SERVICE_PARTNER_ID : null;
            return info;
        }

        public static Maintenance AnonymizeMaintenance(Maintenance maintenance)
        {
            if (maintenance.PreferredServicePartner != null)
            {
                maintenance.PreferredServicePartner.Name = PARTNER_NAME;
                maintenance.PreferredServicePartner.PartnerNumber = PARTNER_NUMBER;
                maintenance.PreferredServicePartner.Id = SERVICE_PARTNER_ID;
                maintenance.PreferredServicePartner.Contact.Phone = PHONE;
                maintenance.PreferredServicePartner.Contact.Url = URL;
                maintenance.PreferredServicePartner.Contact.Email = EMAIL;
                maintenance.PreferredServicePartner.Address = ADDRESS;
                maintenance.PreferredServicePartner.Location = LOCATION;
            }

            if (maintenance.PredictiveMaintenance != null)
            {
                maintenance.PredictiveMaintenance.Setting.Email = EMAIL;
                maintenance.PredictiveMaintenance.Setting.Phone = PHONE;
            }

            return maintenance;
        }

        public static Positions AnonymizePositions(Positions positions)
        {
            positions.PositionList = positions.PositionList.Select(position =>
            {
                position.GpsCoordinates = LOCATION;
                position.Address = ADDRESS;
                return position;
            }).ToList();

            return positions;
        }

        public static Status AnonymizeStatus(Status status)
        {
            return status;
        }

        public static TripStatistics AnonymizeTripStatistics(TripStatistics tripStatistics)
        {
            return tripStatistics;
        }

        public static string AnonymizeUrl(string url)
        {
            return VinRegex.Replace(url, VIN);
        }

        public static User AnonymizeUser(User user)
        {
            user.Email = EMAIL;
            user.FirstName = FIRST_NAME;
            user.LastName = LAST_NAME;
            user.Nickname = NICKNAME;
            user.ProfilePictureUrl = PROFILE_PICTURE_URL;
            user.DateOfBirth = DateTime.ParseExact(DATE_OF_BIRTH, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            user.Phone = PHONE;
            return user;
        }
    }
}
