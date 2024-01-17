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
    [System.Web.Http.Route("api/v{version:apiVersion}/ChangeUserCompany")]
    public class ChangeUserCompanyController : ApiController
    {
        public async Task<HttpResponseMessage> Post([System.Web.Http.FromBody] JObject _changeRequest)
        {
            HttpResponseMessage response;

            Classes.Json.GenericResponse genericResponse = new Classes.Json.GenericResponse();
            string jsonResponse = string.Empty;

            try
            {
                Classes.Json.ChangeUserCompanyRequest changeUserCompanyRequest = new Classes.Json.ChangeUserCompanyRequest();
                changeUserCompanyRequest = JsonConvert.DeserializeObject<Classes.Json.ChangeUserCompanyRequest>(_changeRequest.ToString());

                if (changeUserCompanyRequest == null)
                {
                    genericResponse.mensagem = "Conteúdo da requisição inválido";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();

                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                if (string.IsNullOrEmpty(changeUserCompanyRequest.Token))
                {
                    genericResponse.mensagem = "Token inexistente";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();

                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                if (string.IsNullOrEmpty(changeUserCompanyRequest.Token)
                    || changeUserCompanyRequest.UsuarioID == 0
                    || changeUserCompanyRequest.Empresas.Count == 0)
                {
                    genericResponse.mensagem = "Necessário informar todos os campos";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();

                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                Classes.Database database = new Classes.Database();
                if (!database.BuscarTokenExistente(changeUserCompanyRequest.Token))
                {
                    genericResponse.mensagem = "Token inválido";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();

                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                Boolean falha = true;

                foreach(Classes.Json.Empresas empresa in changeUserCompanyRequest.Empresas)
                {
                    database.Desativar_EmpresaUsuario(empresa.EmpresaID, changeUserCompanyRequest.UsuarioID);

                    if (!database.Gravar_EmpresaUsuario(empresa.EmpresaID, changeUserCompanyRequest.UsuarioID))
                    {
                        falha = true;
                        break;
                    }
                }

                if (!falha)
                {
                    genericResponse.mensagem = "Não foi possível processar um ou mais itens";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();

                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                genericResponse.mensagem = "OK";
                jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();

                response = Request.CreateResponse(System.Net.HttpStatusCode.OK);
                response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                Classes.Database.RegistrarErro("Server API", "ChangeUserCompanyController", "Post", ex.Message, _changeRequest.ToString());

                genericResponse.mensagem = "Não foi possível atender a solicitação";
                jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                response = Request.CreateResponse(System.Net.HttpStatusCode.NoContent);
                response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
            }

            return response;
        }

    }
}