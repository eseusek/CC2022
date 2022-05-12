using Azure.Storage.Queues; // Namespace for Queue storage types

namespace Projekt.Helper
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
