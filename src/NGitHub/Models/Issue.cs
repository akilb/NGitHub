using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace NGitHub.Models {
    [JsonObject]
    public class Issue {
        [JsonProperty(PropertyName = "gravatar_id")]
        public string GravatarId { get; set; }

        [JsonProperty(PropertyName = "number")]
        public int Number { get; set; }

        [JsonProperty(PropertyName = "votes")]
        public int Votes {get;set;}

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

        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }

        [JsonProperty(PropertyName = "comments")]
        public int Comments { get; set; }

        [JsonProperty(PropertyName = "user")]
        public string User { get; set; }

        // TODO: deserialize this.
        //[JsonProperty(PropertyName = "labels")]
        [JsonIgnore]
        public List<Label> Labels { get; set; }
    }

    [JsonObject]
    public class IssueResult {
        [JsonProperty(PropertyName = "issue")]
        public Issue Issue { get; set; }
    }

    [JsonObject]
    public class IssuesResult {
        [JsonProperty(PropertyName = "issues")]
        public List<Issue> Issues { get; set; }
    }
}
