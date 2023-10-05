using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace BlockC_Api.Classes.Json
{
    public class AddEntryRequest
    {
        [JsonRequired]
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonRequired]
        [JsonProperty("registries")]
        public List<RegistryList> RegistryList { get; set; }
    }

    public partial class RegistryList
    {
        [JsonProperty("companyId", NullValueHandling = NullValueHandling.Ignore)]
        public long CompanyID { get; set; }

        [JsonProperty("categoryId", NullValueHandling = NullValueHandling.Ignore)]
        public int CategoryID { get; set; }

        [JsonProperty("subcategoryId", NullValueHandling = NullValueHandling.Ignore)]
        public int SubCategoryID { get; set; }

        [JsonProperty("sourceId", NullValueHandling = NullValueHandling.Ignore)]
        public int SourceID { get; set; }

        [JsonProperty("gasId", NullValueHandling = NullValueHandling.Ignore)]
        public string GasID { get; set; }

        [JsonProperty("unit", NullValueHandling = NullValueHandling.Ignore)]
        public string Unit { get; set; }

        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public decimal EntryValue { get; set; }

        [JsonRequired]
        [JsonProperty("createdById")]
        public long CreatedByID { get; set; }

        [JsonProperty("refferedYear", NullValueHandling = NullValueHandling.Ignore)]
        public int ReferenceYear { get; set; }

        [JsonProperty("refferedMonth", NullValueHandling = NullValueHandling.Ignore)]
        public int ReferenceMonth { get; set; }

        [JsonProperty("documents", NullValueHandling = NullValueHandling.Ignore)]
        public List<RegistryDocuments> Documents { get; set; }

        [JsonProperty("comments", NullValueHandling = NullValueHandling.Ignore)]
        public List<RegistryComments> Comments { get; set; }

        [JsonProperty("customFields", NullValueHandling = NullValueHandling.Ignore)]
        public List<RegistryCustomFields> CustomFields { get; set; }

        public partial class RegistryCustomFields
        {
            [JsonProperty("fieldName", NullValueHandling = NullValueHandling.Ignore)]
            public string FieldName { get; set; }

            [JsonProperty("fieldValue", NullValueHandling = NullValueHandling.Ignore)]
            public decimal FieldValue { get; set; }

            [JsonProperty("fieldLabel", NullValueHandling = NullValueHandling.Ignore)]
            public string FieldLabel { get; set; }
        }

        public partial class RegistryDocuments
        {
            [JsonProperty("documentId", NullValueHandling = NullValueHandling.Ignore)]
            public string DocumentId { get; set; }

            [JsonProperty("documentType", NullValueHandling = NullValueHandling.Ignore)]
            public string DocumentType { get; set; }

            [JsonProperty("documentImage", NullValueHandling = NullValueHandling.Ignore)]
            public string DocumentImage { get; set; }

            [JsonProperty("documentName", NullValueHandling = NullValueHandling.Ignore)]
            public string DocumentName { get; set; }

            [JsonProperty("documentContentType", NullValueHandling = NullValueHandling.Ignore)]
            public string DocumentContentType { get; set; }
        }

    }

    public partial class RegistryComments
    {
        [JsonProperty("comment", NullValueHandling = NullValueHandling.Ignore)]
        public string Comment { get; set; }

        [JsonProperty("userName", NullValueHandling = NullValueHandling.Ignore)]
        public string UserName { get; set; }

        [JsonProperty("createdAt", NullValueHandling = NullValueHandling.Ignore)]
        public string CreatedAt { get; set; }
    }

}