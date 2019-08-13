using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace serverlessPromoServiceCosmosDB.Models
{
    public class ProductPromoAction
    {
        public ProductPromoActionType type { get; set; }
        public decimal quantity { get; set; }
        public decimal amount { get; set; }

        public string productId { get; set; }

        public string catalogId { get; set; }



        public override string ToString()
        {
            // return JsonConvert.DeserializeObject(this);
            return JsonConvert.SerializeObject(this);
        }
    }
}
