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
    [System.Web.Http.Route("api/v{version:apiVersion}/InventoryResults")]
    public class InventoryResultsController : ApiController
    {
        public async Task<HttpResponseMessage> Get([System.Web.Http.FromBody] JObject _request)
        {
            HttpResponseMessage response;

            Classes.Json.GenericResponse genericResponse = new Classes.Json.GenericResponse();
            string jsonResponse = string.Empty;

            try
            {
                Classes.Json.InventoryResultsRequest inventoryRequest = new Classes.Json.InventoryResultsRequest();
                inventoryRequest = JsonConvert.DeserializeObject<Classes.Json.InventoryResultsRequest>(_request.ToString());

                if (inventoryRequest == null)
                {
                    genericResponse.mensagem = "Conteúdo da requisição inválido";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                if (string.IsNullOrEmpty(inventoryRequest.Token))
                {
                    genericResponse.mensagem = "Necessário informar todos os campos";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                    response = Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                Classes.Database database = new Classes.Database();
                if (!database.BuscarTokenExistente(inventoryRequest.Token))
                {
                    genericResponse.mensagem = "Token inválido";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                    response = Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                string[] filtroEmpresaID;
                string[] filtroAno;
                string[] filtroMes;

                filtroEmpresaID = "".Split(',');
                filtroAno = "".Split(',');
                filtroMes = "".Split(',');

                foreach (Classes.Json.InventoryResultsRequest.Filters filtro in inventoryRequest.Filtros)
                {
                    filtroEmpresaID = filtro.EmpresaID.Split(',');
                    filtroAno = filtro.AnoReferencia.Split(',');
                    filtroMes = filtro.MesReferencia.Split(',');
                }

                Classes.Json.InventoryResultsResponse resultsResponse = new Classes.Json.InventoryResultsResponse();
                if (!database.BuscarEmissaoCalculosTotais(inventoryRequest.UsuarioID, filtroEmpresaID, filtroAno, filtroMes, ref resultsResponse))
                {
                    genericResponse.mensagem = "Não encontramos resultados para a requisição";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                    response = Request.CreateResponse(System.Net.HttpStatusCode.OK);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                database.BuscarEmissaoCalculosTotaisEscopo(inventoryRequest.UsuarioID, filtroEmpresaID, filtroAno, filtroMes, ref resultsResponse);
                database.BuscarEmissaoCalculosTotaisAno(inventoryRequest.UsuarioID, filtroEmpresaID, filtroAno, filtroMes, ref resultsResponse);

                jsonResponse = JsonConvert.SerializeObject(resultsResponse).ToString();
                response = Request.CreateResponse(System.Net.HttpStatusCode.OK);
                response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                Classes.Database.RegistrarErro("Server API", "InventoryResultsController", "GET", ex.Message, _request.ToString());

                genericResponse.mensagem = "Não foi possível atender a solicitação";
                jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                response = Request.CreateResponse(System.Net.HttpStatusCode.NoContent);
                response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
            }

            return response;
        }

    }
}