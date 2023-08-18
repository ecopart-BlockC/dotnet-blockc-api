﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockC_Api.Classes.Json
{
    public class GetUserCompanyResponse
    {
        [JsonProperty("companies")]
        public List<GetUserCompanyList> UsuarioEmpresas { get; set; }
    }

    public partial class GetUserCompanyList
    {
        [JsonProperty("companyId")]
        public long EmpresaID { get; set; }

        [JsonProperty("headquarterId")]
        public long MatrizID { get; set; }

        [JsonProperty("isHeadquarter")]
        public Boolean Matriz { get; set; }

        [JsonProperty("companyName")]
        public string RazaoSocial { get; set; }

        [JsonProperty("companyRegister")]
        public string CNPJ { get; set; }

        [JsonProperty("hasEmission")]
        public Boolean Emissao { get; set; }

        [JsonProperty("sector")]
        public string Setor { get; set; }

        [JsonProperty("city")]
        public string Cidade { get; set; }

        [JsonProperty("state")]
        public string UF { get; set; }

        [JsonProperty("country")]
        public string Pais { get; set; }

        [JsonProperty("equityAffiliate")]
        public Boolean Participacao { get; set; }

        [JsonProperty("equityPercentageOwned")]
        public decimal Percentual { get; set; }

        [JsonProperty("hasEquityControl")]
        public Boolean ControleOperacional { get; set; }

        [JsonProperty("users")]
        public List<CompanyUsers> Usuarios { get; set; }

        [JsonProperty("branches")]
        public List<BranchesList> Filiais { get; set; }
    }

    public partial class CompanyUsers
    {
        [JsonProperty("userId")]
        public long UsuarioID { get; set; }

        [JsonProperty("name")]
        public string UsuarioNome { get; set; }

        [JsonProperty("lastName")]
        public string UsuarioSobrenome { get; set; }
    }

    public partial class BranchesList
    {
        [JsonProperty("companyId")]
        public long EmpresaID { get; set; }

        [JsonProperty("headquarterId")]
        public long MatrizID { get; set; }

        [JsonProperty("isHeadquarter")]
        public Boolean Matriz { get; set; }

        [JsonProperty("companyName")]
        public string RazaoSocial { get; set; }

        [JsonProperty("companyRegister")]
        public string CNPJ { get; set; }

        [JsonProperty("hasEmission")]
        public Boolean Emissao { get; set; }

        [JsonProperty("sector")]
        public string Setor { get; set; }

        [JsonProperty("city")]
        public string Cidade { get; set; }

        [JsonProperty("state")]
        public string UF { get; set; }

        [JsonProperty("country")]
        public string Pais { get; set; }

        [JsonProperty("equityAffiliate")]
        public Boolean Participacao { get; set; }

        [JsonProperty("equityPercentageOwned")]
        public decimal Percentual { get; set; }

        [JsonProperty("hasEquityControl")]
        public Boolean ControleOperacional { get; set; }

        [JsonProperty("users")]
        public List<CompanyUsers> Usuarios { get; set; }
    }

}