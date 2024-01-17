using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockC_Api.Classes.Json
{
    public class ChangePasswordRequest
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("email")]
        public string email { get; set; }

        [JsonProperty("password")]
        public string senha { get; set; }

        [JsonProperty("newPassword")]
        public string novaSenha { get; set; }

    }
}