using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockC_Api.Classes.Json
{
    public partial class ChangeUserCompanyRequest
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("userId")]
        public long UsuarioID { get; set; }

        [JsonProperty("companies")]
        public List<Empresas> Empresas { get; set; }
    }

    public partial class Empresas
    {
        [JsonProperty("companyId")]
        public long EmpresaID { get; set; }
    }


}