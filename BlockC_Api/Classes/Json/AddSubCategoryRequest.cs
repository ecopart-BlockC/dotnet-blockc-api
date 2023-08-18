using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockC_Api.Classes.Json
{
    public class AddSubCategoryRequest
    {
        [JsonProperty("token")]
        public string Token { get; set; }
        
        [JsonProperty("categoryId")]
        public int CategoryID { get; set; }

        [JsonProperty("subcategoryName")]
        public string SubCategoryName { get; set; }

        [JsonProperty("scope")]
        public int Scope { get; set; }

        [JsonProperty("comments")]
        public string Comments { get; set; }
    }
}