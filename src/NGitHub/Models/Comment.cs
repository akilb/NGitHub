using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace NGitHub.Models {
    [JsonObject]
    public class Comment {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "gravatar_id")]
        public string GravatarId { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty(PropertyName = "updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty(PropertyName = "body")]
        public string Body { get; set; }

        [JsonProperty(PropertyName = "user")]
        public string User { get; set; }
    }

    [JsonObject]
    public class CommentsResult {
        [JsonProperty(PropertyName = "comments")]
        public List<Comment> Comments { get; set; }
    }
}
