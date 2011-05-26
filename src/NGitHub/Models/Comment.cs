using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace NGitHub.Models {
    [JsonObject]
    public class Comment {
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty(PropertyName = "updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty(PropertyName = "body")]
        public string Body { get; set; }

        [JsonProperty(PropertyName = "user")]
        public User User { get; set; }
    }
}
