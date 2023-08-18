using Newtonsoft.Json;

namespace BlockC_Api.Classes.Json
{
    public class GenericResponse
    {
        [JsonProperty("mensagem")]
        public string mensagem { get; set; }
    }
}
