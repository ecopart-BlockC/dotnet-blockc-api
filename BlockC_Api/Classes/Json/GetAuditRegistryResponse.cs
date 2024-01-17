using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockC_Api.Classes.Json
{
    public class GetAuditRegistryResponse
    {
        [JsonProperty("auditId")]
        public string AuditoriaID { get; set; }

        [JsonProperty("registries")]
        public List<Registros> RegistrosList { get; set; }

        public partial class Registros
        {
            [JsonProperty("registryId")]
            public string ID { get; set; }

            [JsonProperty("categoryId")]
            public string CategoriaID { get; set; }

            [JsonProperty("category")]
            public string Categoria { get; set; }

            [JsonProperty("subcategoryId")]
            public string SubCategoriaID { get; set; }

            [JsonProperty("subcategory")]
            public string SubCategoria { get; set; }

            [JsonProperty("tco2e")]
            public Double tco2e { get; set; }

            [JsonProperty("registryStatus")]
            public string RegistroStatus { get; set; }
        }
    }
}