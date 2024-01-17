using Asp.Versioning;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace BlockC_Api.Controllers.v1
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [ApiVersion("2.0")]
    [System.Web.Http.Route("api/v{version:apiVersion}/LargeFile")]
    public class LargeFileController : ApiController
    {
        public async Task<HttpResponseMessage> Post()
        {
            HttpResponseMessage response;

            try
            {
                var root = HttpContext.Current.Server.MapPath("~/App_Data");
                //var root = @"C:\Testes\";
                var provider = new MultipartFormDataStreamProvider(root);

                try
                {
                     await Request.Content.ReadAsMultipartAsync(provider);

                    foreach (var key in provider.FormData.AllKeys)
                    {
                        foreach (var val in provider.FormData.GetValues(key))
                        {
                            var keyValue = string.Format("{0}: {1}", key, val);
                        }
                    }

                    foreach (MultipartFileData fileData in provider.FileData)
                    {
                        var contentType = fileData.Headers.ContentType;
                        var fileName = fileData.Headers.ContentDisposition.FileName.Replace(@"""", string.Empty);
                        File.Copy(fileData.LocalFileName, Path.Combine(root, fileName), true);
                
                        if (File.Exists(fileData.LocalFileName))
                            File.Delete(fileData.LocalFileName);

                    }

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                catch (Exception e)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
                }


                //string fileName = @"C:\Testes\Teste.pdf";
                //byte[] receivedBytes = await Request.Content.ReadAsByteArrayAsync();

                //using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true))
                //{
                //    await Request.Content.CopyToAsync(fs);
                //}

                //response = Request.CreateResponse(System.Net.HttpStatusCode.OK);
                //response.Content = new StringContent("", Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
                response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
            }

            return response;
        }

    }
}