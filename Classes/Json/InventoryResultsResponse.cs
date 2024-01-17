using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockC_Api.Classes.Json
{
    public class InventoryResultsResponse
    {
        [JsonProperty("results")]
        public List<InventoryResults> Totais { get; set; }

        [JsonProperty("scopeTotals")]
        public List<ScopeTotals> TotalEscopo { get; set; }

        [JsonProperty("yearTotals")]
        public List<YearTotals> TotalAno { get; set; }
    }

    public partial class YearTotals
    {
        [JsonProperty("referenceYear")]
        public int AnoReferencia { get; set; }

        [JsonProperty("tco2")]
        public Decimal Tco2 { get; set; }

        [JsonProperty("tch4")]
        public Decimal Tch4 { get; set; }

        [JsonProperty("tn2o")]
        public Decimal Tn2o { get; set; }

        [JsonProperty("tco2_bio")]
        public Decimal Tco2_Bio { get; set; }

        [JsonProperty("thfc")]
        public Decimal Thfc { get; set; }

        [JsonProperty("tpfc")]
        public Decimal Tpfc { get; set; }

        [JsonProperty("tsf6")]
        public Decimal Tsf6 { get; set; }

        [JsonProperty("tnf3")]
        public Decimal Tnf3 { get; set; }

        [JsonProperty("tco2e")]
        public Decimal Tco2e { get; set; }
    }

    public partial class ScopeTotals
    {
        [JsonProperty("referenceYear")]
        public int AnoReferencia { get; set; }

        [JsonProperty("scope")]
        public string Escopo { get; set; }

        [JsonProperty("tco2")]
        public Double Tco2 { get; set; }

        [JsonProperty("tch4")]
        public Double Tch4 { get; set; }

        [JsonProperty("tn2o")]
        public Double Tn2o { get; set; }

        [JsonProperty("tco2_bio")]
        public Double Tco2_Bio { get; set; }

        [JsonProperty("thfc")]
        public Double Thfc { get; set; }

        [JsonProperty("tpfc")]
        public Double Tpfc { get; set; }

        [JsonProperty("tsf6")]
        public Double Tsf6 { get; set; }

        [JsonProperty("tnf3")]
        public Double Tnf3 { get; set; }

        [JsonProperty("tco2e")]
        public Double Tco2e { get; set; }
    }

    public partial class InventoryResults
    {
        [JsonProperty("referenceYear")]
        public int AnoReferencia { get; set; }

        [JsonProperty("emission")]
        public string Emissao { get; set; }

        [JsonProperty("scope")]
        public string Escopo { get; set; }

        [JsonProperty("tco2")]
        public Double Tco2 { get; set; }

        [JsonProperty("tch4")]
        public Double Tch4 { get; set; }

        [JsonProperty("tn2o")]
        public Double Tn2o { get; set; }

        [JsonProperty("tco2_bio")]
        public Double Tco2_Bio { get; set; }

        [JsonProperty("thfc")]
        public Double Thfc { get; set; }

        [JsonProperty("tpfc")]
        public Double Tpfc { get; set; }

        [JsonProperty("tsf6")]
        public Double Tsf6 { get; set; }

        [JsonProperty("tnf3")]
        public Double Tnf3 { get; set; }

        [JsonProperty("tco2e")]
        public Double Tco2e { get; set; }
    }

}