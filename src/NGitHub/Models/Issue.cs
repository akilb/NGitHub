using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace NGitHub.Models {
    [JsonObject]
    public class Issue {
        public Issue() {
            Labels = new List<Label>();
        }

        [JsonProperty(PropertyName = "number")]
        public int Number { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "body")]
        public string Body { get; set; }

        [JsonProperty(PropertyName = "created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty(PropertyName = "updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty(PropertyName = "closed_at")]
        public DateTime ClosedAt { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "html_url")]
        public string HtmlUrl { get; set; }

        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }

        [JsonProperty(PropertyName = "comments")]
        public int Comments { get; set; }

        [JsonProperty(PropertyName = "user")]
        public User User { get; set; }

        [JsonProperty(PropertyName = "assignee")]
        public User Assignee { get; set; }

        [JsonProperty(PropertyName = "milestone")]
        public Milestone Milestone { get; set; }

        [JsonProperty(PropertyName = "labels")]
        public List<Label> Labels { get; set; }

        [JsonProperty(PropertyName = "pull_request")]
        public PullRequest PullRequest { get; set; }
    }

    [JsonObject]
    public class IssueResults {
        public IssueResults() {
            Issues = new List<Issue>();
        }

        [JsonProperty(PropertyName = "issues")]
        public List<Issue> Issues { get; set; }
    }
}
