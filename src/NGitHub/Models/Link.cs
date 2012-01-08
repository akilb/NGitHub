using Newtonsoft.Json;

namespace NGitHub.Models {
    [JsonObject]
    public class Link {
        [JsonProperty(PropertyName = "href")]
        public string HRef { get; set; }
    }
}
