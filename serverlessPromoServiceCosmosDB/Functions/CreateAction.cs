using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using serverlessPromoServiceCosmosDB.Models;
using System.Net.Http;
using System.Text;
using System.Net;

namespace serverlessPromoServiceCosmosDB.Functions
{
    public class CreateAction
    {
        [FunctionName("CreateAction")]
        public static HttpResponseMessage Run(
            [HttpTrigger(AuthorizationLevel.Anonymous,"post", Route = "v1/promotions/{id}/actions")]HttpRequestMessage req,
            [CosmosDB(
                databaseName: "PromoDatabase",
                collectionName: "PromoCollection",
                ConnectionStringSetting = "CosmosDBConnection",
                Id ="{id}")

             ]
            ProductPromo productPromo,
            ILogger log)
        {

            ResponseObject responseobject = new ResponseObject();
            ProductPromoAction productPromoAction = new ProductPromoAction();
            var content = req.Content;
            string jsonContent = content.ReadAsStringAsync().Result;
            dynamic action = JsonConvert.DeserializeObject<ProductPromoAction>(jsonContent);



            try
            {
                productPromo.actions = productPromoAction;
                productPromoAction.type = action.type;
                productPromoAction.amount = action.amount;
                productPromoAction.quantity = action.quantity;
                productPromoAction.catalogId = action.catalogId;
                productPromoAction.productId = action.productId;



                responseobject.correlationalId = Guid.NewGuid().ToString();
                responseobject.statusCode = 201;
                responseobject.statusReason = "Created";
                responseobject.success = true;
                return new HttpResponseMessage(HttpStatusCode.Created)
                {
                    Content = new StringContent(responseobject.ToString(), Encoding.UTF8, "application/json")
                };

            }
            catch (Exception ex)
            {
                responseobject.correlationalId = Guid.NewGuid().ToString();
                responseobject.statusCode = 400;
                responseobject.statusReason = "Bad Request";
                responseobject.success = false;
                return new HttpResponseMessage(HttpStatusCode.Created)
                {
                    Content = new StringContent(responseobject.ToString(), Encoding.UTF8, "application/json")
                };
            }



        }

    }

}
