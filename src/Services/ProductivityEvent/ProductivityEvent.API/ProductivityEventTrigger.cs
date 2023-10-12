using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using HttpTriggerAttribute = Microsoft.Azure.Functions.Worker.HttpTriggerAttribute;

namespace ProductivityEvent.API
{
    public class ProductivityEventTrigger
    {
        [Function("ProductivityEventTrigger")]
        [TableOutput("ProductivityEvent", Connection = "AzureWebJobsStorage")]
        public static ProductivityEventData Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req,
            [TableInput("ProductivityEvent" , "PartitionKey", "{queueTrigger}")] ProductivityEventData productivityEventDataInput,
            FunctionContext context)
        {
            var logger = context.GetLogger("ProductivityEventTrigger");
            logger.LogInformation("C# HTTP trigger function processed a request.");

            return new ProductivityEventData
            {
                PartitionKey = "queue",
                RowKey = Guid.NewGuid().ToString(),
                EventStart = true,
                ProductivityScore = 3.0
            };
        }
    }
}
