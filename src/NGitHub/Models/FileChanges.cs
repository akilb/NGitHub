using Newtonsoft.Json;

namespace NGitHub.Models {
    [JsonObject]
    public class FileChanges {
        [JsonProperty(PropertyName = "filename")]
        public string FileName { get; set; }

        [JsonProperty(PropertyName = "additions")]
        public int Additions { get; set; }

        [JsonProperty(PropertyName = "deletions")]
        public int Deletions { get; set; }

        [JsonProperty(PropertyName = "total")]
        public int Total { get; set; }
    }
}
