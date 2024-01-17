using Newtonsoft.Json;

namespace BlockC_Api.Classes.Json
{
    public class NewTokenRequest
    {
        [JsonProperty("apikey")]
        public string apiKey { get; set; }
    }
}
