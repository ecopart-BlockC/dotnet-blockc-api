using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockC_Api.Classes.Json
{
    public class AddCommentRequest
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("registryId")]
        public string RegistryID { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("createdAt")]
        public string CreatedAt { get; set; }

    }
}