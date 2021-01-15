using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Concurrency.IntegrationTests.Base;

namespace MongoDB.Concurrency.IntegrationTests.Model
{
    public class PersonWithIntId : IBaseConcurrencyObject
    {
        public PersonWithIntId(int name, string randomProp)
        {
            Name = name;
            RandomProp = randomProp;
        }

        [BsonId] public int Name { get; set; }

        public string? RandomProp { get; set; }

        public int Version { get; set; }
    }
}
