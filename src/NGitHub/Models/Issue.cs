using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace NGitHub.Models {
    [JsonObject]
    public class Issue {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "html_url")]
        public string HtmlUrl { get; set; }

        [JsonProperty(PropertyName = "number")]
        public int Number { get; set; }

        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "body")]
        public string Body { get; set; }

        [JsonProperty(PropertyName = "user")]
        public User User { get; set; }

        [JsonProperty(PropertyName = "labels")]
        public List<Label> Labels { get; set; }

        [JsonProperty("assignee")]
        public User Assignee { get; set; }

        [JsonProperty("milestone")]
        public Milestone Milestone { get; set; }

        [JsonProperty(PropertyName = "comments")]
        public int Comments { get; set; }

        [JsonProperty("pull_request")]
        public PullRequest PullRequest { get; set; }

        [JsonProperty(PropertyName = "closed_at")]
        public DateTime? ClosedAt { get; set; }

        [JsonProperty(PropertyName = "created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty(PropertyName = "updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
