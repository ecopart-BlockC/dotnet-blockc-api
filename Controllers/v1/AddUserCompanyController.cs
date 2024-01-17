using Asp.Versioning;
using BlockC_Api.Classes.Json;
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
    [ApiVersion("1.0")]
    [System.Web.Http.Route("api/v{version:apiVersion}/AddUserCompany")]
    public class AddUserCompanyController : ApiController
    {
        public async Task<HttpResponseMessage> Post([System.Web.Http.FromBody] JObject _addUserCompanyRequest)
        {
            HttpResponseMessage response;

            Classes.Json.GenericResponse genericResponse = new Classes.Json.GenericResponse();
            string jsonResponse = string.Empty;

            try
            {
                Classes.Json.AddUserCompanyRequest userCompanyRequest = new Classes.Json.AddUserCompanyRequest();
                userCompanyRequest = JsonConvert.DeserializeObject<Classes.Json.AddUserCompanyRequest>(_addUserCompanyRequest.ToString());

                if (userCompanyRequest == null)
                {
                    genericResponse.mensagem = "Conteúdo da requisição inválido";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();

                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                if (userCompanyRequest.Empresas == null)
                {
                    genericResponse.mensagem = "Conteúdo da requisição inválido";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();

                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                if (string.IsNullOrEmpty(userCompanyRequest.Token))
                {
                    genericResponse.mensagem = "Token inexistente";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();

                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                if (userCompanyRequest.UsuarioID <= 0)
                {
                    genericResponse.mensagem = "Necessário informar todos os campos";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();

                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                Classes.Database database = new Classes.Database();
                if (!database.BuscarTokenExistente(userCompanyRequest.Token))
                {
                    genericResponse.mensagem = "Token inválido";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();

                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                foreach(UserCompaniesList empresa in userCompanyRequest.Empresas)
                {
                    if (!database.Gravar_EmpresaUsuario(empresa.EmpresaID, userCompanyRequest.UsuarioID))
                    {
                        genericResponse.mensagem = "Não foi possível realizar o cadastro";
                        jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();

                        response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                        response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                        return response;
                    }
                }

                genericResponse.mensagem = "OK";
                jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();

                response = Request.CreateResponse(System.Net.HttpStatusCode.OK);
                response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                Classes.Database.RegistrarErro("Server API", "AddUserCompanyController", "Post", ex.Message, _addUserCompanyRequest.ToString());

                genericResponse.mensagem = "Não foi possível atender a solicitação";
                jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                response = Request.CreateResponse(System.Net.HttpStatusCode.NoContent);
                response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
            }

            return response;
        }

    }
}