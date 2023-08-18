using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockC_Api.Classes.Json
{
    public class GetCategoriesRequest
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("scope")]
        public int Escopo { get; set; }

        [JsonProperty("categoryMode", NullValueHandling = NullValueHandling.Ignore)]
        public string CategoriaModo { get; set; }
    }
}