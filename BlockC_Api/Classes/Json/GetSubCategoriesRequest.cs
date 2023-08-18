using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockC_Api.Classes.Json
{
    public class GetSubCategoriesRequest
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("scope", NullValueHandling = NullValueHandling.Ignore)]
        public int Escopo { get; set; }

        [JsonProperty("categoryId")]
        public int CategoriaId { get; set; }
    }
}