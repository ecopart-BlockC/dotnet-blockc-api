using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace BlockC_Api.Controllers.v1
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UpdateEmissionController : ApiController
    {
        public async Task<HttpResponseMessage> Post([System.Web.Http.FromBody] JObject _request)
        {
            HttpResponseMessage response;

            Classes.Json.GenericResponse genericResponse = new Classes.Json.GenericResponse();
            string jsonResponse = string.Empty;

            try
            {
                Classes.Json.UpdateEmissionRequest emissionRequest = new Classes.Json.UpdateEmissionRequest();
                emissionRequest = JsonConvert.DeserializeObject<Classes.Json.UpdateEmissionRequest>(_request.ToString());

                if (emissionRequest == null)
                {
                    genericResponse.mensagem = "Conteúdo da requisição inválido";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                if (string.IsNullOrEmpty(emissionRequest.Token))
                {
                    genericResponse.mensagem = "Token inexistente";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                if (string.IsNullOrEmpty(emissionRequest.EmissaoID) || emissionRequest.CategoriaID <= 0 || emissionRequest.UsuarioID <= 0 
                    || emissionRequest.Mes <= 0 || emissionRequest.Ano <= 0 || emissionRequest.Fator <= 0)
                {
                    genericResponse.mensagem = "Necessário informar os campos emissionId, categoryId, month, year, factor e userId";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                Classes.Database database = new Classes.Database();
                if (!database.BuscarTokenExistente(emissionRequest.Token))
                {
                    genericResponse.mensagem = "Token inválido";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                if (!database.VerificarFatorExistenteID(emissionRequest.EmissaoID))
                {
                    genericResponse.mensagem = "Fator de Emissão não existente";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                if (!database.AtualizarFatorEmissao(emissionRequest.EmissaoID, emissionRequest.CategoriaID, emissionRequest.Ano
                    , emissionRequest.Mes, emissionRequest.Fator, emissionRequest.UsuarioID))
                {
                    genericResponse.mensagem = "Não foi possível atualizar o fator de emissão";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                    response = Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                Classes.Json.GenericResponse emissionResponse = new Classes.Json.GenericResponse();
                emissionResponse.mensagem = "OK";

                jsonResponse = JsonConvert.SerializeObject(emissionResponse).ToString();
                response = Request.CreateResponse(System.Net.HttpStatusCode.OK);
                response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                Classes.Database.RegistrarErro("Server API", "UpdateEmissionController", "Post", ex.Message, _request.ToString());
                genericResponse.mensagem = "Não foi possível atender a solicitação";
                jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                response = Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
                response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
            }

            return response;
        }
    }
}