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
    public class ApproveMasterUserController : ApiController
    {

        public async Task<HttpResponseMessage> Post([System.Web.Http.FromBody] JObject _request)
        {
            HttpResponseMessage response;

            Classes.Json.GenericResponse genericResponse = new Classes.Json.GenericResponse();
            string jsonResponse = string.Empty;

            try
            {
                Classes.Json.ApproveMasterUserRequest approveRequest = new Classes.Json.ApproveMasterUserRequest();
                approveRequest = JsonConvert.DeserializeObject<Classes.Json.ApproveMasterUserRequest>(_request.ToString());

                if (approveRequest == null)
                {
                    genericResponse.mensagem = "Conteúdo da requisição inválido";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                if (string.IsNullOrEmpty(approveRequest.Token) || string.IsNullOrEmpty(approveRequest.Email))
                {
                    genericResponse.mensagem = "Necessário informar todos os campos";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                Classes.Database database = new Classes.Database();
                if (!database.BuscarTokenExistente(approveRequest.Token))
                {
                    genericResponse.mensagem = "Token inválido";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                if (!database.AtualizarUsuarioAprovacao(approveRequest.Email))
                {
                    genericResponse.mensagem = "Não foi possível aprovar o usuário";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                try
                {
                    Classes.Mail mail = new Classes.Mail();
                    string mensagem = "Seu cadastro foi aprovado e o acesso à plataforma liberada." + Environment.NewLine;
                    mensagem += Environment.NewLine;

                    System.Threading.Thread mailThread;
                    mailThread = new System.Threading.Thread(delegate ()
                    {
                        mail.EnviarEmail(approveRequest.Email, "", mensagem, "Cadastro aprovado");
                    });

                    mailThread.IsBackground = true;
                    mailThread.Start();
                }
                catch(Exception ex)
                {
                    Classes.Database.RegistrarErro("Server API", "ApproveMasterUserController - EnvioMail", "Post", ex.Message, "");
                }

                genericResponse.mensagem = "OK";
                jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                response = Request.CreateResponse(System.Net.HttpStatusCode.OK);
                response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                Classes.Database.RegistrarErro("Server API", "ApproveMasterUserController", "Post", ex.Message, _request.ToString());

                genericResponse.mensagem = "Não foi possível atender a solicitação";
                jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                response = Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
                response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
            }

            return response;
        }

    }
}