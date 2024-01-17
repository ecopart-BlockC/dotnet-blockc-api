using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockC_Api.Classes.Json
{
    public class GetCategoriesResponse
    {
        [JsonProperty("categories")]
        public List<Categories> categoryList { get; set; }
    }

    public partial class Categories
    {
        [JsonProperty("categoryId")]
        public int categoryId { get; set; }

        [JsonProperty("categoryName")]
        public string categoryName { get; set; }

        [JsonProperty("categoryMode")]
        public string categoryMode { get; set; }

    }


}