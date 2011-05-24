using Newtonsoft.Json;

namespace NGitHub.Models {
    [JsonObject]
    public class Label {
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "color")]
        public string Color { get; set; }
    }
}
