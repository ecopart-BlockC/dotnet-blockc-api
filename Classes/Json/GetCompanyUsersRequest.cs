using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockC_Api.Classes.Json
{
    public class GetCompanyUsersRequest
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("userId", NullValueHandling = NullValueHandling.Ignore)]
        public long UsuarioID { get; set; }

        [JsonProperty("companies")]
        public List<Companies> EmpresaLista { get; set; }


        public partial class Companies
        {
            [JsonProperty("companyId")]
            public long EmpresaID { get; set; }
        }

    }
}