using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using serverlessPromoServiceCosmosDB.Models;

namespace serverlessPromoServiceCosmosDB.Functions
{
    public static class DeletePromoById
    {
       // private static string id;

        [FunctionName("deletepromo")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "promotion/{id}")] HttpRequest req,
            [CosmosDB(
                databaseName: "PromoDatabase",
                collectionName: "PromoCollection",
                ConnectionStringSetting = "CosmosDBConnection",
           

            Id = "{id}") ]
             ProductPromo productpromo,


            ILogger log)
             
           
        {
            ResponseObject responseobject = new ResponseObject();
           
           
            try
            {
                // productpromo = result;
                responseobject.correlationalId = Guid.NewGuid().ToString();
                responseobject.statusCode = 202;
                responseobject.statusReason = "Accepted";
                responseobject.success = true;
                // var result = productpromo;
                string id = req.Query["id"];
               // = new ProductPromo();
                //productpromo.Id == id;

               // productpromo
                return new OkObjectResult(responseobject);
                /* {
                     //StatusCode = new StringContent(responseobject.ToString(), Encoding.UTF8, "application/json")
                 }*/



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
