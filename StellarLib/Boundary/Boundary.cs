using System;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace StellarLib;

public class Boundary : FarmHierarchyBase, IFarmMember
{
    private IFarmMember _parent;
    [JsonIgnore]
    public string identity
    {
        get
        {
            // The Boundary Data provided does not contain a boundarId.  If this is the case, then derive the Id from the name
            if (boundaryId == null | string.Empty.Equals(boundaryId))
            {
                boundaryId = Regex.Replace(name, @"[\s*, ']", string.Empty);
            }
            return boundaryId;
        }
        set
        {
            boundaryId = value;
        }
    }
    [JsonIgnore]
    public IFarmMember Parent { get { return _parent; } set { _parent = value; parentId = value.identity; } }
    // [JsonIgnore]
    public string farmerId { get; set; }
    public string parentId { get; set; }
    public bool isPrimary { get; set; }
    public float acreage { get; set; }
    public string parentType { get; set; }
    public string id { get; set; }
    public string status { get; set; }
    public DateTime createdDateTime { get; set; }
    public DateTime modifiedDateTime { get; set; }
    public string name { get; set; }
    public string description { get; set; }

    // TODO: What to do with boundaryId
    public string boundaryId;

    // TODO: Build Geometry class
    // public Geometry geometry { get; set; }


    public IDictionary<string, string> properties { get; set; }

    // TODO: Apparently applications, tillage, harvests and planting are not part of the Farm Hierarchy
    // public IList<Application> applications;
    // public IList<Harvest> harvests;
    // public IList<Tillage> tillages;
    // public IList<Planting> plantings;

    // TODO: remove numb, it is used for verification
    public int numb { get; set; }
}
