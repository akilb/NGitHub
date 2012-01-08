using Newtonsoft.Json;

namespace NGitHub.Models {
    [JsonObject]
    public class PullRequestLinks {
        [JsonProperty("self")]
        public Link Self { get; set; }

        [JsonProperty("html")]
        public Link Html { get; set; }

        [JsonProperty("comments")]
        public Link Comments { get; set; }

        [JsonProperty("review_comments")]
        public Link ReviewComments { get; set; }
    }
}
