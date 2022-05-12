#nullable disable
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Projekt.Helper;
using Projekt.Models;

namespace Projekt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatingController : ControllerBase
    {
        private readonly IConfiguration Configuration;
        string connectionString;
        private readonly CosmosDbHelperPerson _cosmosDbHelperPerson;
        private readonly CosmosDbHelperMessage _cosmosDbHelperMessage;
        private int _personNrCounter;
        private readonly QueueHelper _queueHelper;

        public DatingController(IConfiguration configuration)
        {
            Configuration = configuration;
            connectionString = Configuration["StorageConnectionString"];
            _cosmosDbHelperPerson = new CosmosDbHelperPerson(configuration);
            _queueHelper = new QueueHelper(configuration);
            _cosmosDbHelperMessage = new CosmosDbHelperMessage(configuration);
        }

        [HttpGet("{gender}")]
        public async Task<ActionResult<Person>> GetDataFromFirst(string gender)
        {
            var persons = await _cosmosDbHelperPerson.GetAllByGender(gender);
            _personNrCounter = 0;
            return persons[0];
        }

        [HttpGet]
        public async Task<ActionResult<Person>> GetDataFromNext(string gender, int next)
        {
            var persons = await _cosmosDbHelperPerson.GetAllByGender(gender);
            return persons[next];
        }

        [HttpPut("{sender, receiver, text, isAnswer}")]
        public async Task ShowInterest(Guid sender, Guid receiver, string text, bool isAnswer)
        {
            Message message = new Message(isAnswer? "Answer": "Message", sender, receiver, text);
            var messageToSend = Convert.ToBase64String(Encoding.UTF8.GetBytes(message.ToString()));
            _queueHelper.InsertMessage("interestmessages", messageToSend);
            await _cosmosDbHelperMessage.AddItemsToContainerAsync(message);
        }

        [HttpPost("{person}")]
        public async void Register(Person person)
        {
            await _cosmosDbHelperPerson.AddItemsToContainerAsync(person);
        }

        [HttpDelete("{id, gender}")]
        public async void NotInterestedAnyMore(Guid id, string gender)
        {
            await _cosmosDbHelperPerson.DeletePerson(id, gender);
        }
    }
}
