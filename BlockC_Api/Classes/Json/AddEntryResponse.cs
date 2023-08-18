using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockC_Api.Classes.Json
{
    public class AddEntryResponse
    {
        [JsonProperty("registries")]
        public List<RegistryResponseList> RegistryList { get; set; }
    }

    public partial class RegistryResponseList
    {
        [JsonProperty("registryId")]
        public string EntryID { get; set; }

        [JsonProperty("registryInsertStatus")]
        public string EntryStatus { get; set; }

        [JsonProperty("documents")]
        public List<RegistryDocuments> Documents { get; set; }

        public partial class RegistryDocuments
        {
            [JsonProperty("documentName")]
            public string DocumentName { get; set; }

            [JsonProperty("documentId")]
            public string DocumentID { get; set; }

            [JsonProperty("documentInsertStatus")]
            public string InsertStatus { get; set; }
        }
    }


}