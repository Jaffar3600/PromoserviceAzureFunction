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
using System.Net;
using System.Text;

namespace serverlessPromoServiceCosmosDB.Funtions
{
    public class RetrivePromoById
    {
        [FunctionName("retrivepromo")]
        public static async Task<IActionResult> Run(
             [HttpTrigger(AuthorizationLevel.Function, "get", Route = "promotion/{id}")] HttpRequestMessage req,
             [CosmosDB(
                databaseName: "PromoDatabase",
                collectionName: "PromoCollection",
                ConnectionStringSetting = "CosmosDBConnection",
                Id = "{id}")
            ]ProductPromo productpromo,
             ILogger log)
        {
            ResponseObject responseobject = new ResponseObject();
            log.LogInformation($"Function triggered");

           /* if (productpromo == null)
            {
                log.LogInformation($"Item not found");
                return new NotFoundObjectResult("Id not found in collection");
            }
            else
            {
                //log.LogInformation($"Found ToDo item {productpromo.Description}");
                return new OkObjectResult(productpromo);
            }*/
            try
            {
               // productpromo = result;
                responseobject.correlationalId = Guid.NewGuid().ToString();
                responseobject.statusCode = 200;
                responseobject.statusReason = "OK";
                responseobject.success = true;

                responseobject.promotion = productpromo;
                return new OkObjectResult(responseobject)
               /* {
                    //StatusCode = new StringContent(responseobject.ToString(), Encoding.UTF8, "application/json")
                }*/;



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
