namespace StellarLib;

using System;
using System.Text.Json.Serialization;
 public class StellarConfig
    {
        [JsonIgnore]
        public string? client_id
        {
            get
            {
                return Environment.GetEnvironmentVariable("client_id");
            }
            private set { }
        }
        public string tenant_id { get; set; } = string.Empty;
        [JsonIgnore]
        public string? application_secret
        {
            get
            {
                return Environment.GetEnvironmentVariable("application_secret");
            }
            private set { }
        }
        public string resource { get; set; } = string.Empty;
        public string host { get; set; } = string.Empty;
        public string grant_type { get; set; } = string.Empty;
        public string apiVersion { get; set; } = string.Empty;
    }