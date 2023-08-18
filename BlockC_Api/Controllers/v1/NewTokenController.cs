using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Http.Cors;

namespace BlockC_Api.Controllers.v1
{
    [EnableCors(origins:"*", headers:"*", methods: "*")]
    public class NewTokenController : ApiController
    {
        public async Task<HttpResponseMessage> Get([System.Web.Http.FromBody] JObject _newTokenRequest)
        {
            HttpResponseMessage response;

            Classes.Json.GenericResponse genericResponse = new Classes.Json.GenericResponse();
            string jsonResponse = string.Empty;

            try
            {
                Classes.Json.NewTokenRequest newTokenRequest = new Classes.Json.NewTokenRequest();
                newTokenRequest = JsonConvert.DeserializeObject<Classes.Json.NewTokenRequest>(_newTokenRequest.ToString());

                if (newTokenRequest == null)
                {
                    genericResponse.mensagem = "Conteúdo da requisição inválido";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();

                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                if (string.IsNullOrEmpty(newTokenRequest.apiKey))
                {
                    genericResponse.mensagem = "Chave da API inexistente";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();

                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                if (!Classes.Database.ValidarApiKey(newTokenRequest.apiKey))
                {
                    genericResponse.mensagem = "Chave da API inválida";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();

                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                Classes.Database database = new Classes.Database();
                string newToken = Classes.Utilitario.RandomString(235);

                while (database.BuscarTokenExistente(newToken))
                {
                    newToken = Classes.Utilitario.RandomString(235);
                }

                database.GravarToken(newToken);

                Classes.Json.NewTokenResponse tokenResponse = new Classes.Json.NewTokenResponse();
                tokenResponse.Token = newToken;

                jsonResponse = JsonConvert.SerializeObject(tokenResponse).ToString();
                response = Request.CreateResponse(System.Net.HttpStatusCode.OK);
                response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                Classes.Database.RegistrarErro("Server API", "NewTokenController", "Get", ex.Message, _newTokenRequest.ToString());

                genericResponse.mensagem = "Não foi possível atender a solicitação";
                jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                response = Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
                response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
            }

            return response;
        }

    }
}