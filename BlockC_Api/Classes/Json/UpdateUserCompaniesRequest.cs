using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockC_Api.Classes.Json
{
    public class UpdateUserCompaniesRequest
    {
        [JsonRequired]
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonRequired]
        [JsonProperty("userId")]
        public long UserID { get; set; }

        [JsonProperty("addCompanies")]
        public List<AddCompanies> AddCompaniesList { get; set; }

        [JsonProperty("removeCompanies")]
        public List<RemoveCompanies> RemoveCompaniesList { get; set; }



        public partial class AddCompanies
        {
            [JsonProperty("companyId")]
            public long CompanyID { get; set; }
        }

        public partial class RemoveCompanies
        {
            [JsonProperty("companyId")]
            public long CompanyID { get; set; }
        }

    }
}