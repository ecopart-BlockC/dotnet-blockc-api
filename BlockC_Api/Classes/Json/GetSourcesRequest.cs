using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockC_Api.Classes.Json
{
    public class GetSourcesRequest
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("scope", NullValueHandling = NullValueHandling.Ignore)]
        public int Escopo { get; set; }

        [JsonProperty("categoryId")]
        public int CategoriaID { get; set; }

        [JsonProperty("subcategoryId")]
        public int SubCategoriaID { get; set; }

        [JsonProperty("companyId")]
        public int EmpresaID { get; set; }

    }
}