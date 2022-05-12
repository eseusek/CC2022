using Newtonsoft.Json;

namespace Projekt.Models
{
    public class Person
    {
        [JsonProperty(PropertyName = "id")]
        public Guid UniqueIdentifier { get; }
        public string? Name { get; set; }
        public int Age { get; set; }
        public long Sice { get;set; }
        public string? Hobbys { get; set; }
        public string PictureLink { get; set; }
        public string EmailAdress { get; set; }
        public string Gender { get; set; }

        public Person()
        {
            UniqueIdentifier = Guid.NewGuid();
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
