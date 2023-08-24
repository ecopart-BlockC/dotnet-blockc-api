using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static BlockC_Api.Classes.Json.UpdateUserCompaniesRequest;

namespace BlockC_Api.Classes.Json
{
    public class UpdateUserCompaniesResponse
    {
        [JsonProperty("addCompanies")]
        public List<AddCompanies> AddCompaniesList { get; set; }

        [JsonProperty("removeCompanies")]
        public List<RemoveCompanies> RemoveCompaniesList { get; set; }


        public partial class AddCompanies
        {
            [JsonProperty("companyId")]
            public long CompanyID { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }
        }

        public partial class RemoveCompanies
        {
            [JsonProperty("companyId")]
            public long CompanyID { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }
        }
    }
}