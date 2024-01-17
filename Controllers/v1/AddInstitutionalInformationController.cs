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
    [System.Web.Http.Route("api/v{version:apiVersion}/AddInstitutionalInformation")]
    public class AddInstitutionalInformationController : ApiController
    {
        private int institutionalInformationID;

        public async Task<HttpResponseMessage> Post([System.Web.Http.FromBody] JObject _request)
        {
            HttpResponseMessage response;

            Classes.Json.GenericResponse genericResponse = new Classes.Json.GenericResponse();
            string jsonResponse = string.Empty;

            try
            {
                Classes.Json.AddInstitutionalInformationRequest institutionalInformationRequest = new Classes.Json.AddInstitutionalInformationRequest();
                institutionalInformationRequest = JsonConvert.DeserializeObject<Classes.Json.AddInstitutionalInformationRequest>(_request.ToString());


                Classes.Database database = new Classes.Database();
                if (!database.BuscarTokenExistente(institutionalInformationRequest.Token))
                {
                    genericResponse.mensagem = "Token inválido";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                    response = Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                if (!database.GravarInformacoesInstitucionais(institutionalInformationRequest.CompanyId
                                                            , institutionalInformationRequest.Content.ToString()
                                                            , institutionalInformationRequest.UserId.ToString()))
                {
                    genericResponse.mensagem = "Não foi possível gravar a informação institucional";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                Classes.Json.AddInstitutionalInformationResponse institutionalInformationResponse = new Classes.Json.AddInstitutionalInformationResponse();
                institutionalInformationResponse.InstitutionalInformationId = institutionalInformationID;

                jsonResponse = JsonConvert.SerializeObject(institutionalInformationResponse).ToString();
                response = Request.CreateResponse(System.Net.HttpStatusCode.OK);
                response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                Classes.Database.RegistrarErro("Server API", "AddInstitutionalInformationController", "Post", ex.Message, _request.ToString());

                genericResponse.mensagem = "Não foi possível atender a solicitação";
                jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                response = Request.CreateResponse(System.Net.HttpStatusCode.NoContent);
                response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
            }

            return response;
        }

    }
}