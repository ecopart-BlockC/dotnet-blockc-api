using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockC_Api.Classes.Json
{
    public class GetTotalRegisterResponse
    {
        [JsonProperty("categories")]
        public List<CategoryTotal> categoryTotals {  get; set; }


        public partial class CategoryTotal
        {
            [JsonProperty("categoryId")]
            public long CategoryID { get; set; }

            [JsonProperty("category")]
            public string Category { get; set; }

            [JsonProperty("total")]
            public long Total { get; set; }
        }
    }
}