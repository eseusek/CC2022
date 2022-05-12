using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Projekt.Models;

namespace InterestTrigger
{
    public class InLove
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public Guid PartOne { get; set; }
        public Guid PartTwo { get; set; }
    }

    public class InterestTrigger
    {
        [FunctionName("InterestTrigger")]
        [return: Table("InLove")]
        public InLove Run([QueueTrigger("interestmessages", Connection = "AzureWebJobsStorage")]string queueEntryMessage, ILogger log)
        {
            var message = Newtonsoft.Json.JsonConvert.DeserializeObject<Message>(queueEntryMessage);

            if (message.MessageType == "Answer")
            {
                return new InLove { PartitionKey = "Queue", RowKey = Guid.NewGuid().ToString(), PartOne = message.Sender, PartTwo = message.Receiver};
            }

            return null;
        }
    }
}
