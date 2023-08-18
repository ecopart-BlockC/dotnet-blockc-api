using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockC_Api.Classes.Json
{
    public class UpdateEmissionRequest
    {
        [JsonRequired]
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonRequired]
        [JsonProperty("emissionId")]
        public string EmissaoID { get; set; }

        [JsonRequired]
        [JsonProperty("categoryId")]
        public int CategoriaID { get; set; }

        [JsonRequired]
        [JsonProperty("month")]
        public int Mes { get; set; }

        [JsonRequired]
        [JsonProperty("year")]
        public int Ano { get; set; }

        [JsonRequired]
        [JsonProperty("factor")]
        public float Fator { get; set; }

        [JsonRequired]
        [JsonProperty("userId")]
        public Int64 UsuarioID { get; set; }

    }
}