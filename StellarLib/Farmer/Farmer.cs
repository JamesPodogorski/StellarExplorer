using System;
using System.Text.Json.Serialization;

namespace StellarLib;

public class Farmer : FarmHierarchyBase
{
        [JsonIgnore]
        public string identity { get { return farmerId; } set { farmerId = value; } }
        [JsonIgnore]
        // public IFarmMember Parent { get { return null; } set { } }
        public string farmerId;
        public string name { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public IDictionary<string, string> properties { get; set; }
    
        // public IList<Farm> farms;
}