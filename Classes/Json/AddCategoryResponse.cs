using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockC_Api.Classes.Json
{
    public class AddCategoryResponse
    {
        [JsonProperty("categoryId")]
        public int CategoryId { get; set; }
    }
}