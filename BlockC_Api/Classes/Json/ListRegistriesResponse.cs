using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static BlockC_Api.Classes.Json.RegistryList;

namespace BlockC_Api.Classes.Json
{
    public class ListRegistriesResponse
    {
        [JsonProperty("registries")]
        public List<RegistriesResponseCollection> Registries { get; set; }
    }

    public partial class RegistriesResponseCollection
    {
        [JsonProperty("registryId")]
        public string RegistryID { get; set; }

        [JsonProperty("companyId")]
        public long CompanyID { get; set; }

        [JsonProperty("companyName")]
        public string CompanyName { get; set; }

        [JsonProperty("categoryId")]
        public int CategoryID { get; set; }

        [JsonProperty("categoryName")]
        public string CategoryName { get; set; }

        [JsonProperty("categoryEntryCount")]
        public Int64 CategoryEntryCount { get; set; } 

        [JsonProperty("subcategoryId")]
        public int SubCategoryID { get; set; }

        [JsonProperty("subcategoryName")]
        public string SubCategoryName { get; set; }

        [JsonProperty("unit")]
        public string Unity { get; set; }

        [JsonProperty("value")]
        public decimal UnitValue { get; set; }

        [JsonProperty("status")]
        public string RegistryStatus { get; set; }

        [JsonProperty("registryDate")]
        public DateTime RegistryDate { get; set; }

        [JsonProperty("registeredBy")]
        public string RegisteredBy { get; set; }

        [JsonProperty("referredMonth")]
        public int ReferredMonth { get; set; }

        [JsonProperty("referredYear")]
        public int ReferredYear { get; set; }

        [JsonProperty("sourceId")]
        public string SourceID { get; set; }

        [JsonProperty("sourceName")]
        public string SourceName { get; set; }

        [JsonProperty("fuelName")]
        public string FuelName { get; set; }

        [JsonProperty("customFields", NullValueHandling = NullValueHandling.Ignore)]
        public List<RegistryCustomFields> CustomFields { get; set; }

        [JsonProperty("documents")]
        public List<Documents> DocumentList { get; set; }

        [JsonProperty("comments", NullValueHandling = NullValueHandling.Ignore)]
        public List<RegistryComments> Comments { get; set; }


        public partial class RegistryComments
        {
            [JsonProperty("commentId", NullValueHandling = NullValueHandling.Ignore)]
            public string CommentID { get; set; }

            [JsonProperty("comment", NullValueHandling = NullValueHandling.Ignore)]
            public string Comment { get; set; }

            [JsonProperty("userName", NullValueHandling = NullValueHandling.Ignore)]
            public string UserName { get; set; }

            [JsonProperty("createdAt", NullValueHandling = NullValueHandling.Ignore)]
            public string CreatedAt { get; set; }
        }

        public partial class RegistryCustomFields
        {
            [JsonProperty("fieldName", NullValueHandling = NullValueHandling.Ignore)]
            public string FieldName { get; set; }

            [JsonProperty("fieldValue", NullValueHandling = NullValueHandling.Ignore)]
            public decimal FieldValue { get; set; }
        }

        public partial class Documents
        {
            [JsonProperty("documentId")]
            public string DocumentID { get; set; }

            [JsonProperty("documentName")]
            public string DocumentName { get; set; }

            [JsonProperty("documentType")]
            public string DocumentType { get; set; }

            [JsonProperty("documentContentType")]
            public string DocumentContentType { get; set; }
        }

    }

}