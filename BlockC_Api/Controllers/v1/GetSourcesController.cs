using Asp.Versioning;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace BlockC_Api.Controllers.v1
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [ApiVersion("1.0")]
    [System.Web.Http.Route("api/v{version:apiVersion}/GetSources")]
    public class GetSourcesController : ApiController
    {
        public async Task<HttpResponseMessage> Get([System.Web.Http.FromBody] JObject _request)
        {
            HttpResponseMessage response;

            Classes.Json.GenericResponse genericResponse = new Classes.Json.GenericResponse();
            string jsonResponse = string.Empty;

            try
            {
                Classes.Json.GetSourcesRequest sourceRequest = new Classes.Json.GetSourcesRequest();
                sourceRequest = JsonConvert.DeserializeObject<Classes.Json.GetSourcesRequest>(_request.ToString());

                if (sourceRequest == null)
                {
                    genericResponse.mensagem = "Conteúdo da requisição inválido";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                if (string.IsNullOrEmpty(sourceRequest.Token))
                {
                    genericResponse.mensagem = "Token inválido";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                    response = Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                if (sourceRequest.CategoriaID <= 0 || sourceRequest.SubCategoriaID <= 0 || sourceRequest.EmpresaID <= 0)
                {
                    genericResponse.mensagem = "Necessário informar todos os parâmetros";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                    response = Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                Classes.Database database = new Classes.Database();
                if (!database.BuscarTokenExistente(sourceRequest.Token))
                {
                    genericResponse.mensagem = "Token inválido";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                    response = Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                int escopo = 0;
                if (sourceRequest.Escopo > 0)
                    escopo = sourceRequest.Escopo;

                string tipoDado = string.Empty;
                if (sourceRequest.TipoDado != null)
                    tipoDado = sourceRequest.TipoDado;

                string tipoProcesso = string.Empty;
                if (sourceRequest.TipoProcesso != null)
                    tipoProcesso = sourceRequest.TipoProcesso;

                string paisID = string.Empty;
                if (sourceRequest.PaisID != null)
                    paisID = sourceRequest.PaisID;

                Classes.Json.GetSourcesResponse sourcesResponse = new Classes.Json.GetSourcesResponse();
                if (!database.BuscarFontes(ref sourcesResponse, escopo, sourceRequest.CategoriaID, sourceRequest.SubCategoriaID, sourceRequest.EmpresaID, tipoDado, tipoProcesso, paisID))
                {
                    genericResponse.mensagem = "Não conseguimos buscar as Fontes cadastradas";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                database.BuscarTipoTransporte(ref sourcesResponse, escopo, sourceRequest.CategoriaID, sourceRequest.SubCategoriaID, sourceRequest.EmpresaID);

                jsonResponse = JsonConvert.SerializeObject(sourcesResponse).ToString();
                response = Request.CreateResponse(System.Net.HttpStatusCode.OK);
                response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                Classes.Database.RegistrarErro("Server API", "GetSourcesController", "GET", ex.Message, _request.ToString());

                genericResponse.mensagem = "Não foi possível atender a solicitação";
                jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                response = Request.CreateResponse(System.Net.HttpStatusCode.NoContent);
                response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
            }

            return response;
        }

    }
}