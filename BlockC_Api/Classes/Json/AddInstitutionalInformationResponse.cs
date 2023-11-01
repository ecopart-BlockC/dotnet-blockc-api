using Newtonsoft.Json;

namespace BlockC_Api.Classes.Json
{
    internal class AddInstitutionalInformationResponse
    {
        [JsonProperty("institutionalInformationId")]
        public int InstitutionalInformationId { get; set; }

    }
}