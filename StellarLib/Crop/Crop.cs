using System;
using System.Text.Json.Serialization;

namespace StellarLib;

public class Crop : FarmHierarchyBase
{
    [JsonIgnore]
    public string identity { get { return id; } set { id = value; } }
    // [JsonIgnore]
    // public IFarmMember Parent { get { return null; } set { } }
    public string id { get; set; }
    public string status { get; set; }
    public DateTime createdDateTime { get; set; }
    public DateTime modifiedDateTime { get; set; }
    public string description { get; set; }
    public IDictionary<string, string> properties { get; set; }
}
