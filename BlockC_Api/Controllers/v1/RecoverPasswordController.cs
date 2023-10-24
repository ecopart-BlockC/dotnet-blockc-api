using Asp.Versioning;
using BlockC_Api.Classes.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
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
    [System.Web.Http.Route("api/v{version:apiVersion}/RecoverPassword")]
    public class RecoverPasswordController : ApiController
    {
        public async Task<HttpResponseMessage> Post([System.Web.Http.FromBody] JObject _recoverRequest)
        {
            HttpResponseMessage response;

            Classes.Json.GenericResponse genericResponse = new Classes.Json.GenericResponse();
            string jsonResponse = string.Empty;


            try
            {
                Classes.Json.RecoverPasswordRequest recoverRequest = new Classes.Json.RecoverPasswordRequest();
                recoverRequest = JsonConvert.DeserializeObject<Classes.Json.RecoverPasswordRequest>(_recoverRequest.ToString());

                if (recoverRequest == null)
                {
                    genericResponse.mensagem = "Conteúdo da requisição inválido";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                if (string.IsNullOrEmpty(recoverRequest.Token))
                {
                    genericResponse.mensagem = "Token inexistente";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                if (string.IsNullOrEmpty(recoverRequest.Token) || string.IsNullOrEmpty(recoverRequest.Email))
                {
                    genericResponse.mensagem = "Necessário informar todos os campos";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                Classes.Database database = new Classes.Database();
                if (!database.BuscarTokenExistente(recoverRequest.Token))
                {
                    genericResponse.mensagem = "Token inválido";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                if (!database.ValidarEmail(recoverRequest.Email))
                {
                    genericResponse.mensagem = "E-mail informado é inválido";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                string chaveTemp = Classes.Utilitario.RandomString(15);
                if (!database.AtualizarUsuarioSenhaPendente(recoverRequest.Email, chaveTemp))
                {
                    genericResponse.mensagem = "Não foi possível iniciar o processo de recuperação de senha";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                string nomeUsuario = database.BuscarNomeUsuario(recoverRequest.Email);

                //ENVIAR A CHAVE TEMP POR EMAIL PARA O USUARIO
                Classes.Mail mail = new Classes.Mail();
                string mensagem = MontarEmailSenhaAlterada(chaveTemp, nomeUsuario);

                System.Threading.Thread mailThread;
                mailThread = new System.Threading.Thread(delegate()
                {
                    mail.EnviarEmail(recoverRequest.Email, "", mensagem, "Recuperação da senha de acesso");
                });
                mailThread.IsBackground = true;
                mailThread.Start();

                Classes.Json.RecoverPasswordResponse recoverResponse = new RecoverPasswordResponse();
                recoverResponse.RecoverKey = chaveTemp;

                jsonResponse = JsonConvert.SerializeObject(recoverResponse).ToString();
                response = Request.CreateResponse(System.Net.HttpStatusCode.OK);
                response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                Classes.Database.RegistrarErro("Server API", "RecoverPasswordController", "Post", ex.Message, _recoverRequest.ToString());

                genericResponse.mensagem = "Não foi possível atender a solicitação";
                jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                response = Request.CreateResponse(System.Net.HttpStatusCode.NoContent);
                response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
            }

            return response;
        }

        protected string MontarEmailSenhaAlterada(string senhaTemp, string nomeUsuario)
        {
            string retorno = string.Empty;

            try
            {
                String strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
                string varURL = HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");

                string varCaminhoHTML = Path.Combine(HttpRuntime.AppDomainAppPath, "Models");
                string varCaminhoModelo = Path.Combine(varCaminhoHTML, "recuperar-senha.html");
                retorno = File.ReadAllText(varCaminhoModelo);
                retorno = retorno.Replace("[NOME]", nomeUsuario);
                retorno = retorno.Replace("[SENHA]", senhaTemp);
            }
            catch (Exception ex)
            {
                Classes.Database.RegistrarErro("Server API", "MontarEmailSenhaAlterada", "Post", ex.Message, string.Empty);
            }

            return retorno;
        }


    }
}