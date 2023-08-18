using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockC_Api.Classes.Json
{
    public class DeleteUserRequest
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("companyId")]
        public long EmpresaID { get; set; }

        [JsonProperty("userId")]
        public long UsuarioID { get; set; }

    }
}