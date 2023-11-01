using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockC_Api.Classes.Json
{
    public class GetInstitutionalInformationResponse
    {
        [JsonProperty("institutionalInformation")]
        public List<InstitutionalInformations> institutionalInformationList { get; set; }
    }

    public partial class InstitutionalInformations
    {
        [JsonProperty("institutionalInformationId")]
        public int institutionalInformationId { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

    }
}