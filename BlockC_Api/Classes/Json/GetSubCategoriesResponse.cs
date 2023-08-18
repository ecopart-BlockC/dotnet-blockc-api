using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockC_Api.Classes.Json
{
    public class GetSubCategoriesResponse
    {
        [JsonProperty("subcategories")]
        public List<SubCategories> subcategoryList { get; set; }
    }

    public partial class SubCategories
    {
        [JsonProperty("subcategoryId")]
        public int SubCategoryId { get; set;}

        [JsonProperty("categoryId")]
        public int CategoryId { get; set;}

        [JsonProperty("subcategoryName")]
        public string SubCategoryName { get; set;}

        [JsonProperty("scope")]
        public int Scope { get; set; }

        [JsonProperty("comments")]
        public string Comments { get; set;}
    }

}