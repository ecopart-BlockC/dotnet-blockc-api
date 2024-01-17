using Newtonsoft.Json;

namespace BlockC_Api.Classes.Json
{
    internal class GetInstitutionalInformationRequest
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("CompanyID")]
        public int CompanyId { get; set; }
    }
}