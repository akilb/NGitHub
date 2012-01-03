using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace NGitHub.Models {
    [JsonObject]
    public class Commit {
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "sha")]
        public string Sha { get; set; }

        [JsonProperty(PropertyName = "commit")]
        public CommitDetails Details { get; set; }

        [JsonProperty(PropertyName = "author")]
        public User Author { get; set; }

        [JsonProperty(PropertyName = "committer")]
        public User Committer { get; set; }

        [JsonProperty(PropertyName = "parents")]
        public List<Commit> Parents { get; set; }

        [JsonProperty(PropertyName = "stats")]
        public FileChanges Stats { get; set; }

        [JsonProperty(PropertyName = "files")]
        public List<FileChanges> Files { get; set; }
    }
}
