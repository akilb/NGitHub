using Newtonsoft.Json;

namespace NGitHub.Models {
    [JsonObject]
    public class CommitComment : Comment {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "path")]
        public string Path { get; set; }

        [JsonProperty(PropertyName = "position")]
        public int Position { get; set; }

        [JsonProperty(PropertyName = "commit_id")]
        public string CommitId { get; set; }
    }
}
