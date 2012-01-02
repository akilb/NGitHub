using System;
using Newtonsoft.Json;

namespace NGitHub.Models {
    [JsonObject]
    public class Milestone {
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "number")]
        public int Number { get; set; }

        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "creator")]
        public User Creator { get; set; }

        [JsonProperty(PropertyName = "open_issues")]
        public int OpenIssues { get; set; }

        [JsonProperty(PropertyName = "closed_issues")]
        public int ClosedIssues { get; set; }

        [JsonProperty(PropertyName = "created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty(PropertyName = "due_on")]
        public DateTime? DueOn { get; set; }
    }
}
