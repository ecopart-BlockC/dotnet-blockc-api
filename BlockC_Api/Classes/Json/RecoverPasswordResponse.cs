using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockC_Api.Classes.Json
{
    public class RecoverPasswordResponse
    {
        [JsonProperty("recoveryKey")]
        public string RecoverKey { get; set; }
    }
}