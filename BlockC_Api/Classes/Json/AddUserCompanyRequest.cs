using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockC_Api.Classes.Json
{
    public class AddUserCompanyRequest
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("userId")]
        public long UsuarioID { get; set; }

        [JsonProperty("companies")]
        public List<UserCompaniesList> Empresas { get; set; }
    }

    public partial class UserCompaniesList
    {
        [JsonProperty("companyId")]
        public long EmpresaID { get; set; }
    }

}