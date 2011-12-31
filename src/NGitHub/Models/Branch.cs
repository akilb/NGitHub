using System.Collections.Generic;
using Newtonsoft.Json;

namespace NGitHub.Models {
    [JsonObject]
    public class Branch {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "commit")]
        public Commit Commit { get; set; }
    }
}
