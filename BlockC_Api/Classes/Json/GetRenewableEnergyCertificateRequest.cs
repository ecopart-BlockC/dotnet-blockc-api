using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockC_Api.Classes.Json
{
    public class GetRenewableEnergyCertificateRequest
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("companyId")]
        public string CompanyId { get; set; }
    }
}