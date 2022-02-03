using System;
using System.Text.Json.Serialization;

namespace StellarLib;

public class Field : FarmHierarchyBase, IFarmMember
{
    private IFarmMember _parent;
    [JsonIgnore]
    public string identity { get { return fieldId; } set { fieldId = value; } }
    [JsonIgnore]
    public IFarmMember Parent { get { return _parent; } set { _parent = value; } }
    public string farmId { get; set; }
    public string farmerId { get; set; }
    public string primaryBoundaryId { get; set; }
    public string fieldId;
    public IList<string> boundaryIds  { get; set; }
    public string id { get; set; }
    public string status { get; set; }
    public string createdDateTime { get; set; }
    public string modifiedDateTime { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public IDictionary<string, string> properties { get; set; }
}
