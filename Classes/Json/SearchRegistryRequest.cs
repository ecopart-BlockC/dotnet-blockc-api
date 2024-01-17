using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockC_Api.Classes.Json
{
    public class SearchRegistryRequest
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("userId")]
        public long UserId { get; set; }

        [JsonProperty("referredDocument")]
        public string DocumentName { get; set; }
    }
}