using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace BlockC_Api.Classes.Json
{
    public class GetSourcesResponse
    {
        [JsonProperty("sources")]
        public List<SourcesList> sources { get; set; }
    }

    public partial class SourcesList
    {
		[JsonProperty("sourceId")]
        public int SourceID { get; set; }

        [JsonProperty("source")]
        public string SourceName { get; set; }

        [JsonProperty("categoryId")]
		public int CategoryID { get; set; }

        [JsonProperty("categoryName")]
        public string Category { get; set; }

        [JsonProperty("subcategoryId")]
        public int SubCategoryID { get; set; }

        [JsonProperty("subcategoryName")]
        public string SubCategory { get; set; }

        [JsonProperty("sourceScope")]
        public string SourceScope { get; set; }

        [JsonProperty("operationalControl")]
        public string OperationalControl { get; set; }

        [JsonProperty("countryId")]
        public string CountryID { get; set; }

        [JsonProperty("countryName")]
        public string Country { get; set; }

        [JsonProperty("fuelName")]
        public string FuelName { get; set; }


        //
        //
        //[JsonProperty("pci_tj_gg")]
        //public double pci_tj_gg { get; set; }
        //
        //[JsonProperty("dens_kg_un")]
        //public double dens_kg_un { get; set; }
        //
        //[JsonProperty("biocomb_perc")]
        //public double biocomb_perc { get; set; }
        //
        //[JsonProperty("ef_tco2_t")]
        //public double ef_tco2_t { get; set; }
        //
        //[JsonProperty("ef_tco2_tj")]
        //public double ef_tco2_tj { get; set; }
        //
        //[JsonProperty("ef_tco2_mwh")]
        //public double ef_tco2_mwh { get; set; }
        //
        //[JsonProperty("ef_tco2_t_km")]
        //public double ef_tco2_t_km { get; set; }
        //
        //[JsonProperty("ef_tco2_pes_km")]
        //public double ef_tco2_pes_km { get; set; }
        //
        //[JsonProperty("ef_tco2_gj")]
        //public double ef_tco2_gj { get; set; }
        //
        //[JsonProperty("ef_tco2_t_tj")]
        //public double ef_tco2_t_tj { get; set; }
        //
        //[JsonProperty("ef_tch4_t")]
        //public double ef_tch4_t { get; set; }
        //
        //[JsonProperty("ef_tch4_tj")]
        //public double ef_tch4_tj { get; set; }
        //
        //[JsonProperty("ef_tch4_mwh")]
        //public double ef_tch4_mwh { get; set; }
        //
        //[JsonProperty("ef_tch4_papel_t")]
        //public double ef_tch4_papel_t { get; set; }
        //
        //[JsonProperty("ef_tch4_textil_t")]
        //public double ef_tch4_textil_t { get; set; }
        //
        //[JsonProperty("ef_tch4_alim_t")]
        //public double ef_tch4_alim_t { get; set; }
        //
        //[JsonProperty("ef_tch4_mad_t")]
        //public double ef_tch4_mad_t { get; set; }
        //
        //[JsonProperty("ef_tch4_jardim_t")]
        //public double ef_tch4_jardim_t { get; set; }
        //
        //[JsonProperty("ef_tch4_bor_cou_t")]
        //public double ef_tch4_bor_cou_t { get; set; }
        //
        //[JsonProperty("ef_tch4_lodo_t")]
        //public double ef_tch4_lodo_t { get; set; }
        //
        //[JsonProperty("ef_tch4_t_km")]
        //public double ef_tch4_t_km { get; set; }
        //
        //[JsonProperty("ef_tch4_pes_km")]
        //public double ef_tch4_pes_km { get; set; }
        //
        //[JsonProperty("ef_tch4_gj")]
        //public double ef_tch4_gj { get; set; }
        //
        //[JsonProperty("ef_tn2o_t")]
        //public double ef_tn2o_t { get; set; }
        //
        //[JsonProperty("ef_tn2o_tj")]
        //public double ef_tn2o_tj { get; set; }
        //
        //[JsonProperty("ef_tn2o_mwh")]
        //public double ef_tn2o_mwh { get; set; }
        //
        //[JsonProperty("ef_tn2o_t_km")]
        //public double ef_tn2o_t_km { get; set; }
        //
        //[JsonProperty("ef_tn2o_pes_km")]
        //public double ef_tn2o_pes_km { get; set; }
        //
        //[JsonProperty("ef_tn2o_n_kgn")]
        //public double ef_tn2o_n_kgn { get; set; }
        //
        //[JsonProperty("ef_tn2o_gj")]
        //public double ef_tn2o_gj { get; set; }
        //
        //[JsonProperty("ef_kgco2_usd")]
        //public double ef_kgco2_usd { get; set; }
        //
        //[JsonProperty("capacidade_t")]
        //public double capacidade_t { get; set; }
        //
        //[JsonProperty("vazamento_perc")]
        //public double vazamento_perc { get; set; }
        //
        //[JsonProperty("temp_med_ano")]
        //public double temp_med_ano { get; set; }
        //
        //[JsonProperty("pp_mm_ano")]
        //public double pp_mm_ano { get; set; }
        //
        //[JsonProperty("fraction")]
        //public double fraction { get; set; }
        //
        //[JsonProperty("gj_t")]
        //public double gj_t { get; set; }
        //
        //[JsonProperty("tc_gj")]
        //public double tc_gj { get; set; }
        //
        //[JsonProperty("odu_factor")]
        //public double odu_factor { get; set; }
        //
        //[JsonProperty("mcf")]
        //public double mcf { get; set; }
        //
        //[JsonProperty("kgch4_kgdbo")]
        //public double kgch4_kgdbo { get; set; }
        //
        //[JsonProperty("formula_tco2")]
        //public string formula_tco2 { get; set; }
        //
        //[JsonProperty("formula_tch4")]
        //public string formula_tch4 { get; set; }
        //
        //[JsonProperty("formula_tn2o")]
        //public string formula_tn2o { get; set; }
        //
        //[JsonProperty("formula_tco2_bio")]
        //public string formula_tco2_bio { get; set; }
        //
        //[JsonProperty("formula_thfc")]
        //public string formula_thfc { get; set; }
        //
        //[JsonProperty("formula_tpfc")]
        //public string formula_tpfc { get; set; }
        //
        //[JsonProperty("formula_tsf6")]
        //public string formula_tsf6 { get; set; }
        //
        //[JsonProperty("formula_tnf3")]
        //public string formula_tnf3 { get; set; }
        //
        //[JsonProperty("formula_tco2e")]
        //public string formula_tco2e { get; set; }
    }

}