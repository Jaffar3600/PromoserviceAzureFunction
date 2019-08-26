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
using System.Linq;

namespace serverlessPromoServiceCosmosDB.Funtions
{
    public class RetrivePromoById
    {
        [FunctionName("retrivepromo")]
        public static async Task<IActionResult> Run(
             [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "promotion/{id}")] HttpRequestMessage req,
             [CosmosDB(
                databaseName: "PromoDatabase",
                collectionName: "PromoCollection",
                ConnectionStringSetting = "CosmosDBConnection",
                Id = "{id}")
            ]ProductPromo productpromo,
             ILogger log)
        {
            ResponseObject responseobject = new ResponseObject();
            var header = req.Headers;
            if (header.Contains("tenant"))
            {
                string value = header.GetValues("tenant").First();
                if (value.Equals("dcp"))
                {
                   
                    log.LogInformation($"Function triggered");


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
                else
                {
                      return new OkObjectResult(responseobject);
                }
            }
            else
            {
                return new OkObjectResult(responseobject);
            }
        }
    }
}
