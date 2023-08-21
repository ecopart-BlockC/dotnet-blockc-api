using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockC_Api.Classes.Json
{
    public class RemoveCompanyRequest
    {
        [JsonRequired]
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonRequired]
        [JsonProperty("companyId")]
        public long EmpresaID { get; set; }

        [JsonRequired]
        [JsonProperty("userId")]
        public long UsuarioID { get; set; }

    }
}