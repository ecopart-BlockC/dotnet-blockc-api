using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BlockC_Api.Classes.Json
{
    public class GetTotalRegisterRequest
    {
        [Required]
        [JsonProperty("token")]
        public string Token { get; set; }

        [Required]
        [JsonProperty("companyId")]
        public long CompanyID { get; set; }

        [Required]
        [JsonProperty("userId")]
        public long UserID { get; set; }
    }
}