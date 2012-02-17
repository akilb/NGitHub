using Newtonsoft.Json;
using System;

namespace NGitHub.Models {
    [JsonObject]
    public class PullRequest {
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "html_url")]
        public string HtmlUrl { get; set; }

        [JsonProperty(PropertyName = "diff_url")]
        public string DiffUrl { get; set; }

        [JsonProperty(PropertyName = "patch_url")]
        public string PatchUrl { get; set; }

        [JsonProperty(PropertyName = "issue_url")]
        public string IssueUrl { get; set; }

        [JsonProperty(PropertyName = "number")]
        public int Number { get; set; }

        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "body")]
        public string Body { get; set; }

        [JsonProperty(PropertyName = "created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty(PropertyName = "updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty(PropertyName = "closed_at")]
        public DateTime? ClosedAt { get; set; }

        [JsonProperty(PropertyName = "merged_at")]
        public DateTime? MergedAt { get; set; }

        [JsonProperty(PropertyName = "_links")]
        public PullRequestLinks Links { get; set; }

        [JsonProperty(PropertyName = "merged")]
        public bool Merged { get; set; }

        [JsonProperty(PropertyName = "mergeable")]
        public bool Mergeable { get; set; }

        [JsonProperty(PropertyName = "merged_by")]
        public User MergedBy { get; set; }

        [JsonProperty(PropertyName = "comments")]
        public int Comments { get; set; }

        [JsonProperty(PropertyName = "commits")]
        public int Commits { get; set; }

        [JsonProperty(PropertyName = "additions")]
        public int Additions { get; set; }

        [JsonProperty(PropertyName = "deletions")]
        public int Deletions { get; set; }

        [JsonProperty(PropertyName = "changed_files")]
        public int ChangedFiles { get; set; }

        [JsonProperty(PropertyName = "head")]
        public CommitRange Head { get; set; }

        [JsonProperty(PropertyName = "base")]
        public CommitRange Base { get; set; }

        [JsonProperty(PropertyName = "user")]
        public User User { get; set; }
    }
}
