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
    public class AddCompanyController : ApiController
    {

        public async Task<HttpResponseMessage> Post([System.Web.Http.FromBody] JObject _addCompanyRequest)
        {
            HttpResponseMessage response;

            Classes.Json.GenericResponse genericResponse = new Classes.Json.GenericResponse();
            string jsonResponse = string.Empty;

            try
            {
                Classes.Json.AddCompanyRequest companyRequest = new Classes.Json.AddCompanyRequest();
                companyRequest = JsonConvert.DeserializeObject<Classes.Json.AddCompanyRequest>(_addCompanyRequest.ToString());

                if (companyRequest == null)
                {
                    genericResponse.mensagem = "Conteúdo da requisição inválido";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                if (string.IsNullOrEmpty(companyRequest.Token))
                {
                    genericResponse.mensagem = "Token inexistente";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                    response = Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                if (string.IsNullOrEmpty(companyRequest.Cidade)
                    || string.IsNullOrEmpty(companyRequest.CNPJ)
                    || string.IsNullOrEmpty(companyRequest.Pais)
                    || string.IsNullOrEmpty(companyRequest.RazaoSocial)
                    || string.IsNullOrEmpty(companyRequest.Setor)
                    || string.IsNullOrEmpty(companyRequest.UF))
                {
                    genericResponse.mensagem = "Necessário informar todos os campos";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                if ((!companyRequest.Matriz) && (companyRequest.MatrizID == 0))
                {
                    genericResponse.mensagem = "Necessário informar o ID da Matriz para cadastrar uma filial";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                Classes.Database database = new Classes.Database();
                if (!database.BuscarTokenExistente(companyRequest.Token))
                {
                    genericResponse.mensagem = "Token inválido";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                    response = Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                string cnpj = companyRequest.CNPJ.Replace(".", "").Replace("-", "").Replace("/", "");
                if (companyRequest.MatrizID == 0)
                {
                    if (database.ValidarCNPJ(cnpj))
                    {
                        genericResponse.mensagem = "CNPJ informado já existe";
                        jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                        response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                        response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                        return response;
                    }
                }
                else
                {
                    if (!database.ValidarCNPJMatriz(cnpj, companyRequest.MatrizID))
                    {
                        if (database.ValidarCNPJ(cnpj))
                        {
                            genericResponse.mensagem = "CNPJ informado já existe";
                            jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                            response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                            response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                            return response;
                        }
                    }
                }

                int matriz = 0;
                int emissao = 0;
                int participacao = 0;
                int controle = 0;                

                if (companyRequest.Matriz)
                {
                    companyRequest.MatrizID = 0;
                    matriz = 1;
                }

                if (companyRequest.Emissao) emissao = 1;
                if (companyRequest.Participacao) participacao = 1;
                if (companyRequest.ControleOperacional) controle = 1;

                long empresaID = database.GravarEmpresa(companyRequest.MatrizID
                    , matriz
                    , companyRequest.RazaoSocial
                    , cnpj
                    , emissao
                    , companyRequest.Setor
                    , companyRequest.Cidade
                    , companyRequest.UF
                    , companyRequest.Pais
                    , participacao
                    , companyRequest.Percentual
                    , controle);

                if (empresaID == 0)
                {
                    genericResponse.mensagem = "Não foi possível realizar o cadastro";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                Classes.Json.AddCompanyResponse companyResponse = new Classes.Json.AddCompanyResponse();
                companyResponse.CompanyId = empresaID;

                jsonResponse = JsonConvert.SerializeObject(companyResponse).ToString();
                response = Request.CreateResponse(System.Net.HttpStatusCode.OK);
                response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                Classes.Database.RegistrarErro("Server API", "AddCompanyController", "Post", ex.Message, _addCompanyRequest.ToString());
                genericResponse.mensagem = "Não foi possível atender a solicitação";
                jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                response = Request.CreateResponse(System.Net.HttpStatusCode.NoContent);
                response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
            }

            return response;
        }

    }
}