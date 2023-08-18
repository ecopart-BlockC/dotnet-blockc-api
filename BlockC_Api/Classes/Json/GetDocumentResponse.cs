using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockC_Api.Classes.Json
{
    public class GetDocumentResponse
    {
        [JsonProperty("documents")]
        public List<DocumentList> Documentos { get; set; }

        public partial class DocumentList
        {
            [JsonProperty("documentId", Required = Required.Always, NullValueHandling = NullValueHandling.Include)]
            public string DocumentoID { get; set; }

            [JsonProperty("documentName", Required = Required.Always, NullValueHandling = NullValueHandling.Include)]
            public string DocumentoNome { get; set; }

            [JsonProperty("documentType", Required = Required.Always, NullValueHandling = NullValueHandling.Include)]
            public string DocumentoTipo { get; set; }

            [JsonProperty("documentExtension", Required = Required.Always, NullValueHandling = NullValueHandling.Include)]
            public string DocumentoExtensao { get; set; }

            [JsonProperty("contentType", Required = Required.Always, NullValueHandling = NullValueHandling.Include)]
            public string DocumentoContentType { get; set; }

            [JsonProperty("documentSize", Required = Required.Always, NullValueHandling = NullValueHandling.Include)]
            public int DocumentoTamanho { get; set; }

            [JsonProperty("documentImage", Required = Required.Always, NullValueHandling = NullValueHandling.Include)]
            public string DocumentoImagem { get; set; }
        }

    }
}