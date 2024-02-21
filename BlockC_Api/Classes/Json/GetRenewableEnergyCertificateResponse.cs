using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace BlockC_Api.Classes.Json
{
    public class GetRenewableEnergyCertificateResponse
    {
        [JsonProperty("renewableEnergyCertificateID")]
        public string RenewableEnergyCertificateID { get; set; }

        [JsonProperty("certificates")]
        public List<Certificates> CertificatesList { get; set; }

        public partial class Certificates
        {
            [JsonProperty("companyRegistryNumber")]
            public string CompanyRegistryNumber { get; set; }

            [JsonProperty("companyName")]
            public string CompanyName { get; set; }

            [JsonProperty("companyId")]
            public string CompanyId { get; set; }

            [JsonProperty("razaoSocial")]
            public string RazaoSocial { get; set; }

            [JsonProperty("hash")]
            public string Hash { get; set; }

            [JsonProperty("certificadoId")]
            public string CertificadoId { get; set; }

            [JsonProperty("certificadoTipo")]
            public string CertificadoTipo { get; set; }

            [JsonProperty("referredYear")]
            public string ReferredYear { get; set; }

            [JsonProperty("statusRegistro")]
            public string StatusRegistro { get; set; }

            [JsonProperty("dataRegistro")]
            public string DataRegistro { get; set; }
        }
    }

}