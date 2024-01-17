using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockC_Api.Classes.Json
{
    public class AddSubCategoryResponse
    {
        [JsonProperty("subcategoryId")]
        public int SubCategoryId { get; set; }
    }
}