using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using serverlessPromoServiceCosmosDB.Models;

namespace serverlessPromoServiceCosmosDB.Functions
{
    public static class Retrivepromos
    {
        /* [FunctionName("Retrivepromos")]
         public static async Task<IActionResult> Run(
             [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
             ILogger log)
         {
             log.LogInformation("C# HTTP trigger function processed a request.");

             string name = req.Query["name"];

             string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
             dynamic data = JsonConvert.DeserializeObject(requestBody);
             name = name ?? data?.name;

             return name != null
                 ? (ActionResult)new OkObjectResult($"Hello, {name}")
                 : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
         }*/


        [FunctionName("retrivepromos")]
        public static async Task<IActionResult> Run(
           [HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/promotions")] HttpRequest req,
           [CosmosDB(
                databaseName: "PromoDatabase",
                collectionName: "PromoCollection",
                ConnectionStringSetting = "CosmosDBConnection",
                SqlQuery = "select * from PromoCollection")
            ]IEnumerable<ProductPromo> productpromo,
           ILogger log)
        {
            ResponseObject responseobject = new ResponseObject();
            /* log.LogInformation($"Function triggered");

             if (productpromo == null)
             {
                 log.LogInformation($"No Todo items found");
             }
             else
             {
                 var ltodoitems = (List<ProductPromo>)productpromo;
                 if (ltodoitems.Count == 0)
                 {
                     log.LogInformation($"No Todo items found");
                 }
                 else
                 {
                     log.LogInformation($"{ltodoitems.Count} Todo items found");
                 }
             }*/
            try
            {
                responseobject.correlationalId = Guid.NewGuid().ToString();
                responseobject.statusCode = 200;
                responseobject.statusReason = "OK";
                responseobject.success = true;

                responseobject.promotions = productpromo;
                return new OkObjectResult(responseobject);

            }
            catch (Exception ex)
            {
                responseobject.correlationalId = Guid.NewGuid().ToString();
                responseobject.statusCode = 500;
                responseobject.statusReason = "Internal Server Error";
                responseobject.success = false;
                return new BadRequestObjectResult(responseobject);
            }
           // return new OkObjectResult(productpromo);
        }
    }
}
