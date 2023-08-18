using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockC_Api.Classes.Json
{
    public class GetRegistryResultResponse
    {
        [JsonProperty("registryId")]
        public string RegistryId { get; set; }

        [JsonProperty("referredYear")]
        public int ReferredYear { get; set; }

        [JsonProperty("referredMonth")]
        public int ReferredMonth { get; set; }

        [JsonProperty("sourceId")]
        public string SourceId { get; set; }

        [JsonProperty("sourceName")]
        public string SourceName { get; set; }

        [JsonProperty("quantity")]
        public decimal? Quantity { get; set; }

        [JsonProperty("ef_tco2_t", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? ef_tco2_t { get; set; }

        [JsonProperty("ef_tco2_tj", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? ef_tco2_tj { get; set; }

        [JsonProperty("ef_tco2_mwh", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? ef_tco2_mwh { get; set; }

        [JsonProperty("ef_tco2_t_km", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? ef_tco2_t_km { get; set; }

        [JsonProperty("ef_tco2_pes_km", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? ef_tco2_pes_km { get; set; }

        [JsonProperty("ef_tco2_gj", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? ef_tco2_gj { get; set; }

        [JsonProperty("ef_tco2_t_tj", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? ef_tco2_t_tj { get; set; }

        [JsonProperty("ef_tch4_t", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? ef_tch4_t { get; set; }

        [JsonProperty("ef_tch4_tj", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? ef_tch4_tj { get; set; }

        [JsonProperty("ef_tch4_mwh", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? ef_tch4_mwh { get; set; }

        [JsonProperty("ef_tch4_papel_t", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? ef_tch4_papel_t { get; set; }

        [JsonProperty("ef_tch4_textil_t", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? ef_tch4_textil_t { get; set; }

        [JsonProperty("ef_tch4_alim_t", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? ef_tch4_alim_t { get; set; }

        [JsonProperty("ef_tch4_mad_t", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? ef_tch4_mad_t { get; set; }

        [JsonProperty("ef_tch4_jardim_t", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? ef_tch4_jardim_t { get; set; }

        [JsonProperty("ef_tch4_bor_cou_t", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? ef_tch4_bor_cou_t { get; set; }

        [JsonProperty("ef_tch4_lodo_t", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? ef_tch4_lodo_t { get; set; }

        [JsonProperty("ef_tch4_t_km", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? ef_tch4_t_km { get; set; }

        [JsonProperty("ef_tch4_pes_km", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? ef_tch4_pes_km { get; set; }

        [JsonProperty("ef_tch4_gj", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? ef_tch4_gj { get; set; }

        [JsonProperty("ef_tn2o_t", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? ef_tn2o_t { get; set; }

        [JsonProperty("ef_tn2o_tj", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? ef_tn2o_tj { get; set; }

        [JsonProperty("ef_tn2o_mwh", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? ef_tn2o_mwh { get; set; }

        [JsonProperty("ef_tn2o_t_km", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? ef_tn2o_t_km { get; set; }

        [JsonProperty("ef_tn2o_pes_km", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? ef_tn2o_pes_km { get; set; }

        [JsonProperty("ef_tn2o_n_kgn", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? ef_tn2o_n_kgn { get; set; }

        [JsonProperty("ef_tn2o_gj", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? ef_tn2o_gj { get; set; }

        [JsonProperty("ef_kgco2_usd", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? ef_kgco2_usd { get; set; }

        [JsonProperty("ef_tco2_tj_biocomb", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? ef_tco2_tj_biocomb { get; set; }

        [JsonProperty("ef_tch4_tj_biocomb", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? ef_tch4_tj_biocomb { get; set; }

        [JsonProperty("ef_tn2o_tj_biocomb", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? ef_tn2o_tj_biocomb { get; set; }

        [JsonProperty("ef_tco2_l", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? ef_tco2_l { get; set; }

        [JsonProperty("ef_tch4_l", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? ef_tch4_l { get; set; }

        [JsonProperty("ef_tn2o_l", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? ef_tn2o_l { get; set; }

        [JsonProperty("ef_tco2_l_biocomb", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? ef_tco2_l_biocomb { get; set; }

        [JsonProperty("ef_tch4_l_biocomb", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? ef_tch4_l_biocomb { get; set; }

        [JsonProperty("ef_tn2o_l_biocomb", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? ef_tn2o_l_biocomb { get; set; }

        [JsonProperty("customFields", NullValueHandling = NullValueHandling.Ignore)]
        public List<RegistryCustomFields> CustomFields { get; set; }



        [JsonProperty("result_tco2", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? CalcResultTco2 { get; set; }

        [JsonProperty("result_tch4", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? CalcResultTch4 { get; set; }

        [JsonProperty("result_tch4_biocomb", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? CalcResultTch4Biocomb { get; set; }

        [JsonProperty("result_tn2o", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? CalcResultTn2o { get; set; }

        [JsonProperty("result_tn2o_biocomb", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? CalcResultTn2oBiocomb { get; set; }

        [JsonProperty("result_tco2bio", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? CalcResultTco2Bio { get; set; }

        [JsonProperty("result_thfc", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? CalcResultThfc { get; set; }

        [JsonProperty("result_tpfc", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? CalcResultTpfc { get; set; }

        [JsonProperty("result_tsf6", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? CalcResultTsf6 { get; set; }

        [JsonProperty("result_tnf3", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? CalcResultTnf3 { get; set; }

        [JsonProperty("result_tco2e", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? CalcResultTco2e { get; set; }



        [JsonProperty("formula_tco2", NullValueHandling = NullValueHandling.Ignore)]
        public string formula_tco2 { get; set; }

        [JsonProperty("formula_tch4", NullValueHandling = NullValueHandling.Ignore)]
        public string formula_tch4 { get; set; }

        [JsonProperty("formula_tn2o", NullValueHandling = NullValueHandling.Ignore)]
        public string formula_tn2o { get; set; }

        [JsonProperty("formula_tco2_bio", NullValueHandling = NullValueHandling.Ignore)]
        public string formula_tco2_bio { get; set; }

        [JsonProperty("formula_thfc", NullValueHandling = NullValueHandling.Ignore)]
        public string formula_thfc { get; set; }

        [JsonProperty("formula_tpfc", NullValueHandling = NullValueHandling.Ignore)]
        public string formula_tpfc { get; set; }

        [JsonProperty("formula_tsf6", NullValueHandling = NullValueHandling.Ignore)]
        public string formula_tsf6 { get; set; }

        [JsonProperty("formula_tnf3", NullValueHandling = NullValueHandling.Ignore)]
        public string formula_tnf3 { get; set; }

        [JsonProperty("formula_tco2e", NullValueHandling = NullValueHandling.Ignore)]
        public string formula_tco2e { get; set; }

        [JsonProperty("formula_tch4_biocomb", NullValueHandling = NullValueHandling.Ignore)]
        public string formula_tch4_biocomb { get; set; }

        [JsonProperty("formula_tn2o_biocomb", NullValueHandling = NullValueHandling.Ignore)]
        public string formula_tn2o_biocomb { get; set; }


        [JsonProperty("calculation_tco2", NullValueHandling = NullValueHandling.Ignore)]
        public string Calculo_tco2 { get; set; }

        [JsonProperty("calculation_tch4", NullValueHandling = NullValueHandling.Ignore)]
        public string Calculo_tch4 { get; set; }

        [JsonProperty("calculation_tn2o", NullValueHandling = NullValueHandling.Ignore)]
        public string Calculo_tn2o { get; set; }

        [JsonProperty("calculation_tco2_bio", NullValueHandling = NullValueHandling.Ignore)]
        public string Calculo_tco2_bio { get; set; }

        [JsonProperty("calculation_thfc", NullValueHandling = NullValueHandling.Ignore)]
        public string Calculo_thfc { get; set; }

        [JsonProperty("calculation_tpfc", NullValueHandling = NullValueHandling.Ignore)]
        public string Calculo_tpfc { get; set; }

        [JsonProperty("calculation_tsf6", NullValueHandling = NullValueHandling.Ignore)]
        public string Calculo_tsf6 { get; set; }

        [JsonProperty("calculation_tnf3", NullValueHandling = NullValueHandling.Ignore)]
        public string Calculo_tnf3 { get; set; }

        [JsonProperty("calculation_tco2e", NullValueHandling = NullValueHandling.Ignore)]
        public string Calculo_tco2e { get; set; }

        [JsonProperty("calculation_tch4_biocomb", NullValueHandling = NullValueHandling.Ignore)]
        public string Calculo_tch4_biocomb { get; set; }

        [JsonProperty("calculation_tn2o_biocomb", NullValueHandling = NullValueHandling.Ignore)]
        public string Calculo_tn2o_biocomb { get; set; }






        public partial class RegistryCustomFields
        {
            [JsonProperty("fieldName", NullValueHandling = NullValueHandling.Ignore)]
            public string FieldName { get; set; }

            [JsonProperty("fieldValue", NullValueHandling = NullValueHandling.Ignore)]
            public decimal FieldValue { get; set; }
        }


    }
}