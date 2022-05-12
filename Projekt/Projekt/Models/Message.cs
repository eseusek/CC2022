using Newtonsoft.Json;

namespace Projekt.Models
{
    public class Message
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }
        public string? MessageType { get; set; }
        public Guid Sender { get; set; }
        public Guid Receiver { get; set; }
        public string? Text { get; set; }

        public Message(string? messageType, Guid sender, Guid receiver, string? text)
        {
            Id = Guid.NewGuid();
            MessageType = messageType;
            Sender = sender;
            Receiver = receiver;
            Text = text;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
