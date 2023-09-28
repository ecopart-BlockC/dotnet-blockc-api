using Antlr.Runtime.Misc;
using BlockC_Api.Classes.Json;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Helpers;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using static BlockC_Api.Classes.Json.RegistriesResponseCollection;

namespace BlockC_Api.Classes
{
    public class Database
    {
#if DEBUG
        public static string connString = System.Configuration.ConfigurationManager.ConnectionStrings["BLOCKC_HML"].ToString();
#else
        public static string connString = System.Configuration.ConfigurationManager.ConnectionStrings["BLOCKC_PRD"].ToString();
#endif

        public static void RegistrarErro(string app, string varTela, string varRotina, string varMensagem, string varRequisicao)
        {
            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Gravar_Erro", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("varApp", app);
                        varComm.Parameters.AddWithValue("varTela", varTela);
                        varComm.Parameters.AddWithValue("varRotina", varRotina);
                        varComm.Parameters.AddWithValue("varDescricao", varMensagem);
                        varComm.Parameters.AddWithValue("varRequisicao", varRequisicao);
                        varComm.ExecuteNonQuery();
                    }
                }
            }
            catch
            { }
        }

        public static Boolean ValidarApiKey(string apiKey)
        {
            Boolean retorno;

            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Validar_ApiKey", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("varApiKey", apiKey);

                        using (SqlDataReader reader = varComm.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                        {
                            retorno = reader.HasRows;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "ValidarApiKey", ex.Message, string.Empty);
                retorno = false;
            }

            return retorno;
        }

        public Boolean BuscarTokenExistente(string tokenValido)
        {
            Boolean retorno = false;

            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Buscar_Token_Existente", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("varToken", tokenValido);

                        using (SqlDataReader reader = varComm.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                        {
                            retorno = reader.HasRows;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "BuscarTokenExistente", ex.Message, string.Empty);
            }

            return retorno;
        }

        public void GravarToken(string newToken)
        {
            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Gravar_Token", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("varNewToken", newToken);
                        varComm.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "GravarToken", ex.Message, string.Empty);
            }
        }

        public Boolean ValidarEmail(string email)
        {
            Boolean retorno = true;

            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Buscar_Usuario_Email", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("varEmail", email);

                        using (SqlDataReader reader = varComm.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                        {
                            retorno = reader.HasRows;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "ValidarEmail", ex.Message, string.Empty);
                retorno = true;
            }

            return retorno;
        }

        public long GravarUsuario(string Nome, string Sobrenome, string Tipo, string Email, string Senha)
        {
            long retorno = 0;

            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Gravar_Usuario", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("varNome", Nome);
                        varComm.Parameters.AddWithValue("varSobrenome", Sobrenome);
                        varComm.Parameters.AddWithValue("varTipo", Tipo);
                        varComm.Parameters.AddWithValue("varEmail", Email);
                        varComm.Parameters.AddWithValue("varSenha", Crypto.Encrypt(Senha));

                        using (SqlDataReader reader = varComm.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                        {
                            if (!reader.HasRows) return 0;

                            reader.Read();
                            Int64.TryParse(reader["UsuarioID"].ToString(), out retorno);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "GravarUsuario", ex.Message, string.Empty);
            }

            return retorno;
        }

        public Boolean ValidarCNPJ(string cnpj)
        {
            Boolean retorno = true;

            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Buscar_Empresa_CNPJ", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("varCNPJ", cnpj);

                        using (SqlDataReader reader = varComm.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                        {
                            retorno = reader.HasRows;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "ValidarCNPJ", ex.Message, string.Empty);
                retorno = true;
            }

            return retorno;
        }

        public long GravarEmpresa(long MatrizID, int Matriz, string RazaoSocial, string CNPJ, int Emissao, string Setor, string Cidade, string UF, string Pais, int Participacao, Decimal Percentual, int ControleOperacional)
        {
            long retorno = 0;

            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Gravar_Empresa", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        
                        if (MatrizID <= 0)
                        {
                            varComm.Parameters.AddWithValue("varMatrizID", null);
                        }
                        else
                        {
                            varComm.Parameters.AddWithValue("varMatrizID", MatrizID);
                        }                            

                        varComm.Parameters.AddWithValue("varMatriz", Matriz);
                        varComm.Parameters.AddWithValue("varRazaoSocial", RazaoSocial);
                        varComm.Parameters.AddWithValue("varCNPJ", CNPJ);
                        varComm.Parameters.AddWithValue("varEmissao", Emissao);
                        varComm.Parameters.AddWithValue("varSetor", Setor);
                        varComm.Parameters.AddWithValue("varCidade", Cidade);
                        varComm.Parameters.AddWithValue("varUF", UF);
                        varComm.Parameters.AddWithValue("varPais", Pais);
                        varComm.Parameters.AddWithValue("varParticipacao", Participacao);
                        varComm.Parameters.AddWithValue("varPercentual", Percentual);
                        varComm.Parameters.AddWithValue("varControleOperacional", ControleOperacional);

                        using (SqlDataReader reader = varComm.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                        {
                            if (!reader.HasRows) return 0;

                            reader.Read();
                            Int64.TryParse(reader["EmpresaID"].ToString(), out retorno);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "GravarEmpresa", ex.Message, string.Empty);
            }

            return retorno;
        }

        public Boolean Gravar_EmpresaUsuario(long empresaID, long usuarioID)
        {
            Boolean retorno = true;

            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Gravar_EmpresaUsuario", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("varEmpresaID", empresaID);
                        varComm.Parameters.AddWithValue("varUsuarioID", usuarioID);
                        varComm.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "Gravar_EmpresaUsuario", ex.Message, string.Empty);
                retorno = false;
            }

            return retorno;
        }

        public DataTable BuscarUsuario(string email, string senha)
        {
            DataTable retorno = new DataTable();

            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Buscar_Usuario", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("varEmail", email);

                        using (SqlDataReader myReader = varComm.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (myReader.HasRows)
                            {
                                retorno.Load(myReader);

                                int pendente = Convert.ToInt32(retorno.Rows[0]["UsuarioPendente"].ToString());
                                string senhaDB = Crypto.Decrypt(retorno.Rows[0]["UsuarioSenha"].ToString());

                                if (pendente == 1)
                                    senhaDB = Crypto.Decrypt(retorno.Rows[0]["UsuarioChaveTemp"].ToString());

                                if (senha != senhaDB)
                                    retorno.Clear();

                            }                                
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "BuscarUsuario", ex.Message, string.Empty);
            }

            return retorno;
        }

        public Boolean AlterarSenhaUsuario(long usuarioID, string novaSenha)
        {
            Boolean retorno = true;

            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Atualizar_Usuario_Senha", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("varUsuarioID", usuarioID);
                        varComm.Parameters.AddWithValue("varNovaSenha", Crypto.Encrypt(novaSenha));
                        varComm.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "AlterarSenhaUsuario", ex.Message, string.Empty);
                retorno = false;
            }

            return retorno;
        }

        public Boolean DesativarUsuario(long empresaID, long usuarioID)
        {
            Boolean retorno = true;

            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Desativar_Usuario", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("varUsuarioID", usuarioID);
                        varComm.Parameters.AddWithValue("varEmpresaID", empresaID);
                        varComm.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "DesativarUsuario", ex.Message, string.Empty);
                retorno = false;
            }

            return retorno;
        }

        public Boolean VerificarUsuarioMaster(long usuarioID)
        {
            Boolean retorno = true;

            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Buscar_Usuario_Master", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("varUsuarioID", usuarioID);
                        
                        using (SqlDataReader myReader = varComm.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            retorno = myReader.HasRows;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "VerificarUsuarioMaster", ex.Message, string.Empty);
                retorno = false;
            }

            return retorno;
        }

        public Boolean VerificarUsuarioExiste(long usuarioID)
        {
            Boolean retorno = true;

            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Buscar_Usuario_Existente", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("varUsuarioID", usuarioID);

                        using (SqlDataReader myReader = varComm.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            retorno = myReader.HasRows;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "VerificarUsuarioExiste", ex.Message, string.Empty);
                retorno = false;
            }

            return retorno;
        }

        public Boolean VerificarUsuarioAprovado(string email)
        {
            Boolean retorno = false;

            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Validar_Login", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("varEmail", email);

                        using (SqlDataReader myReader = varComm.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (myReader.HasRows)
                            {
                                myReader.Read();

                                Int32.TryParse(myReader["UsuarioAprovado"].ToString(), out int aprovado);

                                if (aprovado == 1)
                                    retorno = true;
                            }                  
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "VerificarUsuarioAprovado", ex.Message, string.Empty);
                retorno = false;
            }

            return retorno;
        }

        public Boolean ValidarLogin(string email, string senha, ref Classes.Json.LoginResponse loginResponse)
        {
            Boolean retorno = true;

            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Validar_Login", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("varEmail", email);

                        using (SqlDataReader myReader = varComm.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (!myReader.HasRows) return false;

                            Classes.Json.userCompanies companies = new userCompanies();
                            loginResponse.UsuarioEmpresas = new List<userCompanies>();

                            while (myReader.Read())
                            {
                                int pendente = Convert.ToInt32(myReader["UsuarioPendente"].ToString());
                                string senhaDB = Crypto.Decrypt(myReader["UsuarioSenha"].ToString());

                                if (pendente == 1)
                                    senhaDB = Crypto.Decrypt(myReader["UsuarioChaveTemp"].ToString());

                                if (senha != senhaDB) return false;

                                loginResponse.UsuarioID = Convert.ToInt64(myReader["UsuarioID"].ToString());
                                loginResponse.UsuarioNome = myReader["UsuarioNome"].ToString();
                                loginResponse.UsuarioSobreNome = myReader["UsuarioSobrenome"].ToString();
                                loginResponse.UsuarioEmail = email;
                                loginResponse.UsuarioTipo = myReader["UsuarioTipo"].ToString();
                                loginResponse.Pendente = (myReader["UsuarioPendente"].ToString() == "1") ? true : false;

                                if (string.IsNullOrEmpty(myReader["EmpresaID"].ToString()) || myReader["EmpresaID"].ToString() == "0")
                                    continue;

                                companies = new userCompanies();
                                companies.EmpresaID = Convert.ToInt64(myReader["EmpresaID"].ToString());
                                companies.MatrizID = (string.IsNullOrEmpty(myReader["EmpresaMatrizID"].ToString()) ? 0 : Convert.ToInt64(myReader["EmpresaMatrizID"].ToString()));
                                companies.Matriz = (myReader["EmpresaMatriz"].ToString() == "1") ? true : false;
                                companies.RazaoSocial = myReader["EmpresaRazao"].ToString();
                                companies.CNPJ = myReader["EmpresaCNPJ"].ToString();
                                companies.Emissao = (myReader["EmpresaEmissao"].ToString() == "1") ? true : false;
                                companies.Setor = myReader["EmpresaSetor"].ToString();
                                companies.Cidade = myReader["EmpresaCidade"].ToString();
                                companies.UF = myReader["EmpresaUF"].ToString();
                                companies.Pais = myReader["EmpresaPais"].ToString();
                                companies.Participacao = (myReader["EmpresaParticipacao"].ToString() == "1") ? true : false;
                                companies.Percentual = Convert.ToDecimal(myReader["EmpresaPercParticipacao"].ToString());
                                companies.ControleOperacional = (myReader["EmpresaControleOp"].ToString() == "1") ? true : false;
                                loginResponse.UsuarioEmpresas.Add(companies);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "VerificarUsuarioExiste", ex.Message, string.Empty);
                retorno = false;
            }

            return retorno;
        }

        public Boolean InvalidarToken(string token)
        {
            Boolean retorno = true;

            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Atualizar_Token_Vencido", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("varToken", token);
                        varComm.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "InvalidarToken", ex.Message, string.Empty);
                retorno = false;
            }

            return retorno;
        }

        public void Desativar_EmpresaUsuario(long EmpresaID, long UsuarioID)
        {
            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Desativar_EmpresaUsuario", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("varEmpresaID", EmpresaID);
                        varComm.Parameters.AddWithValue("varUsuarioID", UsuarioID);
                        varComm.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "Desativar_EmpresaUsuario", ex.Message, string.Empty);
            }
        }

        public Boolean AtualizarUsuarioSenhaPendente(string email, string chaveTemp)
        {
            Boolean retorno = true;

            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Atualizar_Usuario_SenhaPendente", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("varEmail", email);
                        varComm.Parameters.AddWithValue("varChaveTemp", Crypto.Encrypt(chaveTemp));
                        varComm.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "AtualizarUsuarioSenhaPendente", ex.Message, string.Empty);
                retorno = false;
            }

            return retorno;
        }

        public DataTable BuscarParametroEmail()
        {
            DataTable retorno = new DataTable();

            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Buscar_Parametro_Mail", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;

                        using (SqlDataReader varReader = varComm.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            retorno.Load(varReader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "BuscarParametroEmail", ex.Message, string.Empty);
            }

            return retorno;
        }

        public Boolean BuscarEmpresaUsuario(long userID, ref Classes.Json.GetUserCompanyResponse userCompanyResponse, ref string mensagem)
        {
            Boolean retorno = true;

            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Buscar_EmpresaUsuario_Usuario", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("UsuarioID", userID);

                        using (SqlDataReader myReader = varComm.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (!myReader.HasRows)
                            {
                                mensagem = "Não há empresas relacionadas ao usuário";
                                return false;
                            }

                            Classes.Json.GetUserCompanyList companies = new GetUserCompanyList();
                            Classes.Json.BranchesList branches = new BranchesList();
                            Classes.Json.CompanyUsers companyUsers = new CompanyUsers();

                            userCompanyResponse.UsuarioEmpresas = new List<GetUserCompanyList>();
                            companies.Filiais = new List<BranchesList>();

                            while (myReader.Read())
                            {
                                branches = new BranchesList();
                                companies = new GetUserCompanyList();
                                companies.EmpresaID = Convert.ToInt64(myReader["EmpresaUsuario_EmpresaID"].ToString());
                                companies.MatrizID = (string.IsNullOrEmpty(myReader["Empresa_MatrizID"].ToString()) ? 0 : Convert.ToInt64(myReader["Empresa_MatrizID"].ToString()));
                                companies.Matriz = (myReader["Empresa_Matriz"].ToString() == "1") ? true : false;
                                companies.RazaoSocial = myReader["Empresa_Razao"].ToString();
                                companies.CNPJ = myReader["Empresa_CNPJ"].ToString();
                                companies.Emissao = (myReader["Empresa_Emissao"].ToString() == "1") ? true : false;
                                companies.Setor = myReader["Empresa_Setor"].ToString();
                                companies.Cidade = myReader["Empresa_Cidade"].ToString();
                                companies.UF = myReader["Empresa_UF"].ToString();
                                companies.Pais = myReader["Empresa_Pais"].ToString();
                                companies.Participacao = (myReader["Empresa_Participacao"].ToString() == "1") ? true : false;
                                companies.Percentual = Convert.ToDecimal(myReader["Empresa_PercParticipacao"].ToString());
                                companies.ControleOperacional = (myReader["Empresa_ControleOp"].ToString() == "1") ? true : false;

                                BuscarEmpresaUsuarios(companies.EmpresaID, ref companies, ref companyUsers);

                                BuscarEmpresaFilial(companies.EmpresaID, ref companies, ref branches);
                                userCompanyResponse.UsuarioEmpresas.Add(companies);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "BuscarEmpresaUsuario", ex.Message, string.Empty);
                retorno = false;
                mensagem = "Não foi possível verificar as empresas relacionadas ao usuário";
            }

            return retorno;
        }

        public void BuscarFilialUsuarios(long EmpresaID, ref BranchesList branches, ref CompanyUsers companyUsers)
        {
            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Buscar_Empresa_Usuarios", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("EmpresaID", EmpresaID);

                        using (SqlDataReader myReader = varComm.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (!myReader.HasRows) return;

                            branches.Usuarios = new List<CompanyUsers>();

                            while (myReader.Read())
                            {
                                companyUsers = new CompanyUsers();
                                companyUsers.UsuarioID = Convert.ToInt64(myReader["UsuarioID"].ToString());
                                companyUsers.UsuarioNome = (string.IsNullOrEmpty(myReader["UsuarioNome"].ToString()) ? "Não encontrado" : myReader["UsuarioNome"].ToString());
                                companyUsers.UsuarioSobrenome = (string.IsNullOrEmpty(myReader["UsuarioSobrenome"].ToString()) ? "Não encontrado" : myReader["UsuarioSobrenome"].ToString());
                                branches.Usuarios.Add(companyUsers);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "BuscarFilialUsuarios", ex.Message, string.Empty);
            }
        }

        public void BuscarEmpresaUsuarios(long EmpresaID, ref GetUserCompanyList companies, ref CompanyUsers companyUsers)
        {
            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Buscar_Empresa_Usuarios", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("EmpresaID", EmpresaID);

                        using (SqlDataReader myReader = varComm.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (!myReader.HasRows) return;

                            companies.Usuarios = new List<CompanyUsers>();

                            while (myReader.Read())
                            {
                                companyUsers = new CompanyUsers();
                                companyUsers.UsuarioID = Convert.ToInt64(myReader["UsuarioID"].ToString());
                                companyUsers.UsuarioNome = (string.IsNullOrEmpty(myReader["UsuarioNome"].ToString()) ? "Não encontrado" : myReader["UsuarioNome"].ToString());
                                companyUsers.UsuarioSobrenome = (string.IsNullOrEmpty(myReader["UsuarioSobrenome"].ToString()) ? "Não encontrado" : myReader["UsuarioSobrenome"].ToString());
                                companies.Usuarios.Add(companyUsers);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "BuscarEmpresaFilial", ex.Message, string.Empty);
            }
        }

        public void BuscarEmpresaFilial(long matrizID, ref Classes.Json.GetUserCompanyList companies, ref Classes.Json.BranchesList branches)
        {
            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Buscar_Empresa_Filial", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("MatrizID", matrizID);

                        using (SqlDataReader myReader = varComm.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (!myReader.HasRows) return;

                            companies.Filiais = new List<BranchesList>();
                            
                            while (myReader.Read())
                            {
                                branches = new BranchesList();
                                branches.EmpresaID = Convert.ToInt64(myReader["EmpresaUsuario_EmpresaID"].ToString());
                                branches.MatrizID = (string.IsNullOrEmpty(myReader["Empresa_MatrizID"].ToString()) ? 0 : Convert.ToInt64(myReader["Empresa_MatrizID"].ToString()));
                                branches.Matriz = (myReader["Empresa_Matriz"].ToString() == "1") ? true : false;
                                branches.RazaoSocial = myReader["Empresa_Razao"].ToString();
                                branches.CNPJ = myReader["Empresa_CNPJ"].ToString();
                                branches.Emissao = (myReader["Empresa_Emissao"].ToString() == "1") ? true : false;
                                branches.Setor = myReader["Empresa_Setor"].ToString();
                                branches.Cidade = myReader["Empresa_Cidade"].ToString();
                                branches.UF = myReader["Empresa_UF"].ToString();
                                branches.Pais = myReader["Empresa_Pais"].ToString();
                                branches.Participacao = (myReader["Empresa_Participacao"].ToString() == "1") ? true : false;
                                branches.Percentual = Convert.ToDecimal(myReader["Empresa_PercParticipacao"].ToString());
                                branches.ControleOperacional = (myReader["Empresa_ControleOp"].ToString() == "1") ? true : false;
                                branches.Usuarios = new List<CompanyUsers>();

                                CompanyUsers companyUsers = new CompanyUsers();
                                BuscarFilialUsuarios(companies.EmpresaID, ref branches, ref companyUsers);

                                companies.Filiais.Add(branches);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "BuscarEmpresaFilial", ex.Message, string.Empty);
            }
        }

        public string BuscarUsuarioLista(long EmpresaID, ref Classes.Json.GetCompanyUsersResponse userCompanyResponse)
        {
            string retorno = "OK";

            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Buscar_Usuario_Lista", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("EmpresaID", EmpresaID);

                        using (SqlDataReader myReader = varComm.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (!myReader.HasRows)
                            {
                                retorno = "A empresa informada não possui usuários relacionados";
                                return retorno;
                            }

                            Classes.Json.CompanyUserList userList = new CompanyUserList();
                            Classes.Json.userCompanies companies = new userCompanies();
                            userCompanyResponse.UsersList = new List<CompanyUserList>();

                            while (myReader.Read())
                            {
                                userList = new CompanyUserList();
                                userList.UsuarioID = Convert.ToInt64(myReader["Usuario_ID"].ToString());
                                userList.Nome = myReader["Usuario_Nome"].ToString();
                                userList.Sobrenome = myReader["Usuario_Sobrenome"].ToString();
                                userList.Tipo = myReader["Usuario_Tipo"].ToString();
                                userList.Email = myReader["Usuario_Email"].ToString();

                                BuscarUsuarioEmpresas(Convert.ToInt64(myReader["Usuario_ID"].ToString()), ref userList, ref companies);
                                userCompanyResponse.UsersList.Add(userList);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "BuscarUsuarioLista", ex.Message, string.Empty);
                retorno = "Não conseguimos buscar as informações do usuário informado";
            }

            return retorno;
        }

        public void BuscarUsuarioEmpresas(long UsuarioID, ref CompanyUserList userList, ref userCompanies companies)
        {
            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Buscar_EmpresaFilial_Usuario", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("UsuarioID", UsuarioID);

                        using (SqlDataReader myReader = varComm.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (!myReader.HasRows) return;

                            userList.Empresas = new List<userCompanies>();

                            while (myReader.Read())
                            {
                                companies = new userCompanies();
                                companies.EmpresaID = Convert.ToInt64(myReader["EmpresaUsuario_EmpresaID"].ToString());
                                companies.MatrizID = (string.IsNullOrEmpty(myReader["Empresa_MatrizID"].ToString()) ? 0 : Convert.ToInt64(myReader["Empresa_MatrizID"].ToString()));
                                companies.Matriz = (myReader["Empresa_Matriz"].ToString() == "1") ? true : false;
                                companies.RazaoSocial = myReader["Empresa_Razao"].ToString();
                                companies.CNPJ = myReader["Empresa_CNPJ"].ToString();
                                companies.Emissao = (myReader["Empresa_Emissao"].ToString() == "1") ? true : false;
                                companies.Setor = myReader["Empresa_Setor"].ToString();
                                companies.Cidade = myReader["Empresa_Cidade"].ToString();
                                companies.UF = myReader["Empresa_UF"].ToString();
                                companies.Pais = myReader["Empresa_Pais"].ToString();
                                companies.Participacao = (myReader["Empresa_Participacao"].ToString() == "1") ? true : false;
                                companies.Percentual = Convert.ToDecimal(myReader["Empresa_PercParticipacao"].ToString());
                                companies.ControleOperacional = (myReader["Empresa_ControleOp"].ToString() == "1") ? true : false;

                                userList.Empresas.Add(companies);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "BuscarUsuarioEmpresas", ex.Message, string.Empty);
            }
        }

        public Boolean AtualizarUsuarioAprovacao(string Email)
        {
            Boolean retorno = true;

            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Atualizar_Usuario_Aprovacao", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("Email", Email);
                        varComm.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "AtualizarUsuarioAprovacao", ex.Message, string.Empty);
                retorno = false;
            }

            return retorno;
        }

        public Boolean BuscarCategorias(int Escopo, string categoriaModo, ref GetCategoriesResponse categoriesResponse)
        {
            Boolean retorno = true;

            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Buscar_Categoria", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("Escopo", Escopo);
                        varComm.Parameters.AddWithValue("Modo", categoriaModo);

                        using (SqlDataReader myReader = varComm.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (!myReader.HasRows) return false;

                            Classes.Json.Categories categoryList = new Categories();
                            categoriesResponse.categoryList = new List<Categories>();

                            while (myReader.Read())
                            {
                                categoryList = new Categories();
                                categoryList.categoryId = Convert.ToInt32(myReader["CategoriaID"].ToString());
                                categoryList.categoryName = myReader["CategoriaNome"].ToString();
                                categoryList.categoryMode = myReader["CategoriaModo"].ToString();
                                categoriesResponse.categoryList.Add(categoryList);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "BuscarCategorias", ex.Message, string.Empty);
                retorno = false;
            }

            return retorno;
        }

        public Boolean VerificarCategoriaExiste(string categoryName)
        {
            Boolean retorno = true;

            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Verificar_Categoria", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("CategoriaNome", categoryName);

                        using (SqlDataReader myReader = varComm.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            retorno = myReader.HasRows;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "VerificarCategoriaExiste", ex.Message, string.Empty);
                retorno = true;
            }

            return retorno;
        }

        public Boolean VerificarCategoriaExisteID(int CategoriaID)
        {
            Boolean retorno = true;

            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Verificar_Categoria_ID", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("CategoriaID", CategoriaID);

                        using (SqlDataReader myReader = varComm.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            retorno = myReader.HasRows;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "VerificarCategoriaExisteID", ex.Message, string.Empty);
                retorno = true;
            }

            return retorno;
        }

        public Boolean GravarCategoria(string categoriaNome, ref int categoriaID)
        {
            Boolean retorno = true;

            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Gravar_Categoria", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("CategoriaNome", categoriaNome);

                        using (SqlDataReader myReader = varComm.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (myReader.HasRows)
                            {
                                myReader.Read();

                                Int32.TryParse(myReader["CategoriaID"].ToString(), out categoriaID);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "GravarCategoria", ex.Message, string.Empty);
                retorno = false;
            }

            return retorno;
        }

        public Boolean GravarSubCategoria(int CategoryID, string SubCategoryName, int Scope, string Comments, ref int subCategoriaID)
        {
            Boolean retorno = true;

            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Gravar_SubCategoria", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("CategoriaID", CategoryID);
                        varComm.Parameters.AddWithValue("SubCategoriaNome", SubCategoryName);
                        varComm.Parameters.AddWithValue("SubCategoriaEscopo", Scope);
                        varComm.Parameters.AddWithValue("SubCategoriaObs", Comments);

                        using (SqlDataReader myReader = varComm.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (myReader.HasRows)
                            {
                                myReader.Read();

                                Int32.TryParse(myReader["SubCategoriaID"].ToString(), out subCategoriaID);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "GravarSubCategoria", ex.Message, string.Empty);
                retorno = false;
            }

            return retorno;
        }

        public Boolean VerificarSubCategoriaExiste(string SubCategoryName)
        {
            Boolean retorno = true;

            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Verificar_SubCategoria", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("SubCategoriaNome", SubCategoryName.ToUpper());

                        using (SqlDataReader myReader = varComm.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            retorno = myReader.HasRows;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "VerificarSubCategoriaExiste", ex.Message, string.Empty);
                retorno = true;
            }

            return retorno;
        }

        public Boolean BuscarSubCategorias(int Escopo, int CategoryId, ref GetSubCategoriesResponse subCategoriesResponse)
        {
            Boolean retorno = true;

            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Buscar_SubCategoria", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("Escopo", Escopo);
                        varComm.Parameters.AddWithValue("CategoriaId", CategoryId);

                        using (SqlDataReader myReader = varComm.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (!myReader.HasRows) return false;

                            Classes.Json.SubCategories subcategory = new SubCategories();
                            subCategoriesResponse.subcategoryList = new List<SubCategories>();

                            while (myReader.Read())
                            {
                                subcategory = new SubCategories();
                                subcategory.SubCategoryId = Convert.ToInt32(myReader["SubCategoriaID"].ToString());
                                subcategory.CategoryId = Convert.ToInt32(myReader["CategoriaID"].ToString());
                                subcategory.SubCategoryName = myReader["SubCategoriaNome"].ToString();
                                subcategory.Scope = Convert.ToInt32(myReader["SubCategoriaEscopo"].ToString());
                                subcategory.Comments = myReader["SubCategoriaObs"].ToString();
                                subCategoriesResponse.subcategoryList.Add(subcategory);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "BuscarSubCategorias", ex.Message, string.Empty);
                retorno = false;
            }

            return retorno;
        }

        public Boolean AtualizarDocumento(string RegistryID, string DocumentName, string DocumentType, string DocumentContentType, string DocumentExtension, int DocumentSize, byte[] DocumentImage, long CreatedByID, ref string documentID)
        {
            Boolean retorno = true;

            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Atualizar_Documento", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("docLancamentoID", RegistryID);
                        varComm.Parameters.AddWithValue("docNome", DocumentName);
                        varComm.Parameters.AddWithValue("docTipo", DocumentType);
                        varComm.Parameters.AddWithValue("docContentType", DocumentContentType);
                        varComm.Parameters.AddWithValue("docExtensao", DocumentExtension);
                        varComm.Parameters.AddWithValue("docTamanho", DocumentSize);
                        varComm.Parameters.AddWithValue("docImagem", DocumentImage);
                        varComm.Parameters.AddWithValue("docCriadoPor", CreatedByID);

                        documentID = (string)varComm.ExecuteScalar();

                        //using (SqlDataReader myReader = varComm.ExecuteReader(CommandBehavior.CloseConnection))
                        //{
                        //    retorno = myReader.HasRows;
                        //
                        //    if (myReader.HasRows)
                        //    {
                        //        myReader.Read();
                        //        documentID = myReader["DocumentoID"].ToString();
                        //    }
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "GravarDocumento", ex.Message, string.Empty);
                retorno = false;
            }

            return retorno;
        }

        public Boolean GravarDocumento(string DocumentName, string DocumentType, string DocumentContentType, string DocumentExtension, int DocumentSize, byte[] DocumentImage, long CreatedBy, ref string documentID)
        {
            Boolean retorno = true;

            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Gravar_Documento", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("docNome", DocumentName);
                        varComm.Parameters.AddWithValue("docTipo", DocumentType);
                        varComm.Parameters.AddWithValue("docContentType", DocumentContentType);
                        varComm.Parameters.AddWithValue("docExtensao", DocumentExtension);
                        varComm.Parameters.AddWithValue("docTamanho", DocumentSize);
                        varComm.Parameters.AddWithValue("docImagem", DocumentImage);
                        varComm.Parameters.AddWithValue("docCriadoPor", CreatedBy);

                        using (SqlDataReader myReader = varComm.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            retorno = myReader.HasRows;

                            if (myReader.HasRows)
                            {
                                myReader.Read();
                                documentID = myReader["DocumentoID"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "GravarDocumento", ex.Message, string.Empty);
                retorno = false;
            }

            return retorno;
        }

        public Boolean AtualizarLancamento(string RegistryID, long CompanyID, string documentID, int CategoryID, int SubCategoryID, string Unit, decimal EntryValue, string DocumentType, string Comments, long CreatedByID, int SourceID, string GasID, string entryStatus)
        {
            Boolean retorno = true;

            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Atualizar_Lancamento", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("lancID", RegistryID);
                        varComm.Parameters.AddWithValue("lancCompanyID", CompanyID);
                        varComm.Parameters.AddWithValue("lancDocumentID", documentID);
                        varComm.Parameters.AddWithValue("lancCategoryID", CategoryID);
                        varComm.Parameters.AddWithValue("lancSubCategoryID", SubCategoryID);
                        varComm.Parameters.AddWithValue("lancUnit", Unit);
                        varComm.Parameters.AddWithValue("lancEntryValue", EntryValue);
                        varComm.Parameters.AddWithValue("lancDocumentType", DocumentType);
                        varComm.Parameters.AddWithValue("lancComments", Comments);
                        varComm.Parameters.AddWithValue("lancCreatedByID", CreatedByID);
                        varComm.Parameters.AddWithValue("lancStatus", entryStatus);
                        varComm.Parameters.AddWithValue("lancSourceID", SourceID);
                        varComm.Parameters.AddWithValue("lancGasID", GasID);
                        varComm.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "AtualizarLancamento", ex.Message, string.Empty);
                retorno = false;
            }

            return retorno;
        }

        public Boolean GravarLancamento(long CompanyID, string documentID, int CategoryID, int SubCategoryID, int SourceID, string Unit, decimal EntryValue
            , string DocumentType, string Comments, long CreatedByID, string entryStatus, int referenceYear, int referenceMonth
            , string GasID, ref string entryID)
        {
            Boolean retorno = true;

            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Gravar_Lancamento", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;

                        if (CompanyID > 0)
                            varComm.Parameters.AddWithValue("lancCompanyID", CompanyID);
                        
                        if (CategoryID > 0)
                            varComm.Parameters.AddWithValue("lancCategoryID", CategoryID);

                        if (SubCategoryID > 0)
                            varComm.Parameters.AddWithValue("lancSubCategoryID", SubCategoryID);

                        if (SourceID > 0)
                            varComm.Parameters.AddWithValue("lancSourceID", SourceID);

                        if (!string.IsNullOrEmpty(GasID))
                            varComm.Parameters.AddWithValue("lancGasID", GasID);

                        if (!string.IsNullOrEmpty(Unit))
                            varComm.Parameters.AddWithValue("lancUnit", Unit);

                        if (EntryValue > 0)
                            varComm.Parameters.AddWithValue("lancEntryValue", EntryValue);

                        if (!string.IsNullOrEmpty(DocumentType))
                            varComm.Parameters.AddWithValue("lancDocumentType", DocumentType);

                        if (!string.IsNullOrEmpty(Comments))
                            varComm.Parameters.AddWithValue("lancComments", Comments);

                        if (referenceYear > 0)
                            varComm.Parameters.AddWithValue("lancRefYear", referenceYear);
                        else
                            varComm.Parameters.AddWithValue("lancRefYear", 0);

                        if (referenceMonth > 0)
                            varComm.Parameters.AddWithValue("lancRefMonth", referenceMonth);
                        else
                            varComm.Parameters.AddWithValue("lancRefMonth", 0);

                        if (!string.IsNullOrEmpty(documentID))
                            varComm.Parameters.AddWithValue("lancDocumentID", documentID);

                        varComm.Parameters.AddWithValue("lancCreatedByID", CreatedByID);
                        varComm.Parameters.AddWithValue("lancStatus", entryStatus);                   

                        System.Guid lancamento = (Guid)varComm.ExecuteScalar();
                        entryID = lancamento.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "GravarLancamento", ex.Message, string.Empty);
                retorno = false;
            }

            return retorno;
        }

        public Boolean DesativarLancamento(string EntryID, long UserID)
        {
            Boolean retorno = true;

            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Desativar_Lancamento", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("lancEntryID", EntryID);
                        varComm.Parameters.AddWithValue("lancUserID", UserID);
                        varComm.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "DesativarLancamento", ex.Message, string.Empty);
                retorno = false;
            }

            return retorno;
        }

        public Boolean BuscarLancamentos(string filtroEmpresa, string filtroCategoria, string filtroSubCategoria, string filtroUnidade, string filtroMes, string filtroAno, string filtroRegistroStatus
                    , string filtroNomeDocumento, long usuarioID, ref Classes.Json.ListRegistriesResponse listResponse, int page, int pageItems, string ordenacao)
        {
            Boolean retorno = true;

            try
            {
                string query = string.Empty;
                query = "SELECT DISTINCT";
                query += "    lanc.ID AS EntryID ";
	            query += "    , lanc.EmpresaID AS EntryEmpresaID ";
	            query += "    , emp.RazaoSocial AS EntryEmpresaRazao ";
                query += "    , lanc.CategoriaID AS EntryCategoriaID ";
	            query += "    , (SELECT Nome FROM tbl_categoria WHERE ID = lanc.CategoriaID) AS EntryCategoriaNome ";
	            query += "    , (SELECT COUNT(ID) FROM tbl_lancamento WHERE Ativo = 1 AND CategoriaID = lanc.CategoriaID AND EmpresaID = lanc.EmpresaID) AS EntryCategoriaQtde ";
                query += "    , lanc.SubCategoriaID AS EntrySubCategoriaID ";
	            query += "    , (SELECT Nome FROM tbl_subcategoria WHERE ID = lanc.SubCategoriaID) AS EntrySubCategoriaNome ";
                query += "    , lanc.UnidadeMedida AS EntryUnidade ";
	            query += "    , lanc.Valor AS EntryValor ";
	            query += "    , lanc.TipoDocumento AS EntryTipoDoc ";
	            query += "    , lanc.Comentarios AS EntryComentarios ";
	            query += "    , lanc.StatusRegistro AS EntryStatusReg ";
	            query += "    , lanc.RegistradoPor AS EntryRegistradoPor ";
	            query += "    , (SELECT CONCAT(Nome, ' ', Sobrenome) FROM tbl_usuario WHERE ID = lanc.RegistradoPor) AS EntryRegistradoPorNome ";
                query += "    , FORMAT(lanc.DataRegistro, 'dd/MM/yyyy') AS EntryDataReg ";
                query += "    , COALESCE(lanc.MesReferencia, 0) AS EntryMesRef ";
                query += "    , COALESCE(lanc.AnoReferencia, 0) AS EntryAnoRef ";
                query += "    , lanc.MesReferencia ";
                query += "    , lanc.AnoReferencia ";
                query += "    , lanc.FonteID AS EntrySourceID ";
                query += "    , (SELECT Nome FROM tbl_fonte WHERE ID = lanc.FonteID) AS EntrySourceName ";
                query += "    , (SELECT Combustivel FROM tbl_fonte WHERE ID = lanc.FonteID) AS EntryFuelName ";
                query += "    , COALESCE((SELECT [calc_tco2e] FROM tbl_emissao_calc WHERE LancamentoID = lanc.ID), 0) AS ResultTco2e ";
                query += "FROM ";
                query += "    tbl_lancamento lanc ";                
                query += "    LEFT OUTER JOIN tbl_lancamento_arquivo larq ON larq.LancamentoID = lanc.ID ";
                query += "    LEFT OUTER JOIN tbl_arquivo arq ON larq.ArquivoID = arq.ID ";
                query += "    INNER JOIN tbl_empresa emp ON lanc.EmpresaID = emp.ID ";
                query += "WHERE ";
                query += "    lanc.Ativo = 1 ";
                query += "    AND emp.Ativo = 1 ";
                query += "    AND lanc.EmpresaID IN (SELECT empu.EmpresaID FROM tbl_empresa_usuario empu INNER JOIN tbl_empresa emp2 ON empu.EmpresaID = emp2.ID WHERE emp2.Ativo = 1 AND empu.UsuarioID = " + usuarioID + ") ";

                if (!string.IsNullOrEmpty(filtroNomeDocumento))
                {
                    query += "AND arq.Nome LIKE '%" + filtroNomeDocumento + "%' ";
                }

                if (!string.IsNullOrEmpty(filtroRegistroStatus))
                {
                    query += "AND lanc.StatusRegistro IN (";

                    string[] registros = filtroRegistroStatus.Split(',');
                    foreach (string registro in registros)
                    {
                        query += string.Concat("'", registro, "',");
                    }

                    if (query.EndsWith(","))
                        query = query.Remove(query.Length - 1, 1);

                    query += ") ";
                }

                if (!string.IsNullOrEmpty(filtroMes))
                {
                    query += "AND lanc.MesReferencia IN (";

                    string[] registros = filtroMes.Split(',');
                    foreach (string registro in registros)
                    {
                        query += string.Concat(registro, ",");
                    }

                    if (query.EndsWith(","))
                        query = query.Remove(query.Length - 1, 1);

                    query += ") ";
                }

                if (!string.IsNullOrEmpty(filtroAno))
                {
                    query += "AND lanc.AnoReferencia IN (";

                    string[] registros = filtroAno.Split(',');
                    foreach (string registro in registros)
                    {
                        query += string.Concat(registro, ",");
                    }

                    if (query.EndsWith(","))
                        query = query.Remove(query.Length - 1, 1);

                    query += ") ";
                }

                if (!string.IsNullOrEmpty(filtroUnidade))
                {
                    query += "AND lanc.UnidadeMedida IN (";

                    string[] registros = filtroUnidade.Split(',');
                    foreach (string registro in registros)
                    {
                        query += string.Concat("'", registro, "',");
                    }

                    if (query.EndsWith(","))
                        query = query.Remove(query.Length - 1, 1);

                    query += ") ";
                }

                if (!string.IsNullOrEmpty(filtroSubCategoria))
                {
                    query += "AND lanc.SubCategoriaID IN (";

                    string[] registros = filtroSubCategoria.Split(',');
                    foreach (string registro in registros)
                    {
                        if (registro != "0")
                            query += string.Concat(registro, ",");
                    }

                    if (query.EndsWith(","))
                        query = query.Remove(query.Length - 1, 1);

                    query += ") ";
                }

                if (!string.IsNullOrEmpty(filtroCategoria))
                {
                    query += "AND lanc.CategoriaID IN (";

                    string[] registros = filtroCategoria.Split(',');
                    foreach (string registro in registros)
                    {
                        if (registro != "0")
                            query += string.Concat(registro, ",");
                    }

                    if (query.EndsWith(","))
                        query = query.Remove(query.Length - 1, 1);

                    query += ") ";
                }

                if (!string.IsNullOrEmpty(filtroEmpresa))
                {
                    query += "AND lanc.EmpresaID IN (";

                    string[] registros = filtroEmpresa.Split(',');
                    foreach (string registro in registros)
                    {
                        if (registro != "0")
                            query += string.Concat(registro, ",");
                    }

                    if (query.EndsWith(","))
                        query = query.Remove(query.Length - 1, 1);

                    query += ") ";
                }

                if (!string.IsNullOrEmpty(ordenacao))
                {
                    query += " ORDER BY " + ordenacao + " ";
                }
                else
                {
                    query += " ORDER BY emp.RazaoSocial ";
                }
                
                query += " OFFSET (" + page + " - 1) * " + pageItems + " ROWS FETCH NEXT " + pageItems + " ROWS ONLY";

                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand(query, varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.Text;

                        using (SqlDataReader myReader = varComm.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            while (myReader.Read())
                            {
                                RegistriesResponseCollection item = new RegistriesResponseCollection();
                                item.RegistryID = myReader["EntryID"].ToString();
                                item.CompanyID = Convert.ToInt64(myReader["EntryEmpresaID"].ToString());
                                item.CompanyName = myReader["EntryEmpresaRazao"].ToString();
                                item.CategoryEntryCount = Convert.ToInt64(myReader["EntryCategoriaQtde"].ToString());

                                item.CategoryID = 0;
                                if (!string.IsNullOrEmpty(myReader["EntryCategoriaID"].ToString()))
                                    item.CategoryID = Convert.ToInt32(myReader["EntryCategoriaID"].ToString());

                                item.CategoryName = string.Empty;
                                if (!string.IsNullOrEmpty(myReader["EntryCategoriaNome"].ToString()))
                                    item.CategoryName = myReader["EntryCategoriaNome"].ToString();

                                item.SubCategoryID = 0;
                                if (!string.IsNullOrEmpty(myReader["EntrySubCategoriaID"].ToString()))
                                    item.SubCategoryID = Convert.ToInt32(myReader["EntrySubCategoriaID"].ToString());

                                item.SubCategoryName = string.Empty;
                                if (!string.IsNullOrEmpty(myReader["EntrySubCategoriaNome"].ToString()))
                                    item.SubCategoryName = myReader["EntrySubCategoriaNome"].ToString();

                                item.Unity = string.Empty;
                                if (!string.IsNullOrEmpty(myReader["EntryUnidade"].ToString()))
                                    item.Unity = myReader["EntryUnidade"].ToString();

                                item.UnitValue = 0;
                                if (!string.IsNullOrEmpty(myReader["EntryValor"].ToString()))
                                    item.UnitValue = Convert.ToDecimal(myReader["EntryValor"].ToString());

                                //item.Comments = string.Empty;
                                //if (!string.IsNullOrEmpty(myReader["EntryComentarios"].ToString()))
                                //    item.Comments = myReader["EntryComentarios"].ToString();

                                item.SourceID = string.Empty;
                                if (!string.IsNullOrEmpty(myReader["EntrySourceID"].ToString()))
                                    item.SourceID = myReader["EntrySourceID"].ToString();

                                item.SourceName = string.Empty;
                                if (!string.IsNullOrEmpty(myReader["EntrySourceName"].ToString()))
                                    item.SourceName = myReader["EntrySourceName"].ToString();

                                item.FuelName = string.Empty;
                                if (!string.IsNullOrEmpty(myReader["EntryFuelName"].ToString()))
                                    item.FuelName = myReader["EntryFuelName"].ToString();

                                item.RegistryStatus = myReader["EntryStatusReg"].ToString();
                                item.RegistryDate = Convert.ToDateTime(myReader["EntryDataReg"].ToString());
                                item.RegisteredBy = myReader["EntryRegistradoPorNome"].ToString();
                                item.ReferredMonth = Convert.ToInt32(myReader["EntryMesRef"].ToString());
                                item.ReferredYear = Convert.ToInt32(myReader["EntryAnoRef"].ToString());

                                item.ResultTco2e = myReader.GetDouble(myReader.GetOrdinal("ResultTco2e"));

                                item.DocumentList = new List<RegistriesResponseCollection.Documents>();
                                BuscarListRegistriesDocuments(myReader["EntryID"].ToString(), ref item);

                                item.Comments = new List<RegistriesResponseCollection.RegistryComments>();
                                DataTable dataComentario = new DataTable();
                                BuscarRegistroComentarios(myReader["EntryID"].ToString(), ref dataComentario);

                                foreach (DataRow row in dataComentario.Rows)
                                {
                                    RegistriesResponseCollection.RegistryComments comment = new RegistriesResponseCollection.RegistryComments();
                                    comment.CommentID = row["CommentID"].ToString();
                                    comment.Comment = row["Comment"].ToString();
                                    comment.UserName = row["CommentCreatedBy"].ToString();
                                    comment.CreatedAt = row["CommentCreatedAt"].ToString();
                                    item.Comments.Add(comment);
                                }

                                item.CustomFields = new List<RegistriesResponseCollection.RegistryCustomFields>();
                                DataTable dataTable = new DataTable();
                                BuscarRegistryCustomFields(myReader["EntryID"].ToString(), ref dataTable);

                                foreach (DataRow row in dataTable.Rows)
                                {
                                    Decimal.TryParse(row["CustomField_Valor"].ToString(), out Decimal result);

                                    RegistriesResponseCollection.RegistryCustomFields field = new RegistriesResponseCollection.RegistryCustomFields();
                                    field.FieldName = row["CustomField_Campo"].ToString();
                                    field.FieldValue = result;

                                    item.CustomFields.Add(field);
                                }

                                listResponse.Registries.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "BuscarLancamentos", ex.Message, string.Empty);
                retorno = false;
            }

            return retorno;
        }

        public void BuscarRegistroComentarios(string EntryID, ref DataTable dataTable)
        {
            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Buscar_Lancamento_Comentario", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("LancamentoID", EntryID);

                        using (SqlDataReader myReader = varComm.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            dataTable.Load(myReader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "BuscarRegistroComentarios", ex.Message, string.Empty);
            }
        }

        public void BuscarRegistryCustomFields(string EntryID, ref DataTable dataTable)
        {
            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Buscar_CustomFields", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("LancamentoID", EntryID);

                        using (SqlDataReader myReader = varComm.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            dataTable.Load(myReader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "BuscarRegistryCustomFields", ex.Message, string.Empty);
            }
        }

        public void BuscarListRegistriesDocuments(string EntryID, ref RegistriesResponseCollection item)
        {
            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Buscar_Arquivo_LancamentoID", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("LancamentoID", EntryID);

                        using (SqlDataReader myReader = varComm.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            while (myReader.Read())
                            {
                                Classes.Json.RegistriesResponseCollection.Documents doc = new RegistriesResponseCollection.Documents();
                                doc.DocumentID = myReader["DocumentoID"].ToString();
                                doc.DocumentType = myReader["DocumentoTipo"].ToString();
                                doc.DocumentName = myReader["DocumentoNome"].ToString();
                                doc.DocumentContentType = myReader["DocumentoContentType"].ToString();
                                item.DocumentList.Add(doc);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "BuscarListRegistriesDocuments", ex.Message, string.Empty);
            }
        }


        public Boolean BuscarLancamentosByDoc(long UserId, string DocumentName, ref Classes.Json.ListRegistriesResponse listResponse)
        {
            Boolean retorno = true;

            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Buscar_Lancamento_Documento", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("UsuarioID", UserId);
                        varComm.Parameters.AddWithValue("Arquivo", DocumentName);

                        using (SqlDataReader myReader = varComm.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            while (myReader.Read())
                            {
                                RegistriesResponseCollection item = new RegistriesResponseCollection();
                                item.RegistryID = myReader["EntryID"].ToString();
                                item.CompanyID = Convert.ToInt64(myReader["EntryEmpresaID"].ToString());
                                item.CompanyName = myReader["EntryEmpresaRazao"].ToString();
                                item.CategoryID = Convert.ToInt32(myReader["EntryCategoriaID"].ToString());
                                item.CategoryName = myReader["EntryCategoriaNome"].ToString();
                                item.SubCategoryID = Convert.ToInt32(myReader["EntrySubCategoriaID"].ToString());
                                item.SubCategoryName = myReader["EntrySubCategoriaNome"].ToString();
                                item.Unity = myReader["EntryUnidade"].ToString();
                                item.UnitValue = Convert.ToDecimal(myReader["EntryValor"].ToString());
                                //item.Comments = myReader["EntryComentarios"].ToString();
                                item.RegistryStatus = myReader["EntryStatusReg"].ToString();
                                item.RegistryDate = Convert.ToDateTime(myReader["EntryDataReg"].ToString());
                                item.RegisteredBy = myReader["EntryRegistradoPorNome"].ToString();

                                item.DocumentList = new List<RegistriesResponseCollection.Documents>();
                                BuscarListRegistriesDocuments(myReader["EntryID"].ToString(), ref item);

                                listResponse.Registries.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "BuscarLancamentosByDoc", ex.Message, string.Empty);
                retorno = false;
            }

            return retorno;
        }

        public Boolean ValidarUnidade(string unidade)
        {
            Boolean retorno = true;

            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Buscar_Unidade", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("unidade", unidade);

                        using (SqlDataReader myReader = varComm.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            retorno = myReader.HasRows;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "ValidarUnidade", ex.Message, string.Empty);
                retorno = false;
            }

            return retorno;
        }

        public Boolean GravarEmissaoCalculo(string entryID)
        {
            Boolean retorno = true;

            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Gravar_Emissao_Calculo", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("lancamentoID", entryID);
                        varComm.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = "LancamentoID: " + entryID + Environment.NewLine;
                msg += "Msg: " + ex.Message;

                RegistrarErro("Server API", "Database.cs", "GravarEmissaoCalculo", msg, string.Empty);
                retorno = false;
            }

            return retorno;
        }

        public void GravarEmissaoCalculoExpressoes(string entryID)
        {
            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Gravar_EmissaoCalc_Expressao", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("LancamentoID", entryID);
                        varComm.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "GravarEmissaoCalculoExpressoes", ex.Message, string.Empty);
            }
        }

        public string BuscarNomeUsuario(string Email)
        {
            string retorno = Email;

            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Buscar_Usuario_Nome", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("email", Email);

                        using (SqlDataReader myReader = varComm.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (myReader.HasRows)
                            {
                                myReader.Read();
                                retorno = myReader["UsuarioNome"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "BuscarNomeUsuario", ex.Message, string.Empty);
            }

            return retorno;
        }

        public Boolean BuscarEmissaoCalculosTotaisAno(string usuarioID, string[] filtroEmpresaID, string[] filtroAno, string[] filtroMes, ref Json.InventoryResultsResponse resultsResponse)
        {
            Boolean retorno = true;

            try
            {
                string empresas = string.Empty;
                foreach (string empresa in filtroEmpresaID)
                {
                    empresas += string.Concat(empresa, ",");
                }

                string anos = string.Empty;
                foreach (string ano in filtroAno)
                {
                    anos += string.Concat(ano, ",");
                }

                string meses = string.Empty;
                foreach (string mes in filtroMes)
                {
                    meses += string.Concat(mes, ",");
                }

                if (empresas.EndsWith(","))
                    empresas = empresas.Remove(empresas.Length - 1);

                if (anos.EndsWith(","))
                    anos = anos.Remove(anos.Length - 1);

                if (meses.EndsWith(","))
                    meses = meses.Remove(meses.Length - 1);

                if (!string.IsNullOrEmpty(empresas))
                    usuarioID = string.Empty;

                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Buscar_EmissaoCalc_Totais_Ano", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("filtroUsuarioID", usuarioID);
                        varComm.Parameters.AddWithValue("filtroEmpresa", empresas);
                        varComm.Parameters.AddWithValue("filtroAno", anos);
                        varComm.Parameters.AddWithValue("filtroMes", meses);

                        using (SqlDataReader myReader = varComm.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (myReader.HasRows == false)
                            {
                                return false;
                            }

                            resultsResponse.TotalAno = new List<YearTotals>();

                            while (myReader.Read())
                            {
                                YearTotals yearTotals = new YearTotals();
                                //yearTotals.AnoReferencia = Convert.ToInt32(myReader["AnoReferencia"].ToString());
                                yearTotals.AnoReferencia = 0;
                                yearTotals.Tco2 = myReader.GetDecimal(myReader.GetOrdinal("tco2"));
                                yearTotals.Tch4 = myReader.GetDecimal(myReader.GetOrdinal("tch4"));
                                yearTotals.Tn2o = myReader.GetDecimal(myReader.GetOrdinal("tn2o"));
                                yearTotals.Tco2_Bio = myReader.GetDecimal(myReader.GetOrdinal("tco2_bio"));
                                yearTotals.Thfc = myReader.GetDecimal(myReader.GetOrdinal("thfc"));
                                yearTotals.Tpfc = myReader.GetDecimal(myReader.GetOrdinal("tpfc"));
                                yearTotals.Tsf6 = myReader.GetDecimal(myReader.GetOrdinal("tsf6"));
                                yearTotals.Tnf3 = myReader.GetDecimal(myReader.GetOrdinal("tnf3"));
                                yearTotals.Tco2e = myReader.GetDecimal(myReader.GetOrdinal("tco2e"));
                                resultsResponse.TotalAno.Add(yearTotals);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "BuscarEmissaoCalculosTotaisAno", ex.Message, string.Empty);
                retorno = false;
            }

            return retorno;
        }

        public Boolean BuscarEmissaoCalculosTotaisEscopo(string usuarioID, string[] filtroEmpresaID, string[] filtroAno, string[] filtroMes, ref Json.InventoryResultsResponse resultsResponse)
        {
            Boolean retorno = true;

            try
            {
                string empresas = string.Empty;
                foreach (string empresa in filtroEmpresaID)
                {
                    empresas += string.Concat(empresa, ",");
                }

                string anos = string.Empty;
                foreach (string ano in filtroAno)
                {
                    anos += string.Concat(ano, ",");
                }

                string meses = string.Empty;
                foreach (string mes in filtroMes)
                {
                    meses += string.Concat(mes, ",");
                }

                if (empresas.EndsWith(","))
                    empresas = empresas.Remove(empresas.Length - 1);

                if (anos.EndsWith(","))
                    anos = anos.Remove(anos.Length - 1);

                if (meses.EndsWith(","))
                    meses = meses.Remove(meses.Length - 1);

                if (!string.IsNullOrEmpty(empresas))
                    usuarioID = string.Empty;

                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Buscar_EmissaoCalc_Totais_Escopo", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("filtroUsuarioID", usuarioID);
                        varComm.Parameters.AddWithValue("filtroEmpresa", empresas);
                        varComm.Parameters.AddWithValue("filtroAno", anos);
                        varComm.Parameters.AddWithValue("filtroMes", meses);

                        using (SqlDataReader myReader = varComm.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (myReader.HasRows == false)
                            {
                                return false;
                            }

                            resultsResponse.TotalEscopo = new List<ScopeTotals>();

                            while (myReader.Read())
                            {
                                ScopeTotals scopeTotals = new ScopeTotals();
                                scopeTotals.Escopo = myReader["Escopo"].ToString();
                                //scopeTotals.AnoReferencia = Convert.ToInt32(myReader["AnoReferencia"].ToString());
                                scopeTotals.AnoReferencia = 0;
                                scopeTotals.Tco2 = Convert.ToDouble(myReader["tco2"].ToString());
                                scopeTotals.Tch4 = Convert.ToDouble(myReader["tch4"].ToString());
                                scopeTotals.Tn2o = Convert.ToDouble(myReader["tn2o"].ToString());
                                scopeTotals.Tco2_Bio = Convert.ToDouble(myReader["tco2_bio"].ToString());
                                scopeTotals.Thfc = Convert.ToDouble(myReader["thfc"].ToString());
                                scopeTotals.Tpfc = Convert.ToDouble(myReader["tpfc"].ToString());
                                scopeTotals.Tsf6 = Convert.ToDouble(myReader["tsf6"].ToString());
                                scopeTotals.Tnf3 = Convert.ToDouble(myReader["tnf3"].ToString());
                                scopeTotals.Tco2e = Convert.ToDouble(myReader["tco2e"].ToString());
                                resultsResponse.TotalEscopo.Add(scopeTotals);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "BuscarEmissaoCalculosTotaisEscopo", ex.Message, string.Empty);
                retorno = false;
            }

            return retorno;
        }

        public Boolean BuscarEmissaoCalculosTotais(string usuarioID, string[] filtroEmpresaID, string[] filtroAno, string[] filtroMes, ref Json.InventoryResultsResponse resultsResponse)
        {
            Boolean retorno = true;

            try
            {
                string empresas = string.Empty;
                foreach (string empresa in filtroEmpresaID)
                {
                    empresas += string.Concat(empresa, ",");
                }

                string anos = string.Empty;
                foreach (string ano in filtroAno)
                {
                    anos += string.Concat(ano, ",");
                }

                string meses = string.Empty;
                foreach (string mes in filtroMes)
                {
                    meses += string.Concat(mes, ",");
                }

                if (empresas.EndsWith(","))
                    empresas = empresas.Remove(empresas.Length - 1);

                if (anos.EndsWith(","))
                    anos = anos.Remove(anos.Length - 1);

                if (meses.EndsWith(","))
                    meses = meses.Remove(meses.Length - 1);

                if (!string.IsNullOrEmpty(empresas))
                    usuarioID = string.Empty;

                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Buscar_EmissaoCalc_Totais", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("filtroUsuarioID", usuarioID);
                        varComm.Parameters.AddWithValue("filtroEmpresa", empresas);
                        varComm.Parameters.AddWithValue("filtroAno", anos);
                        varComm.Parameters.AddWithValue("filtroMes", meses);

                        using (SqlDataReader myReader = varComm.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (myReader.HasRows == false)
                            {
                                return false;
                            }

                            resultsResponse.Totais = new List<InventoryResults>();

                            while (myReader.Read())
                            {
                                InventoryResults results = new InventoryResults();
                                results.Emissao = myReader["NomeEmissao"].ToString();
                                results.Escopo = myReader["Escopo"].ToString();
                                results.AnoReferencia = Convert.ToInt32(myReader["AnoReferencia"].ToString());
                                results.Tco2 = Convert.ToDouble(myReader["tco2"].ToString());
                                results.Tch4 = Convert.ToDouble(myReader["tch4"].ToString());
                                results.Tn2o = Convert.ToDouble(myReader["tn2o"].ToString());
                                results.Tco2_Bio = Convert.ToDouble(myReader["tco2_bio"].ToString());
                                results.Thfc = Convert.ToDouble(myReader["thfc"].ToString());
                                results.Tpfc = Convert.ToDouble(myReader["tpfc"].ToString());
                                results.Tsf6 = Convert.ToDouble(myReader["tsf6"].ToString());
                                results.Tnf3 = Convert.ToDouble(myReader["tnf3"].ToString());
                                results.Tco2e = Convert.ToDouble(myReader["tco2e"].ToString());
                                resultsResponse.Totais.Add(results);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "BuscarEmissaoCalculosTotais", ex.Message, string.Empty);
                retorno = false;
            }

            return retorno;
        }

        public Boolean GravarComentario(string entryID, string Comment, string UserName, DateTime CreatedAt)
        {
            Boolean retorno = true;

            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Gravar_Comentario", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("entryID", entryID);
                        varComm.Parameters.AddWithValue("comment", Comment);
                        varComm.Parameters.AddWithValue("userName", UserName);
                        varComm.Parameters.AddWithValue("createdAt", CreatedAt);
                        varComm.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "GravarComentario", ex.Message, string.Empty);
                retorno = false;
            }

            return retorno;
        }

        public Boolean ValidarCategoriaSubcategoria(int categoryID, int subCategoryID)
        {
            Boolean retorno = true;

            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Buscar_CategoriaSubCategoria", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("categoryID", categoryID);
                        varComm.Parameters.AddWithValue("subCategoryID", subCategoryID);
                        
                        using (SqlDataReader myReader = varComm.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            retorno = myReader.HasRows;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "ValidarCategoriaSubcategoria", ex.Message, string.Empty);
                retorno = false;
            }

            return retorno;
        }

        public Boolean BuscarFontes(ref Classes.Json.GetSourcesResponse sourcesResponse, int Escopo, int CategoriaID, int SubCategoriaID, int EmpresaID, string tipoDado)
        {
            Boolean retorno = true;

            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Buscar_Fonte", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("Escopo", Escopo);
                        varComm.Parameters.AddWithValue("CategoriaID", CategoriaID);
                        varComm.Parameters.AddWithValue("SubCategoriaID", SubCategoriaID);
                        varComm.Parameters.AddWithValue("EmpresaID", EmpresaID);
                        varComm.Parameters.AddWithValue("TipoDado", tipoDado);

                        using (SqlDataReader myReader = varComm.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            sourcesResponse.sources = new List<SourcesList>();

                            while (myReader.Read())
                            {
                                Classes.Json.SourcesList source = new SourcesList();
                                source.SourceID = Convert.ToInt32(myReader["SourceID"].ToString());
                                source.SourceName = myReader["SourceName"].ToString();
                                source.SourceScope = myReader["SourceScope"].ToString();
                                source.CategoryID = Convert.ToInt32(myReader["CategoryID"].ToString());
                                source.Category = myReader["Category"].ToString();
                                source.SubCategoryID = Convert.ToInt32(myReader["SubCategoryID"].ToString());
                                source.SubCategory = myReader["SubCategory"].ToString();
                                source.OperationalControl = myReader["OperationalControl"].ToString();
                                source.CountryID = myReader["CountryID"].ToString();
                                source.Country = myReader["Country"].ToString();
                                source.FuelName = myReader["FuelName"].ToString();
                                source.SourceType = myReader["SourceType"].ToString();
                                //source.pci_tj_gg = Convert.ToDouble(myReader["pci_tj_gg"].ToString());
                                //source.dens_kg_un = Convert.ToDouble(myReader["dens_kg_un"].ToString());
                                //source.biocomb_perc = Convert.ToDouble(myReader["biocomb_perc"].ToString());
                                //source.ef_tco2_t = Convert.ToDouble(myReader["ef_tco2_t"].ToString());
                                //source.ef_tco2_tj = Convert.ToDouble(myReader["ef_tco2_tj"].ToString());
                                //source.ef_tco2_mwh = Convert.ToDouble(myReader["ef_tco2_mwh"].ToString());
                                //source.ef_tco2_t_km = Convert.ToDouble(myReader["ef_tco2_t_km"].ToString());
                                //source.ef_tco2_pes_km = Convert.ToDouble(myReader["ef_tco2_pes_km"].ToString());
                                //source.ef_tco2_gj = Convert.ToDouble(myReader["ef_tco2_gj"].ToString());
                                //source.ef_tco2_t_tj = Convert.ToDouble(myReader["ef_tco2_t_tj"].ToString());
                                //source.ef_tch4_t = Convert.ToDouble(myReader["ef_tch4_t"].ToString());
                                //source.ef_tch4_tj = Convert.ToDouble(myReader["ef_tch4_tj"].ToString());
                                //source.ef_tch4_mwh = Convert.ToDouble(myReader["ef_tch4_mwh"].ToString());
                                //source.ef_tch4_papel_t = Convert.ToDouble(myReader["ef_tch4_papel_t"].ToString());
                                //source.ef_tch4_textil_t = Convert.ToDouble(myReader["ef_tch4_textil_t"].ToString());
                                //source.ef_tch4_alim_t = Convert.ToDouble(myReader["ef_tch4_alim_t"].ToString());
                                //source.ef_tch4_mad_t = Convert.ToDouble(myReader["ef_tch4_mad_t"].ToString());
                                //source.ef_tch4_jardim_t = Convert.ToDouble(myReader["ef_tch4_jardim_t"].ToString());
                                //source.ef_tch4_bor_cou_t = Convert.ToDouble(myReader["ef_tch4_bor_cou_t"].ToString());
                                //source.ef_tch4_lodo_t = Convert.ToDouble(myReader["ef_tch4_lodo_t"].ToString());
                                //source.ef_tch4_t_km = Convert.ToDouble(myReader["ef_tch4_t_km"].ToString());
                                //source.ef_tch4_pes_km = Convert.ToDouble(myReader["ef_tch4_pes_km"].ToString());
                                //source.ef_tch4_gj = Convert.ToDouble(myReader["ef_tch4_gj"].ToString());
                                //source.ef_tn2o_t = Convert.ToDouble(myReader["ef_tn2o_t"].ToString());
                                //source.ef_tn2o_tj = Convert.ToDouble(myReader["ef_tn2o_tj"].ToString());
                                //source.ef_tn2o_mwh = Convert.ToDouble(myReader["ef_tn2o_mwh"].ToString());
                                //source.ef_tn2o_t_km = Convert.ToDouble(myReader["ef_tn2o_t_km"].ToString());
                                //source.ef_tn2o_pes_km = Convert.ToDouble(myReader["ef_tn2o_pes_km"].ToString());
                                //source.ef_tn2o_n_kgn = Convert.ToDouble(myReader["ef_tn2o_n_kgn"].ToString());
                                //source.ef_tn2o_gj = Convert.ToDouble(myReader["ef_tn2o_gj"].ToString());
                                //source.ef_kgco2_usd = Convert.ToDouble(myReader["ef_kgco2_usd"].ToString());
                                //source.capacidade_t = Convert.ToDouble(myReader["capacidade_t"].ToString());
                                //source.vazamento_perc = Convert.ToDouble(myReader["vazamento_perc"].ToString());
                                //source.temp_med_ano = Convert.ToDouble(myReader["temp_med_ano"].ToString());
                                //source.pp_mm_ano = Convert.ToDouble(myReader["pp_mm_ano"].ToString());
                                //source.fraction = Convert.ToDouble(myReader["fraction"].ToString());
                                //source.gj_t = Convert.ToDouble(myReader["gj_t"].ToString());
                                //source.tc_gj = Convert.ToDouble(myReader["tc_gj"].ToString());
                                //source.odu_factor = Convert.ToDouble(myReader["odu_factor"].ToString());
                                //source.mcf = Convert.ToDouble(myReader["mcf"].ToString());
                                //source.kgch4_kgdbo = Convert.ToDouble(myReader["kgch4_kgdbo"].ToString());
                                //source.formula_tco2 = myReader["formula_tco2"].ToString();
                                //source.formula_tch4 = myReader["formula_tch4"].ToString();
                                //source.formula_tn2o = myReader["formula_tn2o"].ToString();
                                //source.formula_tco2_bio = myReader["formula_tco2_bio"].ToString();
                                //source.formula_thfc = myReader["formula_thfc"].ToString();
                                //source.formula_tpfc = myReader["formula_tpfc"].ToString();
                                //source.formula_tsf6 = myReader["formula_tsf6"].ToString();
                                //source.formula_tnf3 = myReader["formula_tnf3"].ToString();
                                //source.formula_tco2e = myReader["formula_tco2e"].ToString();
                                sourcesResponse.sources.Add(source);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "BuscarFontes", ex.Message, string.Empty);
                retorno = false;
            }

            return retorno;
        }

        public Boolean ValidarUsuarioPorNome(string UserName)
        {
            Boolean retorno = true;

            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Validar_Usuario_Nome", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("userName", UserName);

                        using (SqlDataReader myReader = varComm.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            retorno = myReader.HasRows;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "ValidarUsuarioPorNome", ex.Message, string.Empty);
                retorno = false;
            }

            return retorno;
        }

        public Boolean VerificarFatorExistenteID(string EmissaoID)
        {
            Boolean retorno = true;

            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Buscar_FatorEmissao_ID", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("EmissaoID", EmissaoID);

                        using (SqlDataReader myReader = varComm.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            retorno = myReader.HasRows;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "VerificarFatorExistenteID", ex.Message, string.Empty);
                retorno = true;
            }

            return retorno;
        }

        public Boolean VerificarFatorExistente(int CategoriaID, int Mes, int Ano)
        {
            Boolean retorno = true;

            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Buscar_FatorEmissao", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("CategoriaID", CategoriaID);
                        varComm.Parameters.AddWithValue("Mes", Mes);
                        varComm.Parameters.AddWithValue("Ano", Ano);

                        using (SqlDataReader myReader = varComm.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            retorno = myReader.HasRows;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "VerificarFatorExistente", ex.Message, string.Empty);
                retorno = true;
            }

            return retorno;
        }

        public Boolean GravarFatorEmissao(int CategoriaID, int Ano, int Mes, float Fator, long UsuarioID, ref string emissaoID)
        {
            Boolean retorno = true;

            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Gravar_FatorEmissao", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("CategoriaID", CategoriaID);
                        varComm.Parameters.AddWithValue("Mes", Mes);
                        varComm.Parameters.AddWithValue("Ano", Ano);
                        varComm.Parameters.AddWithValue("Fator", Fator);
                        varComm.Parameters.AddWithValue("UsuarioID", UsuarioID);

                        using (SqlDataReader myReader = varComm.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (myReader.HasRows)
                            {
                                myReader.Read();
                                emissaoID = myReader["NovoID"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "GravarFatorEmissao", ex.Message, string.Empty);
                retorno = true;
            }

            return retorno;
        }

        public Boolean AtualizarFatorEmissao(string EmissaoID, int CategoriaID, int Ano, int Mes, float Fator, long UsuarioID)
        {
            Boolean retorno = true;

            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Atualizar_FatorEmissao", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("EmissaoID", EmissaoID);
                        varComm.Parameters.AddWithValue("CategoriaID", CategoriaID);
                        varComm.Parameters.AddWithValue("Mes", Mes);
                        varComm.Parameters.AddWithValue("Ano", Ano);
                        varComm.Parameters.AddWithValue("Fator", Fator);
                        varComm.Parameters.AddWithValue("UsuarioID", UsuarioID);
                        varComm.ExecuteNonQuery();  
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "AtualizarFatorEmissao", ex.Message, string.Empty);
                retorno = true;
            }

            return retorno;
        }

        public Boolean DesativarFatorEmissao(string EmissaoID, long UsuarioID)
        {
            Boolean retorno = true;

            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Desativar_FatorEmissao", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("EmissaoID", EmissaoID);
                        varComm.Parameters.AddWithValue("UsuarioID", UsuarioID);
                        varComm.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "DesativarFatorEmissao", ex.Message, string.Empty);
                retorno = false;
            }

            return retorno;
        }

        public Boolean BuscarGases(ref Classes.Json.GetGasesResponse gasesResponse)
        {
            Boolean retorno = true;

            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Buscar_Gas_Lista", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;

                        using (SqlDataReader myReader = varComm.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (myReader.HasRows)
                            {
                                gasesResponse.Gases = new List<GetGasesResponse.GasesList>();

                                while (myReader.Read())
                                {
                                    Classes.Json.GetGasesResponse.GasesList gas = new GetGasesResponse.GasesList();
                                    gas.GasID = myReader["GasID"].ToString();
                                    gas.Gas = myReader["GasNome"].ToString();
                                    gas.GWP = myReader["GasGWP"].ToString();

                                    gasesResponse.Gases.Add(gas);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "BuscarGases", ex.Message, string.Empty);
                retorno = true;
            }

            return retorno;
        }

        public Boolean BuscarLancamentosDoc(string LancamentoID, ref Classes.Json.GetDocumentResponse documentResponse)
        {
            Boolean retorno = true;

            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Buscar_Documento", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("LancamentoID", LancamentoID);

                        using (SqlDataReader myReader = varComm.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (myReader.HasRows)
                            {
                                documentResponse.Documentos = new List<GetDocumentResponse.DocumentList>();

                                while (myReader.Read())
                                {
                                    Classes.Json.GetDocumentResponse.DocumentList doc = new GetDocumentResponse.DocumentList();
                                    doc.DocumentoID = myReader["DocumentoID"].ToString();
                                    doc.DocumentoNome = myReader["DocumentoNome"].ToString();
                                    doc.DocumentoTipo = myReader["DocumentoTipo"].ToString();
                                    doc.DocumentoContentType = myReader["DocumentoContentType"].ToString();
                                    doc.DocumentoExtensao = myReader["DocumentoExtensao"].ToString();
                                    doc.DocumentoTamanho = Convert.ToInt32(myReader["DocumentoTamanho"].ToString());

                                    byte[] docBytes = (byte[])myReader["DocumentoImagem"];

                                    doc.DocumentoImagem = Convert.ToBase64String(docBytes);
                                    documentResponse.Documentos.Add(doc);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "BuscarLancamentosDoc", ex.Message, string.Empty);
                retorno = true;
            }

            return retorno;
        }

        public Boolean BuscarLancamentoResultado(string RegistroID, ref Classes.Json.GetRegistryResultResponse resultResponse)
        {
            Boolean retorno = true;

            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Buscar_Lancamento_Resultado", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("LancamentoID", RegistroID);

                        using (SqlDataReader myReader = varComm.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (myReader.HasRows)
                            {
                                while (myReader.Read())
                                {
                                    resultResponse.RegistryId = myReader["LancamentoID"].ToString();
                                    resultResponse.ReferredYear = Convert.ToInt32(myReader["LancamentoAnoRef"].ToString());
                                    resultResponse.ReferredMonth = Convert.ToInt32(myReader["LancamentoMesRef"].ToString());
                                    resultResponse.SourceId = myReader["LancamentoFonteID"].ToString();
                                    resultResponse.SourceName = myReader["LancamentoFonte"].ToString();
                                    resultResponse.Quantity = Convert.ToDecimal(myReader["CalcInputQtde"].ToString());

                                    if (Convert.ToDecimal(myReader["ef_tco2_t"].ToString()) > 0)
                                        resultResponse.ef_tco2_t = Convert.ToDecimal(myReader["ef_tco2_t"].ToString());
                                    else
                                        resultResponse.ef_tco2_t = null;

                                    if (Convert.ToDecimal(myReader["ef_tco2_tj"].ToString()) > 0)
                                        resultResponse.ef_tco2_tj = Convert.ToDecimal(myReader["ef_tco2_tj"].ToString());
                                    else
                                        resultResponse.ef_tco2_tj = null;

                                    if (Convert.ToDecimal(myReader["ef_tco2_mwh"].ToString()) > 0)
                                        resultResponse.ef_tco2_mwh = Convert.ToDecimal(myReader["ef_tco2_mwh"].ToString());
                                    else
                                        resultResponse.ef_tco2_mwh = null;

                                    if (Convert.ToDecimal(myReader["ef_tco2_t_km"].ToString()) > 0)
                                        resultResponse.ef_tco2_t_km = Convert.ToDecimal(myReader["ef_tco2_t_km"].ToString());
                                    else
                                        resultResponse.ef_tco2_t_km = null;

                                    if (Convert.ToDecimal(myReader["ef_tco2_pes_km"].ToString()) > 0)
                                        resultResponse.ef_tco2_pes_km = Convert.ToDecimal(myReader["ef_tco2_pes_km"].ToString());
                                    else
                                        resultResponse.ef_tco2_pes_km = null;

                                    if (Convert.ToDecimal(myReader["ef_tco2_gj"].ToString()) > 0)
                                        resultResponse.ef_tco2_gj = Convert.ToDecimal(myReader["ef_tco2_gj"].ToString());
                                    else
                                        resultResponse.ef_tco2_gj = null;

                                    if (Convert.ToDecimal(myReader["ef_tco2_t_tj"].ToString()) > 0)
                                        resultResponse.ef_tco2_t_tj = Convert.ToDecimal(myReader["ef_tco2_t_tj"].ToString());
                                    else
                                        resultResponse.ef_tco2_t_tj = null;

                                    if (Convert.ToDecimal(myReader["ef_tch4_t"].ToString()) > 0)
                                        resultResponse.ef_tch4_t = Convert.ToDecimal(myReader["ef_tch4_t"].ToString());
                                    else
                                        resultResponse.ef_tch4_t = null;

                                    if (Convert.ToDecimal(myReader["ef_tch4_tj"].ToString())> 0)
                                        resultResponse.ef_tch4_tj = Convert.ToDecimal(myReader["ef_tch4_tj"].ToString());
                                    else
                                        resultResponse.ef_tch4_tj = null;

                                    if (Convert.ToDecimal(myReader["ef_tch4_mwh"].ToString()) > 0)
                                        resultResponse.ef_tch4_mwh = Convert.ToDecimal(myReader["ef_tch4_mwh"].ToString());
                                    else
                                        resultResponse.ef_tch4_mwh = null;

                                    if (Convert.ToDecimal(myReader["ef_tch4_papel_t"].ToString()) > 0)
                                        resultResponse.ef_tch4_papel_t = Convert.ToDecimal(myReader["ef_tch4_papel_t"].ToString());
                                    else
                                        resultResponse.ef_tch4_papel_t = null;

                                    if (Convert.ToDecimal(myReader["ef_tch4_textil_t"].ToString()) > 0)
                                        resultResponse.ef_tch4_textil_t = Convert.ToDecimal(myReader["ef_tch4_textil_t"].ToString());
                                    else
                                        resultResponse.ef_tch4_textil_t = null;

                                    if (Convert.ToDecimal(myReader["ef_tch4_alim_t"].ToString()) > 0)
                                        resultResponse.ef_tch4_alim_t = Convert.ToDecimal(myReader["ef_tch4_alim_t"].ToString());
                                    else
                                        resultResponse.ef_tch4_alim_t = null;

                                    if (Convert.ToDecimal(myReader["ef_tch4_mad_t"].ToString()) > 0)
                                        resultResponse.ef_tch4_mad_t = Convert.ToDecimal(myReader["ef_tch4_mad_t"].ToString());
                                    else
                                        resultResponse.ef_tch4_mad_t = null;

                                    if (Convert.ToDecimal(myReader["ef_tch4_jardim_t"].ToString()) > 0)
                                        resultResponse.ef_tch4_jardim_t = Convert.ToDecimal(myReader["ef_tch4_jardim_t"].ToString());
                                    else
                                        resultResponse.ef_tch4_jardim_t = null;

                                    if (Convert.ToDecimal(myReader["ef_tch4_bor_cou_t"].ToString()) > 0)
                                        resultResponse.ef_tch4_bor_cou_t = Convert.ToDecimal(myReader["ef_tch4_bor_cou_t"].ToString());
                                    else
                                        resultResponse.ef_tch4_bor_cou_t = null;

                                    if (Convert.ToDecimal(myReader["ef_tch4_lodo_t"].ToString()) > 0)
                                        resultResponse.ef_tch4_lodo_t = Convert.ToDecimal(myReader["ef_tch4_lodo_t"].ToString());
                                    else
                                        resultResponse.ef_tch4_lodo_t = null;

                                    if (Convert.ToDecimal(myReader["ef_tch4_t_km"].ToString()) > 0)
                                        resultResponse.ef_tch4_t_km = Convert.ToDecimal(myReader["ef_tch4_t_km"].ToString());
                                    else
                                        resultResponse.ef_tch4_t_km = null;

                                    if (Convert.ToDecimal(myReader["ef_tch4_pes_km"].ToString()) > 0)
                                        resultResponse.ef_tch4_pes_km = Convert.ToDecimal(myReader["ef_tch4_pes_km"].ToString());
                                    else
                                        resultResponse.ef_tch4_pes_km = null;

                                    if (Convert.ToDecimal(myReader["ef_tch4_gj"].ToString()) > 0)
                                        resultResponse.ef_tch4_gj = Convert.ToDecimal(myReader["ef_tch4_gj"].ToString());
                                    else
                                        resultResponse.ef_tch4_gj = null;

                                    if (Convert.ToDecimal(myReader["ef_tn2o_t"].ToString()) > 0)
                                        resultResponse.ef_tn2o_t = Convert.ToDecimal(myReader["ef_tn2o_t"].ToString());
                                    else
                                        resultResponse.ef_tn2o_t = null;

                                    if (Convert.ToDecimal(myReader["ef_tn2o_tj"].ToString()) > 0)
                                        resultResponse.ef_tn2o_tj = Convert.ToDecimal(myReader["ef_tn2o_tj"].ToString());
                                    else
                                        resultResponse.ef_tn2o_tj = null;

                                    if (Convert.ToDecimal(myReader["ef_tn2o_mwh"].ToString()) > 0)
                                        resultResponse.ef_tn2o_mwh = Convert.ToDecimal(myReader["ef_tn2o_mwh"].ToString());
                                    else
                                        resultResponse.ef_tn2o_mwh = null;

                                    if (Convert.ToDecimal(myReader["ef_tn2o_t_km"].ToString()) > 0)
                                        resultResponse.ef_tn2o_t_km = Convert.ToDecimal(myReader["ef_tn2o_t_km"].ToString());
                                    else
                                        resultResponse.ef_tn2o_t_km = null;

                                    if (Convert.ToDecimal(myReader["ef_tn2o_pes_km"].ToString()) > 0)
                                        resultResponse.ef_tn2o_pes_km = Convert.ToDecimal(myReader["ef_tn2o_pes_km"].ToString());
                                    else
                                        resultResponse.ef_tn2o_pes_km = null;

                                    if (Convert.ToDecimal(myReader["ef_tn2o_n_kgn"].ToString()) > 0)
                                        resultResponse.ef_tn2o_n_kgn = Convert.ToDecimal(myReader["ef_tn2o_n_kgn"].ToString());
                                    else
                                        resultResponse.ef_tn2o_n_kgn = null;

                                    if (Convert.ToDecimal(myReader["ef_tn2o_gj"].ToString()) > 0)
                                        resultResponse.ef_tn2o_gj = Convert.ToDecimal(myReader["ef_tn2o_gj"].ToString());
                                    else
                                        resultResponse.ef_tn2o_gj = null;

                                    if (Convert.ToDecimal(myReader["ef_kgco2_usd"].ToString()) > 0)
                                        resultResponse.ef_kgco2_usd = Convert.ToDecimal(myReader["ef_kgco2_usd"].ToString());
                                    else
                                        resultResponse.ef_kgco2_usd = null;

                                    if (Convert.ToDecimal(myReader["ef_tco2_tj_biocomb"].ToString()) > 0)
                                        resultResponse.ef_tco2_tj_biocomb = Convert.ToDecimal(myReader["ef_tco2_tj_biocomb"].ToString());
                                    else
                                        resultResponse.ef_tco2_tj_biocomb = null;

                                    if (Convert.ToDecimal(myReader["ef_tch4_tj_biocomb"].ToString()) > 0)
                                        resultResponse.ef_tch4_tj_biocomb = Convert.ToDecimal(myReader["ef_tch4_tj_biocomb"].ToString());
                                    else
                                        resultResponse.ef_tch4_tj_biocomb = null;

                                    if (Convert.ToDecimal(myReader["ef_tn2o_tj_biocomb"].ToString()) > 0)
                                        resultResponse.ef_tn2o_tj_biocomb = Convert.ToDecimal(myReader["ef_tn2o_tj_biocomb"].ToString());
                                    else
                                        resultResponse.ef_tn2o_tj_biocomb = null;

                                    if (Convert.ToDecimal(myReader["ef_tco2_l"].ToString()) > 0)
                                        resultResponse.ef_tco2_l = Convert.ToDecimal(myReader["ef_tco2_l"].ToString());
                                    else
                                        resultResponse.ef_tco2_l = null;

                                    if (Convert.ToDecimal(myReader["ef_tch4_l"].ToString()) > 0)
                                        resultResponse.ef_tch4_l = Convert.ToDecimal(myReader["ef_tch4_l"].ToString());
                                    else
                                        resultResponse.ef_tch4_l = null;

                                    if (Convert.ToDecimal(myReader["ef_tn2o_l"].ToString()) > 0)
                                        resultResponse.ef_tn2o_l = Convert.ToDecimal(myReader["ef_tn2o_l"].ToString());
                                    else
                                        resultResponse.ef_tn2o_l = null;

                                    if (Convert.ToDecimal(myReader["ef_tco2_l_biocomb"].ToString()) > 0)
                                        resultResponse.ef_tco2_l_biocomb = Convert.ToDecimal(myReader["ef_tco2_l_biocomb"].ToString());
                                    else
                                        resultResponse.ef_tco2_l_biocomb = null;

                                    if (Convert.ToDecimal(myReader["ef_tch4_l_biocomb"].ToString()) > 0)
                                        resultResponse.ef_tch4_l_biocomb = Convert.ToDecimal(myReader["ef_tch4_l_biocomb"].ToString());
                                    else
                                        resultResponse.ef_tch4_l_biocomb = null;

                                    if (Convert.ToDecimal(myReader["ef_tn2o_l_biocomb"].ToString()) > 0)
                                        resultResponse.ef_tn2o_l_biocomb = Convert.ToDecimal(myReader["ef_tn2o_l_biocomb"].ToString());
                                    else
                                        resultResponse.ef_tn2o_l_biocomb = null;

                                    if (Convert.ToDecimal(myReader["CalcResultTco2"].ToString()) > 0)
                                        resultResponse.CalcResultTco2 = Convert.ToDecimal(myReader["CalcResultTco2"].ToString());
                                    else
                                        resultResponse.CalcResultTco2 = null;

                                    if (Convert.ToDecimal(myReader["CalcResultTch4"].ToString()) > 0)
                                        resultResponse.CalcResultTch4 = Convert.ToDecimal(myReader["CalcResultTch4"].ToString());
                                    else
                                        resultResponse.CalcResultTch4 = null;

                                    if (Convert.ToDecimal(myReader["CalcResultTn2o"].ToString()) > 0)
                                        resultResponse.CalcResultTn2o = Convert.ToDecimal(myReader["CalcResultTn2o"].ToString());
                                    else
                                        resultResponse.CalcResultTn2o = null;

                                    if (Convert.ToDecimal(myReader["CalcResultTco2Bio"].ToString()) > 0)
                                        resultResponse.CalcResultTco2Bio = Convert.ToDecimal(myReader["CalcResultTco2Bio"].ToString());
                                    else
                                        resultResponse.CalcResultTco2Bio = null;

                                    if (Convert.ToDecimal(myReader["CalcResultThfc"].ToString()) > 0)
                                        resultResponse.CalcResultThfc = Convert.ToDecimal(myReader["CalcResultThfc"].ToString());
                                    else
                                        resultResponse.CalcResultThfc = null;

                                    if (Convert.ToDecimal(myReader["CalcResultTpfc"].ToString()) > 0)
                                        resultResponse.CalcResultTpfc = Convert.ToDecimal(myReader["CalcResultTpfc"].ToString());
                                    else
                                        resultResponse.CalcResultTpfc = null;

                                    if (Convert.ToDecimal(myReader["CalcResultTsf6"].ToString()) > 0)
                                        resultResponse.CalcResultTsf6 = Convert.ToDecimal(myReader["CalcResultTsf6"].ToString());
                                    else
                                        resultResponse.CalcResultTsf6 = null;

                                    if (Convert.ToDecimal(myReader["CalcResultTnf3"].ToString()) > 0)
                                        resultResponse.CalcResultTnf3 = Convert.ToDecimal(myReader["CalcResultTnf3"].ToString());
                                    else
                                        resultResponse.CalcResultTnf3 = null;

                                    if (Convert.ToDecimal(myReader["CalcResultTco2e"].ToString()) > 0)
                                        resultResponse.CalcResultTco2e = Convert.ToDecimal(myReader["CalcResultTco2e"].ToString());
                                    else
                                        resultResponse.CalcResultTco2e = null;

                                    if (Convert.ToDecimal(myReader["CalcResultTch4Biocomb"].ToString()) > 0)
                                        resultResponse.CalcResultTch4Biocomb = Convert.ToDecimal(myReader["CalcResultTch4Biocomb"].ToString());
                                    else
                                        resultResponse.CalcResultTch4Biocomb = null;

                                    if (Convert.ToDecimal(myReader["CalcResultTn2oBiocomb"].ToString()) > 0)
                                        resultResponse.CalcResultTn2oBiocomb = Convert.ToDecimal(myReader["CalcResultTn2oBiocomb"].ToString());
                                    else
                                        resultResponse.CalcResultTn2oBiocomb = null;


                                    if (!string.IsNullOrEmpty(myReader["formula_tco2"].ToString()))
                                        resultResponse.formula_tco2 = myReader["formula_tco2"].ToString();
                                    else
                                        resultResponse.formula_tco2 = null;

                                    if (!string.IsNullOrEmpty(myReader["formula_tch4"].ToString()))
                                        resultResponse.formula_tch4 = myReader["formula_tch4"].ToString();
                                    else
                                        resultResponse.formula_tch4 = null;

                                    if (!string.IsNullOrEmpty(myReader["formula_tch4_biocomb"].ToString()))
                                        resultResponse.formula_tch4_biocomb = myReader["formula_tch4_biocomb"].ToString();
                                    else
                                        resultResponse.formula_tch4_biocomb = null;

                                    if (!string.IsNullOrEmpty(myReader["formula_tn2o"].ToString()))
                                        resultResponse.formula_tn2o = myReader["formula_tn2o"].ToString();
                                    else
                                        resultResponse.formula_tn2o = null;

                                    if (!string.IsNullOrEmpty(myReader["formula_tn2o_biocomb"].ToString()))
                                        resultResponse.formula_tn2o_biocomb = myReader["formula_tn2o_biocomb"].ToString();
                                    else
                                        resultResponse.formula_tn2o_biocomb = null;

                                    if (!string.IsNullOrEmpty(myReader["formula_tco2_bio"].ToString()))
                                        resultResponse.formula_tco2_bio = myReader["formula_tco2_bio"].ToString();
                                    else
                                        resultResponse.formula_tco2_bio = null;

                                    if (!string.IsNullOrEmpty(myReader["formula_thfc"].ToString()))
                                        resultResponse.formula_thfc = myReader["formula_thfc"].ToString();
                                    else
                                        resultResponse.formula_thfc = null;

                                    if (!string.IsNullOrEmpty(myReader["formula_tpfc"].ToString()))
                                        resultResponse.formula_tpfc = myReader["formula_tpfc"].ToString();
                                    else
                                        resultResponse.formula_tpfc = null;

                                    if (!string.IsNullOrEmpty(myReader["formula_tsf6"].ToString()))
                                        resultResponse.formula_tsf6 = myReader["formula_tsf6"].ToString();
                                    else
                                        resultResponse.formula_tsf6 = null;

                                    if (!string.IsNullOrEmpty(myReader["formula_tnf3"].ToString()))
                                        resultResponse.formula_tnf3 = myReader["formula_tnf3"].ToString();
                                    else
                                        resultResponse.formula_tnf3 = null;

                                    if (!string.IsNullOrEmpty(myReader["formula_tco2e"].ToString()))
                                        resultResponse.formula_tco2e = myReader["formula_tco2e"].ToString();
                                    else
                                        resultResponse.formula_tco2e = null;                                
                                
                                    if (!string.IsNullOrEmpty(myReader["CalculoTco2"].ToString()))
                                        resultResponse.Calculo_tco2 = myReader["CalculoTco2"].ToString();
                                    else
                                        resultResponse.Calculo_tco2 = null;                                
                                
                                    if (!string.IsNullOrEmpty(myReader["CalculoTch4"].ToString()))
                                        resultResponse.Calculo_tch4 = myReader["CalculoTch4"].ToString();
                                    else
                                        resultResponse.Calculo_tch4 = null;

                                    if (!string.IsNullOrEmpty(myReader["CalculoTch4Biocomb"].ToString()))
                                        resultResponse.Calculo_tch4_biocomb = myReader["CalculoTch4Biocomb"].ToString();
                                    else
                                        resultResponse.Calculo_tch4_biocomb = null;

                                    if (!string.IsNullOrEmpty(myReader["CalculoTn2o"].ToString()))
                                        resultResponse.Calculo_tn2o = myReader["CalculoTn2o"].ToString();
                                    else
                                        resultResponse.Calculo_tn2o = null;

                                    if (!string.IsNullOrEmpty(myReader["CalculoTch4Biocomb"].ToString()))
                                        resultResponse.Calculo_tn2o_biocomb = myReader["CalculoTch4Biocomb"].ToString();
                                    else
                                        resultResponse.Calculo_tn2o_biocomb = null;

                                    if (!string.IsNullOrEmpty(myReader["CalculoTco2_bio"].ToString()))
                                        resultResponse.Calculo_tco2_bio = myReader["CalculoTco2_bio"].ToString();
                                    else
                                        resultResponse.Calculo_tco2_bio = null;                                
                                
                                    if (!string.IsNullOrEmpty(myReader["CalculoThfc"].ToString()))
                                        resultResponse.Calculo_thfc = myReader["CalculoThfc"].ToString();
                                    else
                                        resultResponse.Calculo_thfc = null;                                
                                
                                    if (!string.IsNullOrEmpty(myReader["CalculoTpfc"].ToString()))
                                        resultResponse.Calculo_tpfc = myReader["CalculoTpfc"].ToString();
                                    else
                                        resultResponse.Calculo_tpfc = null;                                
                                
                                    if (!string.IsNullOrEmpty(myReader["CalculoTsf6"].ToString()))
                                        resultResponse.Calculo_tsf6 = myReader["CalculoTsf6"].ToString();
                                    else
                                        resultResponse.Calculo_tsf6 = null;                                
                                
                                    if (!string.IsNullOrEmpty(myReader["CalculoTnf3"].ToString()))
                                        resultResponse.Calculo_tnf3 = myReader["CalculoTnf3"].ToString();
                                    else
                                        resultResponse.Calculo_tnf3 = null;                                
                                
                                    if (!string.IsNullOrEmpty(myReader["CalculoTco2e"].ToString()))
                                        resultResponse.Calculo_tco2e = myReader["CalculoTco2e"].ToString();
                                    else
                                        resultResponse.Calculo_tco2e = null;


                                    resultResponse.CustomFields = new List<GetRegistryResultResponse.RegistryCustomFields>();
                                    DataTable dataTable = new DataTable();
                                    BuscarRegistryCustomFields(RegistroID, ref dataTable);

                                    foreach (DataRow row in dataTable.Rows)
                                    {
                                        Decimal.TryParse(row["CustomField_Valor"].ToString(), out Decimal result);

                                        GetRegistryResultResponse.RegistryCustomFields field = new GetRegistryResultResponse.RegistryCustomFields();
                                        field.FieldName = row["CustomField_Campo"].ToString();
                                        field.FieldValue = result;

                                        resultResponse.CustomFields.Add(field);
                                    }

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "BuscarLancamentosDoc", ex.Message, string.Empty);
                retorno = true;
            }

            return retorno;
        }

        public void GravarLancamentoArquivo(string entryID, string documentID)
        {
            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Gravar_LancamentoArquivo", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("LancamentoID", entryID);
                        varComm.Parameters.AddWithValue("DocumentoID", documentID);
                        varComm.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "GravarLancamentoArquivo", ex.Message, string.Empty);
            }
        }

        public void AtualizarLancamentoCustomFields(string entryID, string FieldName, decimal Value)
        {
            try
            {
                if (FieldName.Contains("%"))
                    Value = Value / 100;

                if (FieldName == "input_papel_%")
                    FieldName = "ValorPapel";
                else if (FieldName == "input_textil_%")
                    FieldName = "ValorTextil";
                else if (FieldName == "input_alim_%")
                    FieldName = "ValorAlim";
                else if (FieldName == "input_mad_%")
                    FieldName = "ValorMad";
                else if (FieldName == "input_jardim_%")
                    FieldName = "ValorJardim";
                else if (FieldName == "input_bor_cou_%")
                    FieldName = "ValorBorCou";
                else if (FieldName == "input_lodo_%")
                    FieldName = "ValorLodo";
                else if (FieldName == "input_ano_frota")
                    FieldName = "ValorAnoFrota";
                else if (FieldName == "input_qtd_colab" || FieldName == "input_qnt_func")
                    FieldName = "ValorColab";
                else if (FieldName == "input_qtd_trecho")
                    FieldName = "ValorTrecho";
                else if (FieldName == "input_qtd_trecho")
                    FieldName = "ValorTrecho";
                else if (FieldName == "input_mwh" || FieldName == "input_qnt_elet")
                    FieldName = "ValorMwh";
                else if (FieldName == "input_tco2_mwh")
                    FieldName = "ValorTco2Mwh";
                else if (FieldName == "input_tch4_mwh")
                    FieldName = "ValorTch4Mwh";
                else if (FieldName == "input_tn2o_mwh")
                    FieldName = "ValorTn2oMwh";
                else if (FieldName == "input_peso_t")
                    FieldName = "ValorPesoT";
                else if (FieldName == "input_dist_km" || FieldName == "input_km_distance")
                    FieldName = "ValorDistKM";
                else if (FieldName == "input_vazao_m3")
                    FieldName = "ValorVazaoM3";
                else if (FieldName == "input_kgdbo_m3")
                    FieldName = "ValorKgdboM3";
                else if (FieldName == "input_gj")
                    FieldName = "ValorGj";
                else if (FieldName == "input_efic_ferv")
                    FieldName = "ValorEficFerv";
                else if (FieldName == "input_qnt_func")
                    FieldName = "ValorColab";
                else if (FieldName == "input_qnt_comb")
                    FieldName = "ValorQtdComb";
                else if (FieldName == "input_qnt_dias")
                    FieldName = "ValorDias";
                else if (FieldName == "input_tco2")
                    FieldName = "ValorTco2";
                else if (FieldName == "input_tch4")
                    FieldName = "ValorTch4";
                else if (FieldName == "input_tn2o")
                    FieldName = "ValorTn2o";
                else if (FieldName == "input_thfc")
                    FieldName = "ValorThfc";
                else if (FieldName == "input_tpfc")
                    FieldName = "ValorTpfc";
                else if (FieldName == "input_tsf6")
                    FieldName = "ValorTsf6";
                else if (FieldName == "input_tnf3" || FieldName == "input_tn3")
                    FieldName = "ValorTnf3";
                else if (FieldName == "input_tco2e")
                    FieldName = "ValorTco2e";
                else if (FieldName == "input_tco2_bio")
                    FieldName = "ValorTco2_bio";


                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Gravar_Lancamento_CustomField", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("LancamentoID", entryID);
                        varComm.Parameters.AddWithValue("Campo", FieldName);
                        varComm.Parameters.AddWithValue("Valor", Value);
                        varComm.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "AtualizarLancamentoCustomFields", ex.Message, string.Empty);
            }
        }

        public void GravarCustomFields(string entryID, string FieldName, decimal Value)
        {
            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Gravar_CustomField", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("LancamentoID", entryID);
                        varComm.Parameters.AddWithValue("Campo", FieldName);
                        varComm.Parameters.AddWithValue("Valor", Value);
                        varComm.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "GravarCustomFields", ex.Message, string.Empty);
            }
        }

        public void DesativarArquivo(string LancamentoID, string DocumentID)
        {
            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Desativar_Arquivo", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("LancamentoID", LancamentoID);
                        varComm.Parameters.AddWithValue("DocumentoID", DocumentID);
                        varComm.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "DesativarArquivo", ex.Message, string.Empty);
            }
        }

        public Boolean DesativarEmpresa(long EmpresaID)
        {
            Boolean retorno = true;

            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Desativar_Empresa", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("EmpresaID", EmpresaID);
                        varComm.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "DesativarEmpresa", ex.Message, string.Empty);
                retorno = false;
            }

            return retorno;
        }

        public Boolean ValidaEmpresaUsuario(long EmpresaID, long UsuarioID)
        {
            Boolean retorno = true;

            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Validar_Empresa_Usuario", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("EmpresaID", EmpresaID);
                        varComm.Parameters.AddWithValue("UsuarioID", UsuarioID);

                        using (SqlDataReader myReader = varComm.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            retorno = myReader.HasRows;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "ValidaEmpresaUsuario", ex.Message, string.Empty);
                retorno = false;
            }

            return retorno;
        }

        public Boolean AdicionarUsuarioEmpresa(long UserID, long CompanyID, ref string mensagem)
        {
            Boolean retorno = true;

            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Gravar_EmpresaUsuario", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("varEmpresaID", CompanyID);
                        varComm.Parameters.AddWithValue("varUsuarioID", UserID);
                        varComm.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "AdicionarUsuarioEmpresa", ex.Message, string.Empty);
                retorno = false;
                mensagem = ex.Message;
            }

            return retorno;
        }

        public Boolean DesativarUsuarioEmpresa(long UserID, long CompanyID, ref string mensagem)
        {
            Boolean retorno = true;

            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Desativar_EmpresaUsuario", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("varEmpresaID", CompanyID);
                        varComm.Parameters.AddWithValue("varUsuarioID", UserID);
                        varComm.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "DesativarUsuarioEmpresa", ex.Message, string.Empty);
                retorno = false;
                mensagem = ex.Message;
            }

            return retorno;
        }

        public Boolean VerificarLancamentosEmpresa(long CompanyID)
        {
            Boolean retorno = true;

            try
            {
                using (SqlConnection varConn = new SqlConnection(connString))
                {
                    varConn.Open();

                    using (SqlCommand varComm = new SqlCommand("usp_Buscar_Lancamento_Empresa", varConn))
                    {
                        varComm.CommandType = System.Data.CommandType.StoredProcedure;
                        varComm.Parameters.AddWithValue("varEmpresaID", CompanyID);

                        using (SqlDataReader myReader = varComm.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (!myReader.HasRows)
                                return false;

                            myReader.Read();
                            Int64.TryParse(myReader["TotalRegistros"].ToString(), out long registros);

                            if (registros <= 0) return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErro("Server API", "Database.cs", "VerificarLancamentosEmpresa", ex.Message, string.Empty);
                retorno = false;
            }

            return retorno;
        }


    }
}