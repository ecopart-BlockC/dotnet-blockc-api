using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockC_Api.Classes.Json
{
    public class GetGasesResponse
    {
        [JsonProperty("gases")]
        public List<GasesList> Gases { get; set; }

        public partial class GasesList
        {
            [JsonProperty("gasId")]
            public string GasID { get; set; }

            [JsonProperty("gas")]
            public string Gas { get; set; }

            [JsonProperty("gwp")]
            public string GWP { get; set; }
        }

    }
}