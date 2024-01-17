using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockC_Api.Classes.Json
{
    public class UpdateEntryRequest
    {
        [JsonRequired]
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonRequired]
        [JsonProperty("registries")]
        public List<UpdateRegistryList> RegistryList { get; set; }
    }

    public partial class UpdateRegistryList
    {
        [JsonRequired]
        [JsonProperty("registryId")]
        public string RegistryID { get; set; }

        [JsonRequired]
        [JsonProperty("companyId")]
        public long CompanyID { get; set; }

        [JsonRequired]
        [JsonProperty("categoryId")]
        public int CategoryID { get; set; }

        [JsonRequired]
        [JsonProperty("subcategoryId")]
        public int SubCategoryID { get; set; }

        [JsonProperty("sourceId", NullValueHandling = NullValueHandling.Ignore)]
        public int SourceID { get; set; }

        [JsonProperty("gasId", NullValueHandling = NullValueHandling.Ignore)]
        public string GasID { get; set; }

        [JsonProperty("unit")]
        public string Unit { get; set; }

        [JsonProperty("value")]
        public decimal EntryValue { get; set; }

        [JsonProperty("createdById")]
        public long CreatedByID { get; set; }

        [JsonProperty("registryStatus", NullValueHandling = NullValueHandling.Ignore)]
        public string RegistryStatus { get; set; }

        [JsonProperty("documents", NullValueHandling = NullValueHandling.Ignore)]
        public List<RegistryDocuments> Documents { get; set; }

        [JsonProperty("deletedDocuments", NullValueHandling = NullValueHandling.Ignore)]
        public List<RegistryDeletedDocuments> DeletedDocuments { get; set; }

        [JsonProperty("comments", NullValueHandling = NullValueHandling.Ignore)]
        public List<RegistryComments> Comments { get; set; }

        public partial class RegistryComments
        {
            [JsonProperty("comment", NullValueHandling = NullValueHandling.Ignore)]
            public string Comment { get; set; }

            [JsonProperty("userName", NullValueHandling = NullValueHandling.Ignore)]
            public string UserName { get; set; }

            [JsonProperty("createdAt", NullValueHandling = NullValueHandling.Ignore)]
            public string CreatedAt { get; set; }
        }

        public partial class RegistryDeletedDocuments
        {
            [JsonProperty("documentId", NullValueHandling = NullValueHandling.Ignore)]
            public string DocumentID { get; set; }
        }

        public partial class RegistryDocuments
        {
            [JsonProperty("documentId", NullValueHandling = NullValueHandling.Ignore)]
            public string DocumentID{ get; set; }

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


}