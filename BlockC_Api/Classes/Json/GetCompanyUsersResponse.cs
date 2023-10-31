using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockC_Api.Classes.Json
{
    public class GetCompanyUsersResponse
    {
        [JsonProperty("users")]
        public List<CompanyUserList> UsersList { get; set; }
    }

    public partial class CompanyUserList
    {
        [JsonProperty("userId")]
        public long UsuarioID { get; set; }

        [JsonProperty("name")]
        public string Nome { get; set; }

        [JsonProperty("lastName")]
        public string Sobrenome { get; set; }

        [JsonProperty("accountType")]
        public string Tipo { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("active")]
        public Boolean Ativo { get; set; }

        [JsonProperty("companies")]
        public List<userCompanies> Empresas { get; set; }


    }

}