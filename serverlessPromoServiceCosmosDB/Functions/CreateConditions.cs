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
using System.Net;
using System.Net.Http;
using System.Text;

namespace serverlessPromoServiceCosmosDB.Functions
{
    
        public class CreateConditions
        {
            [FunctionName("CreateConditions")]
            public static HttpResponseMessage Run(
                [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/promotions/{id}/conditions")]HttpRequestMessage req,
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
                // ProductPromoCondition productPromoCondition = new ProductPromoCondition();
                var content = req.Content;
                string jsonContent = content.ReadAsStringAsync().Result;
                dynamic condition = JsonConvert.DeserializeObject<ProductPromoCondition>(jsonContent);



                try
                {
                    if (productPromo.conditions == null)
                    {
                        List<ProductPromoCondition> newList = new List<ProductPromoCondition>();
                        newList.Add(condition);
                        productPromo.conditions = newList;
                    }
                    else
                    {
                        productPromo.conditions.Add(condition);
                    }



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
