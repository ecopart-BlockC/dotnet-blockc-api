using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static BlockC_Api.Classes.Json.GetDocumentResponse;

namespace BlockC_Api.Classes.Json
{
    public class GetAuditCertificateResponse
    {
        [JsonProperty("certificates")]
        public List<Certificates> Certificados { get; set; }

        public partial class Certificates
        {
            [JsonProperty("certificateId", Required = Required.Always, NullValueHandling = NullValueHandling.Include)]
            public string CertificadoID { get; set; }

            [JsonProperty("fileName", Required = Required.Always, NullValueHandling = NullValueHandling.Include)]
            public string ArquivoNome { get; set; }

            [JsonProperty("fileExtension", Required = Required.Always, NullValueHandling = NullValueHandling.Include)]
            public string ArquivoExtensao { get; set; }

            [JsonProperty("fileContentType", Required = Required.Always, NullValueHandling = NullValueHandling.Include)]
            public string ArquivoContentType { get; set; }

            [JsonProperty("fileSize", Required = Required.Always, NullValueHandling = NullValueHandling.Include)]
            public int ArquivoTamanho { get; set; }

            [JsonProperty("fileImage", Required = Required.Always, NullValueHandling = NullValueHandling.Include)]
            public string ArquivoImagem { get; set; }
        }
    }
}