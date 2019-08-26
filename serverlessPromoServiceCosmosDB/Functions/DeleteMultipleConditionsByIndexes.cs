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
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace serverlessPromoServiceCosmosDB.Functions
{
    public static class DeleteMultipleConditionsByIndexes
    {
        [FunctionName("DeleteMultipleConditionsByIndexes")]
        public static async Task<IActionResult> Run(
             [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "v1/promotions/{id}/conditions")] HttpRequestMessage req, HttpRequest request,
             [CosmosDB(
                databaseName: "PromoDatabase",
                collectionName: "PromoCollection",
                ConnectionStringSetting = "CosmosDBConnection",
                Id = "{id}")
            ]ProductPromo productpromo,
             ILogger log)



        {
            ResponseObject responseobject = new ResponseObject();
            var headers = req.Headers;
            var query = request.Query;
           
                    try
                    {
                        List<ProductPromoCondition> productPromoCondition = productpromo.conditions;
                        string indexValue = "";
                        if (query.ContainsKey("indexes"))
                        {
                            indexValue = query["indexes"].First();
                            string[] indexValuesArray = indexValue.Split(",");
                            foreach (var item in productPromoCondition.ToList())
                            {
                                bool present = indexValuesArray.Contains(item.index);
                                if (present)
                                {
                                    productPromoCondition.Remove(item);
                                }
                            }
                        }



                        responseobject.correlationalId = Guid.NewGuid().ToString();
                        responseobject.statusCode = 201;
                        responseobject.statusReason = "Accepted";
                        responseobject.success = true;

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

   