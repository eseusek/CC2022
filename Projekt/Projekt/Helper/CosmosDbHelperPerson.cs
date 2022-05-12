using Microsoft.Azure.Cosmos;
using Projekt.Models;
using System.Net;

namespace Projekt.Helper
{
    public class CosmosDbHelperPerson
    {
        private readonly IConfiguration Configuration;
        private static string _EndpointUri;
        private static string _PrimaryKey;
        private CosmosClient _cosmosClient;
        private Database _database;
        private Container _container;

        public CosmosDbHelperPerson(IConfiguration configuration)
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
            this._database = await _cosmosClient.CreateDatabaseIfNotExistsAsync("Users");
        }

        private async Task CreateContainerAsync()
        {
            this._container = await _database.CreateContainerIfNotExistsAsync("Users", "/Gender");
        }

        public async Task AddItemsToContainerAsync(Person person)
        {
            CreateClient();
            await CreateDatabaseAsync();
            await CreateContainerAsync();
            try
            {
                ItemResponse<Person> userResponse = await _container.ReadItemAsync<Person>(person.UniqueIdentifier.ToString(), new PartitionKey(person.Gender));
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                ItemResponse<Person> userResponse = await _container.CreateItemAsync<Person>(person, new PartitionKey(person.Gender));
            }
        }

        public async Task<ItemResponse<Person>> GetItemsFromContainerAsync(Guid uniqueIdentifier, string gender)
        {
            CreateClient();
            await CreateDatabaseAsync();
            await CreateContainerAsync();
            return await _container.ReadItemAsync<Person>(uniqueIdentifier.ToString(), new PartitionKey(gender));
        }

        public async Task<List<Person>> GetAllByGender(string gender)
        {
            CreateClient();
            await CreateDatabaseAsync();
            await CreateContainerAsync();
            var queryDefinition = new QueryDefinition("SELECT * FROM c");
            var iterator = _container.GetItemQueryIterator<Person>(queryDefinition,
                        requestOptions: new QueryRequestOptions()
                        {
                            PartitionKey = new PartitionKey(gender)
                        });

            var results = new List<Person>();

            while (iterator.HasMoreResults)
            {
                var result = await iterator.ReadNextAsync();
                results.AddRange(result.Resource);
            }

            return results;
        }

        public async Task DeletePerson(Guid personId, string gender)
        {
            CreateClient();
            await CreateDatabaseAsync();
            await CreateContainerAsync();
            await _container.DeleteItemAsync<Person>(personId.ToString(), new PartitionKey(gender));
        }
    }
}
