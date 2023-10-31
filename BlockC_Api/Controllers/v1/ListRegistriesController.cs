using Asp.Versioning;
using BlockC_Api.Classes.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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
    [System.Web.Http.Route("api/v{version:apiVersion}/ListRegistries")]
    public class ListRegistriesController : ApiController
    {
        public async Task<HttpResponseMessage> Get([System.Web.Http.FromBody] JObject _request)
        {
            HttpResponseMessage response;

            Classes.Json.GenericResponse genericResponse = new Classes.Json.GenericResponse();
            string jsonResponse = string.Empty;

            try
            {
                Classes.Json.ListRegistriesRequest listRequest = new Classes.Json.ListRegistriesRequest();
                listRequest = JsonConvert.DeserializeObject<Classes.Json.ListRegistriesRequest>(_request.ToString());

                if (listRequest == null)
                {
                    genericResponse.mensagem = "Conteúdo da requisição inválido";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                if (listRequest.Registries == null)
                {
                    genericResponse.mensagem = "Conteúdo da requisição inválido";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                if (listRequest.Pagination == null)
                {
                    genericResponse.mensagem = "Conteúdo da requisição inválido";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                if (string.IsNullOrEmpty(listRequest.Token))
                {
                    genericResponse.mensagem = "Necessário informar um token válido";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                    response = Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                Classes.Database database = new Classes.Database();
                if (!database.BuscarTokenExistente(listRequest.Token))
                {
                    genericResponse.mensagem = "Token inválido";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                    response = Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                if (!database.VerificarUsuarioExiste(listRequest.UserId))
                {
                    genericResponse.mensagem = "Usuário requisitante inválido";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                    response = Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                if (listRequest.Pagination.Page <= 0 || listRequest.Pagination.PageItems <=0)
                {
                    genericResponse.mensagem = "Número da página ou quatidade de registros por página inválidos";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                long usuarioID = listRequest.UserId;
                string filtroEmpresa = string.Empty;
                string filtroCategoria = string.Empty;
                string filtroSubCategoria = string.Empty;
                string filtroUnidade = string.Empty;
                //string filtroDataRegistro = string.Empty;
                string filtroRegistroStatus = string.Empty;
                string filtroNomeDocumento = string.Empty;   
                string filtroMes = string.Empty;
                string filtroAno = string.Empty;
                
                foreach (Classes.Json.ListRegistriesCollection registries in listRequest.Registries)
                {
                    if (registries.CompanyId > 0)
                        filtroEmpresa += string.Concat(registries.CompanyId.ToString(), ",");

                    if (registries.CategoryId > 0)
                        filtroCategoria += string.Concat(registries.CategoryId.ToString(), ",");

                    if (registries.SubCategoryId > 0)
                        filtroSubCategoria += string.Concat(registries.SubCategoryId.ToString(), ",");

                    if (!string.IsNullOrEmpty(registries.Unit))
                        filtroUnidade += string.Concat(registries.Unit.ToString(), ",");

                    if (!string.IsNullOrEmpty(registries.RegistryStatus))
                        filtroRegistroStatus += string.Concat(registries.RegistryStatus.ToString(), ",");

                    if (!string.IsNullOrEmpty(registries.DocumentName))
                        filtroNomeDocumento += string.Concat(registries.DocumentName.ToString(), ",");

                    if (registries.ReferredMonth > 0)
                        filtroMes += string.Concat(registries.ReferredMonth.ToString(), ",");

                    if (registries.ReferredYear > 0)
                        filtroAno += string.Concat(registries.ReferredYear.ToString(), ",");

                    //if (!string.IsNullOrEmpty(registries.MonthYear))
                    //{
                    //    string dateTemp = string.Concat("01/", registries.MonthYear);
                    //    if (DateTime.TryParse(dateTemp, out DateTime temp))
                    //    {
                    //        filtroDataRegistro += string.Concat(dateTemp, ",");
                    //    }
                    //}
                }

                if (filtroEmpresa.EndsWith(","))
                    filtroEmpresa = filtroEmpresa.Remove(filtroEmpresa.Length - 1);

                if (filtroCategoria.EndsWith(","))
                    filtroCategoria = filtroCategoria.Remove(filtroCategoria.Length - 1);

                if (filtroSubCategoria.EndsWith(","))
                    filtroSubCategoria = filtroSubCategoria.Remove(filtroSubCategoria.Length - 1);

                if (filtroUnidade.EndsWith(","))
                    filtroUnidade = filtroUnidade.Remove(filtroUnidade.Length - 1);

                if (filtroRegistroStatus.EndsWith(","))
                    filtroRegistroStatus = filtroRegistroStatus.Remove(filtroRegistroStatus.Length - 1);

                if (filtroNomeDocumento.EndsWith(","))
                    filtroNomeDocumento = filtroNomeDocumento.Remove(filtroNomeDocumento.Length - 1);

                if (filtroMes.EndsWith(","))
                    filtroMes = filtroMes.Remove(filtroMes.Length - 1);

                if (filtroAno.EndsWith(","))
                    filtroAno = filtroAno.Remove(filtroAno.Length - 1);


                //if (filtroDataRegistro.EndsWith(","))
                //    filtroDataRegistro = filtroDataRegistro.Remove(filtroDataRegistro.Length - 1);

                string ordenacao = string.Empty;
                if (listRequest.Sort != null)
                {
                    foreach (Classes.Json.ListRegistriesSort registriesSort in listRequest.Sort)
                    {
                        if (!string.IsNullOrEmpty(registriesSort.Field))
                        {
                            string direcao = "ASC";
                            if (!string.IsNullOrEmpty(registriesSort.Direction))
                            {
                                if (registriesSort.Direction == "ASC" || registriesSort.Direction == "DESC")
                                    direcao = registriesSort.Direction;

                            }

                            ordenacao += string.Concat(registriesSort.Field, " ", direcao, ",");
                        }
                    }
                }

                if (ordenacao.EndsWith(","))
                    ordenacao = ordenacao.Remove(ordenacao.Length - 1);

                ordenacao = ordenacao.Replace("companyId", "lanc.EmpresaID");
                ordenacao = ordenacao.Replace("categoryId", "lanc.CategoriaID");
                ordenacao = ordenacao.Replace("subcategoryId", "lanc.SubCategoriaID");
                ordenacao = ordenacao.Replace("unit", "lanc.UnidadeMedida");
                ordenacao = ordenacao.Replace("referredMonth", "lanc.MesReferencia");
                ordenacao = ordenacao.Replace("referredYear", "lanc.AnoReferencia");
                ordenacao = ordenacao.Replace("status", "lanc.StatusRegistro");                
                ordenacao = ordenacao.Replace("referredDocument", "");
                //ordenacao = ordenacao.Replace("referredDocument", "arq.Nome");

                int pagina = listRequest.Pagination.Page;
                int itensPagina = listRequest.Pagination.PageItems;

                Classes.Json.ListRegistriesResponse listResponse = new ListRegistriesResponse();
                listResponse.Registries = new List<RegistriesResponseCollection>();

                if (!database.BuscarLancamentos(
                    filtroEmpresa
                    , filtroCategoria
                    , filtroSubCategoria
                    , filtroUnidade
                    , filtroMes
                    , filtroAno
                    , filtroRegistroStatus
                    , filtroNomeDocumento
                    , usuarioID
                    , ref listResponse
                    , pagina
                    , itensPagina
                    , ordenacao))
                {
                    genericResponse.mensagem = "Não foi possível buscar os registros";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                    response = Request.CreateResponse(System.Net.HttpStatusCode.NoContent);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                Decimal totalLinhas = database.BuscarTotalLancamentos(
                    filtroEmpresa
                    , filtroCategoria
                    , filtroSubCategoria
                    , filtroUnidade
                    , filtroMes
                    , filtroAno
                    , filtroRegistroStatus
                    , filtroNomeDocumento
                    , usuarioID);

                Decimal total = totalLinhas / itensPagina;
                int totalPages = (int)Math.Ceiling(total);
                listResponse.TotalPages = totalPages;

                jsonResponse = JsonConvert.SerializeObject(listResponse).ToString();
                response = Request.CreateResponse(System.Net.HttpStatusCode.OK);
                response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                Classes.Database.RegistrarErro("Server API", "ListRegistriesController", "Get", ex.Message, _request.ToString());

                genericResponse.mensagem = "Não foi possível atender a solicitação";
                jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                response = Request.CreateResponse(System.Net.HttpStatusCode.NoContent);
                response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
            }

            return response;
        }

    }
}