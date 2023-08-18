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
    public class AddUserController : ApiController
    {

        public async Task<HttpResponseMessage> Post([System.Web.Http.FromBody] JObject _addUserRequest)
        {
            HttpResponseMessage response;

            Classes.Json.GenericResponse genericResponse = new Classes.Json.GenericResponse();
            string jsonResponse = string.Empty;

            try
            {
                Classes.Json.AddUserRequest addUserRequest = new Classes.Json.AddUserRequest();
                addUserRequest = JsonConvert.DeserializeObject<Classes.Json.AddUserRequest>(_addUserRequest.ToString());

                if (addUserRequest == null)
                {
                    genericResponse.mensagem = "Conteúdo da requisição inválido";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();

                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                if (string.IsNullOrEmpty(addUserRequest.Token))
                {
                    genericResponse.mensagem = "Token inexistente";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();

                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                if (string.IsNullOrEmpty(addUserRequest.Tipo) 
                    || string.IsNullOrEmpty(addUserRequest.Token) 
                    || string.IsNullOrEmpty(addUserRequest.Senha) 
                    || string.IsNullOrEmpty(addUserRequest.Nome)
                    || string.IsNullOrEmpty(addUserRequest.Sobrenome)
                    || string.IsNullOrEmpty(addUserRequest.Email))
                {
                    genericResponse.mensagem = "Necessário informar todos os campos";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();

                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                Classes.Database database = new Classes.Database();
                if (!database.BuscarTokenExistente(addUserRequest.Token))
                {
                    genericResponse.mensagem = "Token inválido";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();

                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                if (database.ValidarEmail(addUserRequest.Email))
                {
                    genericResponse.mensagem = "E-mail informado já existe";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();

                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                if (addUserRequest.Tipo.ToUpper() != "MASTER" && addUserRequest.Tipo.ToUpper() != "USER" && addUserRequest.Tipo.ToUpper() != "REGISTER" && addUserRequest.Tipo.ToUpper() != "AUDITOR")
                {
                    genericResponse.mensagem = "O tipo de usuário é inválido. As opções disponíveis são: Master, User, Register e Auditor";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                long usuarioID = database.GravarUsuario(addUserRequest.Nome, addUserRequest.Sobrenome, addUserRequest.Tipo, addUserRequest.Email, addUserRequest.Senha);
                if (usuarioID == 0)
                {
                    genericResponse.mensagem = "Não foi possível realizar o cadastro";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();

                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                if (addUserRequest.Tipo.ToUpper() == "MASTER")
                {
                    Classes.Mail mail = new Classes.Mail();
                    string mensagem = "Um novo usuário Master foi criado e necessita de aprovação." + Environment.NewLine;
                    mensagem += "Nome: " + addUserRequest.Nome + Environment.NewLine;
                    mensagem += "Sobrenome: " + addUserRequest.Sobrenome + Environment.NewLine;
                    mensagem += "E-mail: " + addUserRequest.Email + Environment.NewLine;
                    mensagem += Environment.NewLine;

                    System.Threading.Thread mailThread;
                    mailThread = new System.Threading.Thread(delegate ()
                    {
                        mail.EnviarEmail("jose.dantas@blockc.com.br", "", mensagem, "Novo cadastro pendente de aprovação");
                    });

                    mailThread.IsBackground = true;
                    mailThread.Start();

                    mensagem = "Seu cadastro foi realizado e submetido à aprovação." + Environment.NewLine;
                    mensagem += "Em até 2 dias úteis você receberá um aviso, em seu e-mail, sobre seu cadastro." + Environment.NewLine;
                    mensagem += "" + Environment.NewLine;
                    mensagem += "Obrigado" + Environment.NewLine;

                    mailThread = new System.Threading.Thread(delegate ()
                    {
                        mail.EnviarEmail(addUserRequest.Email, "", mensagem, "Seu cadastro foi submetido à aprovação");
                    });

                    mailThread.IsBackground = true;
                    mailThread.Start();
                }

                Classes.Json.AddUserResponse userResponse = new Classes.Json.AddUserResponse();
                userResponse.UsuarioID = usuarioID;
                userResponse.Nome = addUserRequest.Nome;
                userResponse.Sobrenome = addUserRequest.Sobrenome;
                userResponse.Tipo = addUserRequest.Tipo;
                userResponse.Email = addUserRequest.Email;

                jsonResponse = JsonConvert.SerializeObject(userResponse).ToString();
                response = Request.CreateResponse(System.Net.HttpStatusCode.OK);
                response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                Classes.Database.RegistrarErro("Server API", "AddUserController", "Post", ex.Message, _addUserRequest.ToString());

                genericResponse.mensagem = "Não foi possível atender a solicitação";
                jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                response = Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
                response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
            }

            return response;
        }

    }
}