using Newtonsoft.Json;

namespace NGitHub.Models {
    [JsonObject]
    public class Plan {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "collaborators")]
        public int NumberOfCollaborators { get; set; }

        [JsonProperty(PropertyName = "space")]
        public int Space { get; set; }

        [JsonProperty(PropertyName = "private_repos")]
        public int PrivateRepos { get; set; }
    }
}
