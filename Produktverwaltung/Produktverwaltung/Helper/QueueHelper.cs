using Azure.Storage.Queues; // Namespace for Queue storage types
using Azure.Storage.Queues.Models;

namespace Produktverwaltung.Helper
{
    public class QueueHelper
    {
        private readonly IConfiguration Configuration;
        string connectionString;

        public QueueHelper(IConfiguration configuration)
        {
            Configuration = configuration;
            connectionString = Configuration["StorageConnectionString"];
        }

        public void CreateQueue(string queueName)
        {
            try
            {
                // Instantiate a QueueClient which will be used to create and manipulate the queue
                QueueClient queueClient = new QueueClient(connectionString, queueName);

                // Create the queue
                queueClient.CreateIfNotExists();
            }
            catch (Exception ex)
            {
            }
        }

        public void InsertMessage(string queueName, string message)
        {
            // Instantiate a QueueClient which will be used to create and manipulate the queue
            QueueClient queueClient = new QueueClient(connectionString, queueName);

            // Create the queue if it doesn't already exist
            queueClient.CreateIfNotExists();

            if (queueClient.Exists())
            {
                // Send a message to the queue
                queueClient.SendMessage(message);
            }

        }        
    }
}
