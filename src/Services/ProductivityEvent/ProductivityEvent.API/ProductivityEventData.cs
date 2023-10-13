using Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

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
        }

        public string PartitionKey { get; set; }
        public string RowKey { get; set;}
        public double ProductivityScore { get; set;}
        public bool EventStart { get; set;}
        public DateTimeOffset? Timestamp { get => DateTimeOffset.Now; set => throw new NotImplementedException(); }
        public ETag ETag { get => new("eTag"); set => throw new NotImplementedException(); }
        DateTimeOffset ITableEntity.Timestamp { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        string ITableEntity.ETag { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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
                { "PartitionKey", new EntityProperty(this.PartitionKey)},
                { "PartitionKey", new EntityProperty(this.PartitionKey)},
                { "PartitionKey", new EntityProperty(this.PartitionKey)},
                { "PartitionKey", new EntityProperty(this.PartitionKey)},
                { "PartitionKey", new EntityProperty(this.PartitionKey)},
            };

            return properties;
        }
    }
}
