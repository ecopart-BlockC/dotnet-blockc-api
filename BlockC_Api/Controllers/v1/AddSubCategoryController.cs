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
    public class AddSubCategoryController : ApiController
    {

        public async Task<HttpResponseMessage> Post([System.Web.Http.FromBody] JObject _request)
        {
            HttpResponseMessage response;

            Classes.Json.GenericResponse genericResponse = new Classes.Json.GenericResponse();
            string jsonResponse = string.Empty;

            try
            {
                Classes.Json.AddSubCategoryRequest subCategoryRequest = new Classes.Json.AddSubCategoryRequest();
                subCategoryRequest = JsonConvert.DeserializeObject<Classes.Json.AddSubCategoryRequest>(_request.ToString());

                if (subCategoryRequest == null)
                {
                    genericResponse.mensagem = "Conteúdo da requisição inválido";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                if (string.IsNullOrEmpty(subCategoryRequest.Token))
                {
                    genericResponse.mensagem = "Token inexistente";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                    response = Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                if (string.IsNullOrEmpty(subCategoryRequest.SubCategoryName) || subCategoryRequest.CategoryID == 0 || subCategoryRequest.Scope == 0)
                {
                    genericResponse.mensagem = "Necessário informar todos os campos";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                Classes.Database database = new Classes.Database();
                if (!database.BuscarTokenExistente(subCategoryRequest.Token))
                {
                    genericResponse.mensagem = "Token inválido";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                    response = Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                if (!database.VerificarCategoriaExisteID(subCategoryRequest.CategoryID))
                {
                    genericResponse.mensagem = "A categoria informada é inválida";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                if (database.VerificarSubCategoriaExiste(subCategoryRequest.SubCategoryName))
                {
                    genericResponse.mensagem = "A subcategoria informada já existe";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                int subCategoriaID = 0;
                if (!database.GravarSubCategoria(subCategoryRequest.CategoryID, subCategoryRequest.SubCategoryName, subCategoryRequest.Scope, subCategoryRequest.Comments, ref subCategoriaID))
                {
                    genericResponse.mensagem = "Não foi possível gravar a SubCategoria";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                    response = Request.CreateResponse(System.Net.HttpStatusCode.NoContent);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                Classes.Json.AddSubCategoryResponse subCategoryResponse = new Classes.Json.AddSubCategoryResponse();
                subCategoryResponse.SubCategoryId = subCategoriaID;
                jsonResponse = JsonConvert.SerializeObject(subCategoryResponse).ToString();
                response = Request.CreateResponse(System.Net.HttpStatusCode.OK);
                response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                Classes.Database.RegistrarErro("Server API", "AddSubCategoryController", "Post", ex.Message, _request.ToString());

                genericResponse.mensagem = "Não foi possível atender a solicitação";
                jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                response = Request.CreateResponse(System.Net.HttpStatusCode.NoContent);
                response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
            }

            return response;
        }

    }
}