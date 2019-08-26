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
using System.Net;
using System.Text;
using System.Linq;

namespace serverlessPromoServiceCosmosDB.Funtions
{
    public class InsertItem
    {
        [FunctionName("CreatePromo")]
        public static HttpResponseMessage Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/promotions")]HttpRequestMessage req,
            [CosmosDB(
                databaseName: "PromoDatabase",
                collectionName: "PromoCollection",
                ConnectionStringSetting = "CosmosDBConnection")]
            out ProductPromo document,
            ILogger log)
        {
            ResponseObject responseobject = new ResponseObject();
            var content = req.Content;
            string jsonContent = content.ReadAsStringAsync().Result;
            document = JsonConvert.DeserializeObject<ProductPromo>(jsonContent);
            /*var header = req.Headers;
            if (header.Contains("tenant"))
            { 
                string value = header.GetValues("tenant").First();
                if (value.Equals("dcp"))
                {*/
                 
                    try
                    {
                        responseobject.correlationalId = Guid.NewGuid().ToString();
                        responseobject.statusCode = 201;
                        responseobject.statusReason = "Created";
                        responseobject.success = true;
                        responseobject.promotionId = document.Id;
                        
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
        

    
