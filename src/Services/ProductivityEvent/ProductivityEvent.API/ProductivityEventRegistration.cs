using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using HttpTriggerAttribute = Microsoft.Azure.Functions.Worker.HttpTriggerAttribute;

//?EventStart=Yes&ProductivityScore=-3
namespace ProductivityEvent.API
{
    public class ProductivityEventRegistration
    {
        [Function("ProductivityEventRegistration")]
        //[TableOutput("ProductivityEvent", Connection = "AzureWebJobsStorage")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req,
            //[TableInput("ProductivityEvent" , "PartitionKey", "RowKey")] ProductivityEventData productivityEventDataInput,
            FunctionContext context)
        {
            var logger = context.GetLogger("ProductivityEventRegistration");
            logger.LogInformation("C# HTTP trigger function processed a request.");

            ProductivityEventData productivityEventData = new ProductivityEventData(req.Query["EventStart"].Equals("Yes"), double.Parse(req.Query["ProductivityScore"].ToString()));

            string connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
            CloudTableClient cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
            CloudTable table = cloudTableClient.GetTableReference("ProductivityEvent");

            TableOperation insertOperation = TableOperation.Insert(productivityEventData);
            table.ExecuteAsync(insertOperation);

            var actionResult = (ActionResult)new OkObjectResult(String.Format("Partition Key {0}, RowKey = {1}; EventStart = {2}; ProductivityScore = {3}", productivityEventData.PartitionKey, productivityEventData.RowKey, productivityEventData.EventStart, productivityEventData.ProductivityScore));
            return actionResult;
        }
    }
}
