using BlockC_Api.Classes.Json;
using Microsoft.Win32;
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
using System.Xml.Linq;

namespace BlockC_Api.Controllers.v1
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CreateRegistryController : ApiController
    {

        public async Task<HttpResponseMessage> Post([System.Web.Http.FromBody] JObject _request)
        {
            HttpResponseMessage response;

            Classes.Json.GenericResponse genericResponse = new Classes.Json.GenericResponse();
            string jsonResponse = string.Empty;

            try
            {
                Classes.Json.AddEntryRequest entryRequest = new Classes.Json.AddEntryRequest();
                entryRequest = JsonConvert.DeserializeObject<Classes.Json.AddEntryRequest>(_request.ToString());

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

                    response = Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                Classes.Database database = new Classes.Database();
                if (!database.BuscarTokenExistente(entryRequest.Token))
                {
                    genericResponse.mensagem = "Token inválido";
                    jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();

                    response = Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                    return response;
                }

                string documentID = string.Empty;
                Classes.Json.AddEntryResponse entryResponse = new Classes.Json.AddEntryResponse();
                entryResponse.RegistryList = new List<Classes.Json.RegistryResponseList>();

                foreach (Classes.Json.RegistryList registry in entryRequest.RegistryList)
                {
                    Classes.Json.RegistryResponseList registryResponse = new Classes.Json.RegistryResponseList();

                    //==========================================
                    //              GRAVAR LANCAMENTO
                    //==========================================
                    string entryID = string.Empty;
                    string entryStatus = "Não Auditado";

                    registryResponse.EntryStatus = "FALHA";
                    registryResponse.EntryID = string.Empty;

                    if (!string.IsNullOrEmpty(registry.Unit))
                    {
                        if (!database.ValidarUnidade(registry.Unit))
                        {
                            registryResponse.EntryID = entryID;
                            registryResponse.EntryStatus = "A unidade informada não é válida";
                            entryResponse.RegistryList.Add(registryResponse);
                            continue;
                        }
                    }

                    if (registry.CategoryID > 0 && registry.SubCategoryID > 0)
                    {
                        if (!database.ValidarCategoriaSubcategoria(registry.CategoryID, registry.SubCategoryID))
                        {
                            registryResponse.EntryID = entryID;
                            registryResponse.EntryStatus = "A subcategoria informada não está relacionada à categoria";
                            entryResponse.RegistryList.Add(registryResponse);
                            continue;
                        }
                    }

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

                    string gasID = string.Empty;
                    if (!string.IsNullOrEmpty(registry.GasID))
                        gasID = registry.GasID;

                    Int32.TryParse(registry.ReferenceYear.ToString(), out int referredYear);
                    Int32.TryParse(registry.ReferenceMonth.ToString(), out int referredMonth);

                    if (database.GravarLancamento(companyID, documentID, categoryID, subCategoryID, sourceID, unidade, entryValue, string.Empty, string.Empty
                        , registry.CreatedByID, entryStatus, referredYear, referredMonth, gasID, ref entryID))
                    {

                        if (registry.CustomFields != null)
                        {
                            foreach (Classes.Json.RegistryList.RegistryCustomFields customField in registry.CustomFields)
                            {
                                database.GravarCustomFields(entryID, customField.FieldName, customField.FieldValue);
                                database.AtualizarLancamentoCustomFields(entryID, customField.FieldName, customField.FieldValue);
                            }
                        }

                        if (entryStatus != "Inválido")
                        {
                            entryStatus = "OK";

                            if (!database.GravarEmissaoCalculo(entryID))
                            {
                                entryStatus = "FALHA";
                                Classes.Database.RegistrarErro("Server API", "CreateRegistryController", "Post", "Não foi possível realizar os cálculos do lançamento " + entryID, _request.ToString());
                            }
                            else
                            {
                                database.GravarEmissaoCalculoExpressoes(entryID);
                            }
                        }

                        if (registry.Comments != null)
                        {
                            foreach (RegistryComments comment in registry.Comments)
                            {
                                if (!string.IsNullOrEmpty(comment.Comment))
                                {
                                    DateTime.TryParse(comment.CreatedAt, out DateTime createdAt);
                                    database.GravarComentario(entryID, comment.Comment, comment.UserName, createdAt);
                                }
                            }
                        }

                        registryResponse.EntryID = entryID;
                        registryResponse.EntryStatus = entryStatus;
                        entryResponse.RegistryList.Add(registryResponse);
                    }
                    else
                    {
                        registryResponse.EntryID = entryID;
                        registryResponse.EntryStatus = "FALHA";
                        entryResponse.RegistryList.Add(registryResponse);
                    }

                    //==========================================
                    //              GRAVAR DOCUMENTOS
                    //==========================================
                    if (registry.Documents != null)
                    {
                        registryResponse.Documents = new List<RegistryResponseList.RegistryDocuments>();

                        foreach (RegistryList.RegistryDocuments doc in registry.Documents)
                        {
                            RegistryResponseList.RegistryDocuments responseDocuments = new RegistryResponseList.RegistryDocuments();

                            if (string.IsNullOrEmpty(doc.DocumentImage))
                            {
                                responseDocuments.DocumentID = "";
                                responseDocuments.InsertStatus = "Não possui arquivo";
                                responseDocuments.DocumentName = doc.DocumentName;
                                registryResponse.Documents.Add(responseDocuments);
                                continue; 
                            }

                            if (!Classes.Variaveis.contentTypes.Contains(doc.DocumentContentType.ToLower()))
                            {
                                responseDocuments.DocumentID = "";
                                responseDocuments.InsertStatus = "Documento não permitido. Somente arquivos XLSX, PDF, PNG, JPEG e JPG são permitidos";
                                responseDocuments.DocumentName = doc.DocumentName;
                                registryResponse.Documents.Add(responseDocuments);
                                continue;
                            }

                            byte[] documentImage = Convert.FromBase64String(doc.DocumentImage);
                            string docType = (string.IsNullOrEmpty(doc.DocumentType)) ? "Não Informado" : doc.DocumentType;

                            if (!database.GravarDocumento(doc.DocumentName, docType, doc.DocumentContentType, "", documentImage.Length, documentImage, registry.CreatedByID, ref documentID))
                            {
                                responseDocuments.DocumentID = "";
                                responseDocuments.InsertStatus = "FALHA";
                                responseDocuments.DocumentName = doc.DocumentName;
                            }
                            else
                            {
                                responseDocuments.DocumentID = documentID;
                                responseDocuments.InsertStatus = "OK";
                                responseDocuments.DocumentName = doc.DocumentName;
                            }

                            if (!string.IsNullOrEmpty(documentID) && !string.IsNullOrEmpty(entryID))
                            {
                                database.GravarLancamentoArquivo(entryID, documentID);
                            }

                            registryResponse.Documents.Add(responseDocuments);
                        }

                    }

                }

                jsonResponse = JsonConvert.SerializeObject(entryResponse).ToString();
                response = Request.CreateResponse(System.Net.HttpStatusCode.OK);
                response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                Classes.Database.RegistrarErro("Server API", "CreateRegistryController", "Post", ex.Message, _request.ToString());

                genericResponse.mensagem = "Não foi possível atender a solicitação";
                jsonResponse = JsonConvert.SerializeObject(genericResponse).ToString();
                response = Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
                response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
            }

            return response;
        }

    }
}