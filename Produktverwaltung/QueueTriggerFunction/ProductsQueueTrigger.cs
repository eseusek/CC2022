using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Produktverwaltung.Models;
using System;

namespace QueueTriggerFunction
{
    public class ExpensiveProduct
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string Text { get; set; }
    }

    public static class ProductsQueueTrigger
    {
        [FunctionName("ProductsQueueTrigger")]
        [return: Table("ExpensiveProducts")]
        public static ExpensiveProduct Run(
            [QueueTrigger("produkte")] string myQueueItem,
            ILogger log)
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
