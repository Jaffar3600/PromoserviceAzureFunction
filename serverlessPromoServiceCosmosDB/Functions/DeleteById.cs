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
using System.Net;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.Documents;
using serverlessPromoServiceCosmosDB.Models;
using System.Linq;

namespace serverlessPromoServiceCosmosDB.Functions
{
    public static class DeleteById
    {

        [FunctionName("DeleteById")]
        public static async Task<IActionResult> DeleteTodo(
    [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "v1/promotion/{id}")]HttpRequest req,
    [CosmosDB(ConnectionStringSetting = "CosmosDBConnection")] DocumentClient client,
    TraceWriter log, string id)
        {

            ResponseObject responseobject = new ResponseObject();
            try
            {
                Uri collectionUri = UriFactory.CreateDocumentCollectionUri("PromoDatabase", "PromoCollection");
                var document = client.CreateDocumentQuery(collectionUri).Where(t => t.Id == id)
                        .AsEnumerable().FirstOrDefault();
                if (document == null)
                {
                    return new NotFoundResult();
                }
                await client.DeleteDocumentAsync(document.SelfLink);
                responseobject.correlationalId = Guid.NewGuid().ToString();
                responseobject.statusCode = 202;
                responseobject.statusReason = "Accepted";
                responseobject.success = true;
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
