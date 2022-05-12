using Microsoft.Azure.Cosmos;
using Projekt.Models;

namespace Projekt.Helper
{
    public class CosmosDbHelperMessage
    {
        private readonly IConfiguration Configuration;
        private static string _EndpointUri;
        private static string _PrimaryKey;
        private CosmosClient _cosmosClient;
        private Database _database;
        private Container _container;

        public CosmosDbHelperMessage(IConfiguration configuration)
        {

            _EndpointUri = configuration["EndPointUri"];
            _PrimaryKey = configuration["PrimaryKey"];
            Configuration = configuration;
        }

        private void CreateClient()
        {
            _cosmosClient = new CosmosClient(_EndpointUri, _PrimaryKey, new CosmosClientOptions() { ApplicationName = "datingapp" });
        }

        private async Task CreateDatabaseAsync()
        {
            this._database = await this._cosmosClient.CreateDatabaseIfNotExistsAsync("Users");
        }

        private async Task CreateContainerAsync()
        {
            this._container = await this._database.CreateContainerIfNotExistsAsync("Messages", "/MessageType");
        }

        public async Task AddItemsToContainerAsync(Message message)
        {
            CreateClient();
            await CreateDatabaseAsync();
            await CreateContainerAsync();
            await _container.CreateItemAsync<Message>(message, new PartitionKey(message.MessageType));
        }
    }
}
