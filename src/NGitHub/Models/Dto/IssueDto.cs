using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace NGitHub.Models.Dto {
    [JsonObject]
    public class IssueDto {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("assignee")]
        public string Assignee { get; set; }

        [JsonProperty("milestone")]
        public string Milestone { get; set; }

        [JsonProperty("labels")]
        public string[] Labels { get; set; }
    }
}
