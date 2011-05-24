using Newtonsoft.Json;

namespace NGitHub.Models {
    [JsonObject]
    public class PullRequest {
        [JsonProperty(PropertyName = "html_url")]
        public string HtmlUrl { get; set; }

        [JsonProperty(PropertyName = "diff_url")]
        public string DiffUrl { get; set; }

        [JsonProperty(PropertyName = "patch_url")]
        public string PatchUrl { get; set; }
    }
}
