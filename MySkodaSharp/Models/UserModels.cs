using MySkodaSharp.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MySkodaSharp.Models
{
    // User that is using the API

    public class User
    {
        [JsonProperty("capabilities")]
        public List<UserCapability> Capabilities { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("dateOfBirth")]
        public DateTime? DateOfBirth { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("nickname")]
        public string Nickname { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("preferredContactChannel")]
        public string? PreferredContactChannel { get; set; }

        [JsonProperty("preferredLanguage")]
        public string PreferredLanguage { get; set; }

        [JsonProperty("profilePictureUrl")]
        public string ProfilePictureUrl { get; set; }
    }

    public class UserCapability
    {
        [JsonProperty("id")]
        public UserCapabilityId Id { get; set; }
    }
}
