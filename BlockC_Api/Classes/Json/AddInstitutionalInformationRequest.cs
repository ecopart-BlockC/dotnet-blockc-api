using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockC_Api.Classes.Json
{
    public class AddInstitutionalInformationRequest
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("CompanyID")]
        public int CompanyId { get; set; }

        [JsonProperty("Content")]

        public string Content { get; set;}

        [JsonProperty("UserID")]

        public string UserId { get; set; }
    }
}