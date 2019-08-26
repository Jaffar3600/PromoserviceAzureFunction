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
using System.Net.Http;

using System.Linq;
using Microsoft.Azure.Documents.Client;

namespace serverlessPromoServiceCosmosDB.Functions
{
    public static class Retrivepromos
    {
        [FunctionName("retrivepromos")]
        public static async Task<IActionResult> Run(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/promotions")] HttpRequestMessage req, HttpRequest request,
           [CosmosDB(
                databaseName: "PromoDatabase",
                collectionName: "PromoCollection",
                ConnectionStringSetting = "CosmosDBConnection",
                SqlQuery = "select * from PromoCollection c where c.fromDate <= < udf.now()")
            ]IEnumerable<ProductPromo> productpromo,
           ILogger log)
        {
            ResponseObject responseobject = new ResponseObject();

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
        }
               
        
    }
}