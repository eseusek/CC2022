using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Produktverwaltung.Models;

namespace ProductsQueueTrigger
{
    public class ExpensiveProduct
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string Text { get; set; }
    }

    public class ProductsQueueTriggerFunction
    {
        [FunctionName("ProductsQueueTriggerFunction")]
        [return: Table("ExpensiveProducts")]
        public ExpensiveProduct Run([QueueTrigger("produkte", Connection = "AzureWebJobsStorage")]string myQueueItem, ILogger log)
        {
            var produkt = Newtonsoft.Json.JsonConvert.DeserializeObject<Produkt>(myQueueItem);
            if (produkt.Price > 100)
            {
                return new ExpensiveProduct { PartitionKey = "Queue", RowKey = Guid.NewGuid().ToString(), Text = produkt.Name };
            }
            return null;
        }
    }
}
