using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockC_Api.Classes.Json
{
    public class DeleteRegistryRequest
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("registryId")]
        public string EntryID { get; set; }

        [JsonProperty("userId")]
        public long UserID { get; set; }

    }
}