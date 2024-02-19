using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockC_Api.Classes.Json
{
    public class AddCertificadoEnergiaRenovavelRequest
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("companyRegistryNumber")]
        public string CompanyRegistryNumber { get; set; }

        [JsonProperty("companyName")]
        public string CompanyName { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("certificadoTipo")]
        public int CertificadoTipo { get; set; }

        [JsonProperty("anoReferencia")]
        public int AnoReferencia { get; set; }

        [JsonProperty("statusRegistro")]
        public string StatusRegistro { get; set; }
    }
}