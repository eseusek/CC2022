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

        public void CreateQueueClient(string queueName)
        {
            // Instantiate a QueueClient which will be used to create and manipulate the queue
            QueueClient queueClient = new QueueClient(connectionString, queueName);
        }

        public bool CreateQueue(string queueName)
        {
            try
            {
                // Instantiate a QueueClient which will be used to create and manipulate the queue
                QueueClient queueClient = new QueueClient(connectionString, queueName);

                // Create the queue
                queueClient.CreateIfNotExists();

                if (queueClient.Exists())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
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

        public void PeekMessage(string queueName)
        {
            // Instantiate a QueueClient which will be used to manipulate the queue
            QueueClient queueClient = new QueueClient(connectionString, queueName);

            if (queueClient.Exists())
            {
                // Peek at the next message
                PeekedMessage[] peekedMessage = queueClient.PeekMessages();

                // Display the message
            }
        }

        public void UpdateMessage(string queueName)
        {
            // Instantiate a QueueClient which will be used to manipulate the queue
            QueueClient queueClient = new QueueClient(connectionString, queueName);

            if (queueClient.Exists())
            {
                // Get the message from the queue
                QueueMessage[] message = queueClient.ReceiveMessages();

                // Update the message contents
                queueClient.UpdateMessage(message[0].MessageId,
                        message[0].PopReceipt,
                        "Updated contents",
                        TimeSpan.FromSeconds(10.0)  // Make it invisible for another 60 seconds
                    );
            }
        }

        public void DequeueMessage(string queueName)
        {
            // Instantiate a QueueClient which will be used to manipulate the queue
            QueueClient queueClient = new QueueClient(connectionString, queueName);

            if (queueClient.Exists())
            {
                // Get the next message
                QueueMessage[] retrievedMessage = queueClient.ReceiveMessages();

                // Process (i.e. print) the message in less than 30 seconds

                // Delete the message
                queueClient.DeleteMessage(retrievedMessage[0].MessageId, retrievedMessage[0].PopReceipt);
            }
        }
    }
}
