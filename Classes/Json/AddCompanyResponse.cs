using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockC_Api.Classes.Json
{
    public class AddCompanyResponse
    {
        [JsonProperty("companyId")]
        public long CompanyId { get; set;}
    }
}