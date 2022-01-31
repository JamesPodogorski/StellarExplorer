using System;
using System.Text.Json.Serialization;

namespace StellarLib;

public class Farm : FarmHierarchyBase
{
        [JsonIgnore]
        public string identity { get { return farmId; } set { farmId = value; } }
        [JsonIgnore]
        // public IFarmMember Parent { get { return null; } set { } }
        public string farmId;
        public string name { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public IDictionary<string, string> properties { get; set; }
    
        // public IList<Farm> farms;
}