using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockC_Api.Classes.Json
{
    public class AddUserRequest
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("name")]
        public string Nome { get; set; }

        [JsonProperty("lastName")]
        public string Sobrenome { get; set; }

        [JsonProperty("accountType")]
        public string Tipo { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Senha { get; set; }

    }
}