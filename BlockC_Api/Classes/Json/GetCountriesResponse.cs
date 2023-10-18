using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockC_Api.Classes.Json
{
    public class GetCountriesResponse
    {
        [JsonProperty("countries")]
        public List<Paises> PaisLista { get; set; }


        public partial class Paises
        {
            [JsonProperty("countryId")]
            public string PaisID { get; set; }

            [JsonProperty("country")]
            public string Pais { get; set; }

            [JsonProperty("currency")]
            public string Moeda { get; set; }
        }
        
        
    }
}