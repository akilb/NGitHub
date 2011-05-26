using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace NGitHub.Models {
    [JsonObject]
    public class Commit {
        // TODO: deserialize this...
        [JsonIgnore]
        public List<string> Parents { get; set; }

        [JsonProperty(PropertyName = "author")]
        public User Author { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "committed_date")]
        public DateTime CommittedDate { get; set; }

        [JsonProperty(PropertyName = "authored_date")]
        public DateTime AuthoredDate { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        // TODO: Remove this property...
        private const int ShortMessageLength = 140;
        [JsonIgnore]
        public string ShortMessage {
            get {
                var msg = Message ?? string.Empty;

                return msg.Length >= ShortMessageLength ?
                    msg.Substring(0, ShortMessageLength) + "..." :
                    msg;
            }
        }

        [JsonProperty(PropertyName = "tree")]
        public string Tree { get; set; }

        [JsonProperty(PropertyName = "committer")]
        public User Committer { get; set; }
    }
}
