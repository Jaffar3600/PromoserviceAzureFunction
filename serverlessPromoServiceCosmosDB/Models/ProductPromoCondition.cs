using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace serverlessPromoServiceCosmosDB.Models
{
    public class ProductPromoCondition
    {
        public ProductPromoParameter parameter { get; set; }

        public ProductPromoOperator promoOperator { get; set; }

        public float conditionValue { get; set; }

        public float otherValue { get; set; }

        public override string ToString()
        {
            // return JsonConvert.DeserializeObject(this);
            return JsonConvert.SerializeObject(this);
        }
    }
}
