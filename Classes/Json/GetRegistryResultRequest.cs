using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockC_Api.Classes.Json
{
    public class GetRegistryResultRequest
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("registryId")]
        public string RegistroID { get; set; }


    }
}