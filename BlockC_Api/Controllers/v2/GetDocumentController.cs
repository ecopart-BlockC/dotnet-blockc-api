using Asp.Versioning;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace BlockC_Api.Controllers.v2
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [ApiVersion("2.0")]
    [System.Web.Http.Route("api/v{version:apiVersion}/GetDocument")]
    public class GetDocumentController : ApiController
    {
        public async Task<HttpResponseMessage> Get([System.Web.Http.FromBody] JObject _request)
        {
            HttpResponseMessage response;

            Classes.Json.GenericResponse genericResponse = new Classes.Json.GenericResponse();
            string jsonResponse = string.Empty;

            try
            {
                string filePath = @"C:\ITFour\Projetos\BLOCKC\BlockC_v2\BlockC_Api\App_Data\";
                string fileName = @"529-2711-1-CE.pdf";
                string mimeType = MimeMapping.GetMimeMapping(fileName);

                filePath = Path.Combine(filePath, fileName);
                Stream stream = new FileStream(filePath, FileMode.Open);

                response = Request.CreateResponse(System.Net.HttpStatusCode.OK);
                response.Content = new StreamContent(stream);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue(mimeType);
                response.Content.Headers.ContentLength = stream.Length;
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = Path.GetFileName(filePath) };
                response.Content.Headers.ContentDisposition.FileName = fileName;

            }
            catch (Exception ex)
            {
                Classes.Database.RegistrarErro("Server API", "GetDocumentController - v2", "Get", ex.Message, "");
                genericResponse.mensagem = ex.Message;
                jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                response = Request.CreateResponse(System.Net.HttpStatusCode.NoContent);
                response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
            }

            return response;
        }

    }
}