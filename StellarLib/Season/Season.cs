using System;
using System.Text.Json.Serialization;

namespace StellarLib;

public class Season : FarmHierarchyBase
{
    [JsonIgnore]
    public string identity { get { return id; } set { id = value; } }
    // [JsonIgnore]
    // public IFarmMember Parent { get { return null; } set { } }

    public DateTime startDateTime { get; set; }
    public DateTime endDateTime { get; set; }
    public string id { get; set; }
    public string status { get; set; }
    public DateTime createdDateTime { get; set; }
    public DateTime modifiedDateTime { get; set; }
    public string name { get; set; }
    public string description { get; set; }
}
