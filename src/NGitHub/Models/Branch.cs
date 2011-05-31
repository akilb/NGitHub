using System.Collections.Generic;
using Newtonsoft.Json;

namespace NGitHub.Models {
    [JsonObject]
    public class Branch {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "hash")]
        public string Hash { get; set; }
    }

    [JsonObject]
    public class BranchesResult {
        public BranchesResult() {
            Branches = new Dictionary<string, string>();
        }

        [JsonProperty(PropertyName = "branches")]
        public Dictionary<string, string> Branches { get; set; }
    }
}
