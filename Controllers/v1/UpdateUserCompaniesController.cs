using Asp.Versioning;
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
    [System.Web.Http.Route("api/v{version:apiVersion}/UpdateUserCompanies")]
    public class UpdateUserCompaniesController : ApiController
    {
        public async Task<HttpResponseMessage> Post([System.Web.Http.FromBody] JObject _request)
        {
            HttpResponseMessage response;

            Classes.Json.GenericResponse genericResponse = new Classes.Json.GenericResponse();
            string jsonResponse = string.Empty;

            try
            {
                Classes.Json.UpdateUserCompaniesRequest companyRequest = new Classes.Json.UpdateUserCompaniesRequest();
                companyRequest = JsonConvert.DeserializeObject<Classes.Json.UpdateUserCompaniesRequest>(_request.ToString());

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

                if (companyRequest.UserID <= 0)
                {
                    genericResponse.mensagem = "Necessário informar o ID do usuário";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                    response = Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                if (companyRequest.AddCompaniesList == null && companyRequest.RemoveCompaniesList == null)
                {
                    genericResponse.mensagem = "A requisição não possui nenhuma empresa para adicionar ou remover";
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

                Classes.Json.UpdateUserCompaniesResponse companyResponse = new Classes.Json.UpdateUserCompaniesResponse();

                if (companyRequest.AddCompaniesList != null)
                {
                    companyResponse.AddCompaniesList = new List<Classes.Json.UpdateUserCompaniesResponse.AddCompanies>();

                    foreach(Classes.Json.UpdateUserCompaniesRequest.AddCompanies addCompany in companyRequest.AddCompaniesList)
                    {
                        string mensagem = string.Empty;
                        Classes.Json.UpdateUserCompaniesResponse.AddCompanies company = new Classes.Json.UpdateUserCompaniesResponse.AddCompanies();
                        company.CompanyID = addCompany.CompanyID;
                        company.Status = "OK";

                        if (!database.AdicionarUsuarioEmpresa(companyRequest.UserID, addCompany.CompanyID, ref mensagem))
                        {
                            company.Status = "Não foi possível relacionar a empresa ao usuário";
                        }

                        companyResponse.AddCompaniesList.Add(company);
                    }
                }


                if (companyRequest.RemoveCompaniesList != null)
                {
                    companyResponse.RemoveCompaniesList = new List<Classes.Json.UpdateUserCompaniesResponse.RemoveCompanies>();

                    foreach (Classes.Json.UpdateUserCompaniesRequest.RemoveCompanies delCompany in companyRequest.RemoveCompaniesList)
                    {
                        string mensagem = string.Empty;
                        Classes.Json.UpdateUserCompaniesResponse.RemoveCompanies company = new Classes.Json.UpdateUserCompaniesResponse.RemoveCompanies();
                        company.CompanyID = delCompany.CompanyID;
                        company.Status = "OK";

                        if (!database.DesativarUsuarioEmpresa(companyRequest.UserID, delCompany.CompanyID, ref mensagem))
                        {
                            company.Status = "Não foi possível desassociar a empresa do usuário";
                        }

                        companyResponse.RemoveCompaniesList.Add(company);
                    }
                }

                jsonResponse = JsonConvert.SerializeObject(companyResponse).ToString();
                response = Request.CreateResponse(System.Net.HttpStatusCode.OK);
                response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                Classes.Database.RegistrarErro("Server API", "UpdateUserCompaniesController", "Post", ex.Message, _request.ToString());
                genericResponse.mensagem = "Não foi possível atender a solicitação";
                jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                response = Request.CreateResponse(System.Net.HttpStatusCode.NoContent);
                response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
            }

            return response;
        }
    }
}