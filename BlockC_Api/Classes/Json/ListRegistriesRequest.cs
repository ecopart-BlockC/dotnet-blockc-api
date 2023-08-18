using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockC_Api.Classes.Json
{
    public class ListRegistriesRequest
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("userId")]
        public long UserId { get; set; }

        //[JsonProperty("companies")]
        //public List<ListRegistriesCompanies> companies { get; set; }

        [JsonProperty("registries")]
        public List<ListRegistriesCollection> Registries { get; set; }

        [JsonProperty("pagination")]
        public RegistriesPagination Pagination { get; set; }

        [JsonProperty("sort")]
        public List<ListRegistriesSort> Sort { get; set; }
    }

    public partial class ListRegistriesSort
    {
        [JsonProperty("field")]
        public string Field { get; set; }

        [JsonProperty("direction")]
        public string Direction { get; set; }
    }

    public partial class RegistriesPagination
    {
        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("pageItems")]
        public int PageItems { get; set; }
    }

    public partial class ListRegistriesCollection
    {
        [JsonProperty("companyId")]
        public long CompanyId { get; set; }

        [JsonProperty("categoryId")]
        public int CategoryId { get; set;}

        [JsonProperty("subcategoryId")]
        public int SubCategoryId { get; set; }

        [JsonProperty("unit")]
        public string Unit { get; set; }

        //[JsonProperty("monthYear")]
        //public string MonthYear { get; set; }

        [JsonProperty("referredMonth")]
        public int ReferredMonth { get; set; }

        [JsonProperty("referredYear")]
        public int ReferredYear { get; set; }

        [JsonProperty("status")]
        public string RegistryStatus { get; set; }

        [JsonProperty("referredDocument")]
        public string DocumentName { get; set; }
    }

    //public partial class ListRegistriesCompanies
    //{
    //    [JsonProperty("companyId")]
    //    public long CompanyId { get; set; }
    //}

}