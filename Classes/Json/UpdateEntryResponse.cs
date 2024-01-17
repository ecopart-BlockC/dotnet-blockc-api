using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockC_Api.Classes.Json
{
    public class UpdateEntryResponse
    {
        [JsonProperty("registries")]
        public List<UpdateRegistryResponseList> RegistryList { get; set; }

        public partial class UpdateRegistryResponseList
        {
            [JsonProperty("registryId")]
            public string EntryID { get; set; }

            [JsonProperty("registryUpdateStatus")]
            public string EntryStatus { get; set; }

            [JsonProperty("documents", NullValueHandling = NullValueHandling.Ignore)]
            public List<RegistryDocuments> Documents { get; set; }
        }

        public partial class RegistryDocuments
        {
            [JsonProperty("documentId", NullValueHandling = NullValueHandling.Ignore)]
            public string DocumentID { get; set; }

            [JsonProperty("documentName", NullValueHandling = NullValueHandling.Ignore)]
            public string DocumentName { get; set; }

            [JsonProperty("documentUpdateStatus")]
            public string DocumentStatus { get; set; }

        }
    }



}