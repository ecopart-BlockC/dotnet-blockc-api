using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockC_Api.Classes.Json
{
    public class DeleteEmissionRequest
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("emissionId")]
        public string EmissaoID { get; set; }

        [JsonProperty("userId")]
        public Int64 UsuarioID { get; set; }
    }
}