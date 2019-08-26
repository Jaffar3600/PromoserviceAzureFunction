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
using System.Linq;

namespace serverlessPromoServiceCosmosDB.Functions
{
    public static class GetAllConditions
    {
        [FunctionName("GetAllConditions")]
        public static async Task<IActionResult> Run(
             [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/promotions/{id}/conditions/all")] HttpRequestMessage req,
             [CosmosDB(
                databaseName: "PromoDatabase",
                collectionName: "PromoCollection",
                ConnectionStringSetting = "CosmosDBConnection",
                Id = "{id}")
            ]ProductPromo productpromo,
             ILogger log)
        {
            ResponseObject responseobject = new ResponseObject();
           // var headers = req.Headers;
       
                    try
                    {

                        responseobject.correlationalId = Guid.NewGuid().ToString();
                        responseobject.statusCode = 200;
                        responseobject.statusReason = "OK";
                        responseobject.success = true;
                        var result = productpromo.conditions;
                        responseobject.conditions = result;
                return new OkObjectResult(responseobject);
                       
            }
            catch (Exception ex)
                {
                    responseobject.correlationalId = Guid.NewGuid().ToString();
                    responseobject.statusCode = 500;
                    responseobject.statusReason = "Internal Server Error";
                    responseobject.success = false;
                    return new OkObjectResult(responseobject);
                }


            }
           
}
}