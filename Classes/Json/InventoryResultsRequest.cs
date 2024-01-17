using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockC_Api.Classes.Json
{
    public class InventoryResultsRequest
    {
        [JsonRequired]
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonRequired]
        [JsonProperty("userId")]
        public string UsuarioID { get; set; }

        [JsonRequired]
        [JsonProperty("filters")]
        public List<Filters> Filtros { get; set; }

        public partial class Filters
        {
            [JsonRequired]
            [JsonProperty("companyId")]
            public string EmpresaID { get; set; }

            [JsonRequired]
            [JsonProperty("referenceYear")]
            public string AnoReferencia { get; set; }

            [JsonRequired]
            [JsonProperty("referenceMonth")]
            public string MesReferencia { get; set; }
        }


    }
}