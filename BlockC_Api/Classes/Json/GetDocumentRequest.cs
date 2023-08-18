using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockC_Api.Classes.Json
{
    public class GetDocumentRequest
    {
        [JsonProperty("token", Required = Required.Always, NullValueHandling = NullValueHandling.Include)]
        public string Token { get; set; }

        [JsonProperty("registryId", Required = Required.Always, NullValueHandling = NullValueHandling.Include)]
        public string LancamentoID { get; set; }

    }
}