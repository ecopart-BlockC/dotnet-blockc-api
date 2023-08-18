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
    public class AddCategoryController : ApiController
    {

        public async Task<HttpResponseMessage> Post([System.Web.Http.FromBody] JObject _request)
        {
            HttpResponseMessage response;

            Classes.Json.GenericResponse genericResponse = new Classes.Json.GenericResponse();
            string jsonResponse = string.Empty;

            try
            {
                Classes.Json.AddCategoryRequest categoryRequest = new Classes.Json.AddCategoryRequest();
                categoryRequest = JsonConvert.DeserializeObject<Classes.Json.AddCategoryRequest>(_request.ToString());

                if (categoryRequest == null)
                {
                    genericResponse.mensagem = "Conteúdo da requisição inválido";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();

                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                if (string.IsNullOrEmpty(categoryRequest.Token))
                {
                    genericResponse.mensagem = "Token inexistente";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();

                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                if (string.IsNullOrEmpty(categoryRequest.CategoryName))
                {
                    genericResponse.mensagem = "Necessário informar todos os campos";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();

                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                Classes.Database database = new Classes.Database();
                if (!database.BuscarTokenExistente(categoryRequest.Token))
                {
                    genericResponse.mensagem = "Token inválido";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();

                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                if (database.VerificarCategoriaExiste(categoryRequest.CategoryName.ToUpper()))
                {
                    genericResponse.mensagem = "A categoria informada já existe";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();

                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                int categoriaID = 0;
                if (!database.GravarCategoria(categoryRequest.CategoryName.ToString(), ref categoriaID))
                {
                    genericResponse.mensagem = "Não foi possível salvar a categoria";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();

                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                Classes.Json.AddCategoryResponse categoryResponse = new Classes.Json.AddCategoryResponse();
                categoryResponse.CategoryId = categoriaID;

                jsonResponse = JsonConvert.SerializeObject(categoryResponse).ToString();
                response = Request.CreateResponse(System.Net.HttpStatusCode.OK);
                response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                Classes.Database.RegistrarErro("Server API", "AddCategoryController", "Post", ex.Message, _request.ToString());

                genericResponse.mensagem = "Não foi possível atender a solicitação";
                jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                response = Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
                response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
            }

            return response;
        }

    }
}