using Newtonsoft.Json;

namespace NGitHub.Models {
    [JsonObject]
    public class CommitDetails {
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "sha")]
        public string Sha { get; set; }

        [JsonProperty(PropertyName = "author")]
        public CommitUserSummary Author { get; set; }

        [JsonProperty(PropertyName = "committer")]
        public CommitUserSummary Committer { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        // TODO: Tree
    }
}
