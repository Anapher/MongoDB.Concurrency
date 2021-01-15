using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Concurrency.IntegrationTests.Base;

namespace MongoDB.Concurrency.IntegrationTests.Model
{
    public class PersonWithDifferentVersionElementName : IBaseConcurrencyObject
    {
        public PersonWithDifferentVersionElementName(int id)
        {
            Id = id;
        }

        [BsonId] public int Id { get; set; }

        public string? RandomProp { get; set; }

        [BsonElement("V")] public int Version { get; set; }
    }
}
