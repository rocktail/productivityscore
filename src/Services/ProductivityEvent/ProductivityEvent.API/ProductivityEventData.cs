using Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;

namespace ProductivityEvent.API
{
    public class ProductivityEventData : ITableEntity
    {
        public ProductivityEventData(bool eventStart, double productivityScore)
        {
            this.PartitionKey = "queue";
            this.RowKey = Guid.NewGuid().ToString();
            this.EventStart = eventStart;
            this.ProductivityScore = productivityScore;
            this.ETag = ETag.All;
        }

        public string PartitionKey { get; set; }
        public string RowKey { get; set;}
        public double ProductivityScore { get; set;}
        public bool EventStart { get; set;}
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        DateTimeOffset ITableEntity.Timestamp { get => this.Timestamp.GetValueOrDefault(); set => this.Timestamp = value; }
        string ITableEntity.ETag { get => this.ETag.ToString(); set => this.ETag = new(value); }

        public void ReadEntity(IDictionary<string, EntityProperty> properties, OperationContext operationContext)
        {
            if (properties.TryGetValue("PartitionKey", out var propPartitionKey) && propPartitionKey.PropertyType == EdmType.String) 
            {
                this.PartitionKey = propPartitionKey.StringValue;
            }

            if (properties.TryGetValue("RowKey", out var propRowKey) && propRowKey.PropertyType == EdmType.String)
            {
                this.RowKey = propRowKey.StringValue;
            }

            if (properties.TryGetValue("ProductivityScore", out var propProductivityScore) && propProductivityScore.PropertyType == EdmType.Double)
            {
                this.ProductivityScore = propProductivityScore.DoubleValue.Value;
            }

            if (properties.TryGetValue("EventStart", out var propEventStart) && propEventStart.PropertyType == EdmType.Boolean)
            {
                this.EventStart = propEventStart.BooleanValue.Value;
            }

            if (properties.TryGetValue("Timestamp", out var propTimestamp))
            {
                this.Timestamp = propTimestamp.DateTimeOffsetValue;
            }

            if (properties.TryGetValue("ETag", out var propETag) && propETag.PropertyType == EdmType.String)
            {
                this.ETag = new ETag(propETag.StringValue);
            }
        }

        public IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
        {
            var properties = new Dictionary<string, EntityProperty>
            {
                { "PartitionKey", new EntityProperty(this.PartitionKey)},
                { "RowKey", new EntityProperty(this.RowKey)},
                { "ProductivityScore", new EntityProperty(this.ProductivityScore)},
                { "EventStart", new EntityProperty(this.EventStart)},
                { "Timestamp", new EntityProperty(this.Timestamp)},
                { "ETag", new EntityProperty(this.ETag.ToString())}
            };

            return properties;
        }
    }
}
