using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityEvent.API
{
    public class ProductivityEventData
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set;}
        public double ProductivityScore { get; set;}
        public bool EventStart { get; set;}
    }
}
