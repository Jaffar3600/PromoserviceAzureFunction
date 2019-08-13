using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace serverlessPromoServiceCosmosDB.Models
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]

    public class ResponseObject
    {
        [JsonProperty(PropertyName = "correlationId")]

        public string correlationalId { get; set; }
        public int statusCode { get; set; }
        public string statusReason { get; set; }
        public bool success { get; set; }
        //  public string Id { get; set; }


       // public List<messages> messages { get; set; }
        // [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ProductPromo promotion { get; set; }


        // [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<ProductPromo> promotions { get; set; }


        // [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        // [JsonProperty(PropertyName = "promotionId")]
        public string promotionId { get; set; }



        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
