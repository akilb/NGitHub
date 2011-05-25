using Newtonsoft.Json;

namespace NGitHub.Models {
    [JsonObject]
    public class Branch {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "hash")]
        public string Hash { get; set; }
    }
}
