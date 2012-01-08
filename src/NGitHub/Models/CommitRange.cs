using Newtonsoft.Json;

namespace NGitHub.Models {
    [JsonObject]
    public class CommitRange {
        [JsonProperty(PropertyName = "label")]
        public string Label { get; set; }

        [JsonProperty(PropertyName = "ref")]
        public string Ref { get; set; }

        [JsonProperty(PropertyName = "sha")]
        public string Sha { get; set; }

        [JsonProperty(PropertyName = "user")]
        public User User { get; set; }

        [JsonProperty(PropertyName = "repo")]
        public Repository Repo { get; set; }
    }
}
