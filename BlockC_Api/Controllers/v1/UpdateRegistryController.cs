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
    public class UpdateRegistryController : ApiController
    {
        public async Task<HttpResponseMessage> Post([System.Web.Http.FromBody] JObject _request)
        {
            HttpResponseMessage response;

            Classes.Json.GenericResponse genericResponse = new Classes.Json.GenericResponse();
            string jsonResponse = string.Empty;

            try
            {
                Classes.Json.UpdateEntryRequest entryRequest = new Classes.Json.UpdateEntryRequest();
                entryRequest = JsonConvert.DeserializeObject<Classes.Json.UpdateEntryRequest>(_request.ToString());

                if (entryRequest == null)
                {
                    genericResponse.mensagem = "Conteúdo da requisição inválido";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();

                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                if (entryRequest.RegistryList == null)
                {
                    genericResponse.mensagem = "Conteúdo da requisição inválido";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();

                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                if (string.IsNullOrEmpty(entryRequest.Token))
                {
                    genericResponse.mensagem = "Token inexistente";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();

                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                Classes.Database database = new Classes.Database();
                if (!database.BuscarTokenExistente(entryRequest.Token))
                {
                    genericResponse.mensagem = "Token inválido";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();

                    response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                Classes.Json.UpdateEntryResponse entryResponse = new Classes.Json.UpdateEntryResponse();
                entryResponse.RegistryList = new List<Classes.Json.UpdateEntryResponse.UpdateRegistryResponseList>();

                foreach (Classes.Json.UpdateRegistryList registry in entryRequest.RegistryList)
                {
                    Classes.Json.UpdateEntryResponse.UpdateRegistryResponseList registryResponse = new Classes.Json.UpdateEntryResponse.UpdateRegistryResponseList();

                    if (string.IsNullOrEmpty(registry.RegistryID))
                    {
                        registryResponse.EntryID = registry.RegistryID;
                        registryResponse.EntryStatus = "O ID do Registro não foi informado";
                        continue;
                    }

                    //==================================
                    //          GRAVAR DOCUMENTOS
                    //==================================
                    string documentID = string.Empty;
                    registryResponse.Documents = new List<Classes.Json.UpdateEntryResponse.RegistryDocuments>();

                    if (registry.Documents != null)
                    {
                        foreach (Classes.Json.UpdateRegistryList.RegistryDocuments doc in registry.Documents)
                        {
                            Classes.Json.UpdateEntryResponse.RegistryDocuments docResponse = new Classes.Json.UpdateEntryResponse.RegistryDocuments();
                            docResponse.DocumentStatus = "Sem arquivo na requisição, nenhuma alteração realizada";

                            if (string.IsNullOrEmpty(doc.DocumentID) && string.IsNullOrEmpty(doc.DocumentImage))
                                continue;

                            string docID = string.Empty;
                            if (string.IsNullOrEmpty(doc.DocumentID))
                                docID = Guid.NewGuid().ToString();
                            else
                                docID = doc.DocumentID;

                            docResponse.DocumentStatus = "OK";

                            byte[] documentImage = null;
                            if (!string.IsNullOrEmpty(doc.DocumentImage))
                                documentImage = Convert.FromBase64String(doc.DocumentImage);

                            string docName = string.Empty;
                            if (string.IsNullOrEmpty(doc.DocumentName))
                                docName = "Não informado";
                            else
                                docName = doc.DocumentName;

                            string docType = string.Empty;
                            if (string.IsNullOrEmpty(doc.DocumentType))
                                docType = "Não informado";
                            else
                                docType = doc.DocumentType;

                            string docContentType = string.Empty;
                            if (string.IsNullOrEmpty(doc.DocumentContentType))
                                docContentType = "Não informado";
                            else
                                docContentType = doc.DocumentContentType;

                            int docSize = documentImage.Length;

                            if (!database.GravarDocumento(
                                docID
                                , docName
                                , docType
                                , docContentType
                                , ""
                                , docSize
                                , registry.CreatedByID
                                , ref documentID
                                , documentImage))
                            {
                                docResponse.DocumentStatus = "Não foi possível atualizar o arquivo";
                            }

                            if (!string.IsNullOrEmpty(documentID))
                            {
                                docResponse.DocumentID = documentID;
                                docResponse.DocumentName = doc.DocumentName;
                                database.GravarLancamentoArquivo(registry.RegistryID, documentID);
                            }

                            registryResponse.Documents.Add(docResponse);
                        }
                    }


                    //==================================
                    //         DELETAR DOCUMENTOS
                    //==================================
                    if (registry.DeletedDocuments != null)
                    {
                        foreach (Classes.Json.UpdateRegistryList.RegistryDeletedDocuments doc in registry.DeletedDocuments)
                        {
                            database.DesativarArquivo(registry.RegistryID, doc.DocumentID);
                        }
                    }

                    //==================================
                    //          GRAVAR REGISTRO
                    //==================================
                    string entryStatus = "Não Auditado";

                    long companyID = 0;
                    if (registry.CompanyID > 0)
                        companyID = registry.CompanyID;
                    else
                        entryStatus = "Inválido";

                    int categoryID = 0;
                    if (registry.CategoryID > 0)
                        categoryID = registry.CategoryID;
                    else
                        entryStatus = "Inválido";

                    int subCategoryID = 0;
                    if (registry.SubCategoryID > 0)
                        subCategoryID = registry.SubCategoryID;
                    else
                        entryStatus = "Inválido";

                    int sourceID = 0;
                    if (registry.SourceID > 0)
                        sourceID = registry.SourceID;
                    else
                        entryStatus = "Inválido";

                    string unidade = string.Empty;
                    if (!string.IsNullOrEmpty(registry.Unit))
                        unidade = registry.Unit;
                    else
                        entryStatus = "Inválido";

                    decimal entryValue = 0;
                    if (registry.EntryValue > 0)
                        entryValue = registry.EntryValue;
                    else
                        entryStatus = "Inválido";

                    if (!string.IsNullOrEmpty(registry.RegistryStatus))
                    {
                        if (registry.RegistryStatus.ToUpper() == "INVÁLIDO"
                            || registry.RegistryStatus.ToUpper() == "NÃO AUDITADO"
                            || registry.RegistryStatus.ToUpper() == "AUDITADO"
                            || registry.RegistryStatus.ToUpper() == "REQUER REVISÃO"
                            || registry.RegistryStatus.ToUpper() == "VERIFICADO"
                            || registry.RegistryStatus.ToUpper() == "SOLICITAÇÃO DE AÇÃO CORRETIVA"
                            || registry.RegistryStatus.ToUpper() == "SOLICITAÇÃO DE ESCLARECIMENTO")
                        {
                            entryStatus = registry.RegistryStatus;
                        }
                    }                    

                    registryResponse.EntryID = registry.RegistryID;
                    registryResponse.EntryStatus = "FALHA";

                    if (database.AtualizarLancamento(
                        registry.RegistryID
                        , companyID
                        , ""
                        , categoryID
                        , subCategoryID
                        , unidade
                        , entryValue
                        , ""
                        , ""
                        , registry.CreatedByID
                        , sourceID
                        , registry.GasID
                        , entryStatus))
                    {
                        registryResponse.EntryStatus = "OK";
                    }

                    if (registry.Comments != null)
                    {
                        foreach (var comment in registry.Comments)
                        {
                            database.GravarComentario(registry.RegistryID, comment.Comment, comment.UserName, System.DateTime.Now);
                        }
                    }

                    entryResponse.RegistryList.Add(registryResponse);
                }

                jsonResponse = JsonConvert.SerializeObject(entryResponse).ToString();
                response = Request.CreateResponse(System.Net.HttpStatusCode.OK);
                response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                Classes.Database.RegistrarErro("Server API", "UpdateRegistryController", "Post", ex.Message, _request.ToString());

                genericResponse.mensagem = "Não foi possível atender a solicitação";
                jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                response = Request.CreateResponse(System.Net.HttpStatusCode.NoContent);
                response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
            }

            return response;
        }
    }
}