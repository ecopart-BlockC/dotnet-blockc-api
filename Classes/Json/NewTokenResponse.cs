using Newtonsoft.Json;

namespace BlockC_Api.Classes.Json
{
    public class NewTokenResponse
    {
        [JsonProperty("token")]
        public string Token { get; set; }

    }
}
