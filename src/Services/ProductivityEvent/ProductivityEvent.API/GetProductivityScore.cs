using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace ProductivityEvent.API
{
    public class GetProductivityScore
    {
        private readonly ILogger _logger;

        public GetProductivityScore(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GetProductivityScore>();
        }

        [Function("GetProductivityScore")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
            CloudTableClient cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
            CloudTable table = cloudTableClient.GetTableReference("ProductivityEvent");

            TableQuery<ProductivityEventData> query = new TableQuery<ProductivityEventData>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "productivityEvent")
                );

            var tableTask = table.ExecuteQuerySegmentedAsync(query, new TableContinuationToken());
            var tableData = tableTask.Result;
            var list = tableData.ToList();

            var result = ProductivityScoreCalculator.Calculate(list);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString($"Result is {result}");

            return response;
        }
    }
}
