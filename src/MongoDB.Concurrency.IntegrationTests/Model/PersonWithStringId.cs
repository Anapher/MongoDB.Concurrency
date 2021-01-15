using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Concurrency.IntegrationTests.Base;

namespace MongoDB.Concurrency.IntegrationTests.Model
{
    public class PersonWithStringId : IBaseConcurrencyObject
    {
        public PersonWithStringId(string name)
        {
            Name = name;
        }

        [BsonId] public string Name { get; set; }

        public string? RandomProp { get; set; }

        public int Version { get; set; }
    }
}
