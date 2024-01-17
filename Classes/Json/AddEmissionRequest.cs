using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockC_Api.Classes.Json
{
    public class AddEmissionRequest
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("categoryId")]
        public int CategoriaID { get; set; }

        [JsonProperty("month")]
        public int Mes { get; set; }

        [JsonProperty("year")]
        public int Ano { get; set; }

        [JsonProperty("factor")]
        public float Fator { get; set; }

        [JsonProperty("userId")]
        public Int64 UsuarioID { get; set; }
    }
}
