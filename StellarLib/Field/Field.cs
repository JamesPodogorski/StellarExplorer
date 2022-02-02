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
        public string farmerId;
        public string fieldId;
        public string farmId { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public IDictionary<string, string> properties { get; set; }
        public IList<Boundary> boundaries;
}
