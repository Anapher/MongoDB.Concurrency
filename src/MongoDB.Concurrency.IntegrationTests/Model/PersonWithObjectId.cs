using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Concurrency.IntegrationTests.Base;

namespace MongoDB.Concurrency.IntegrationTests.Model
{
    public class PersonWithObjectId : IBaseConcurrencyObject
    {
        public PersonWithObjectId(ObjectId theIdOfTheObj, string randomProp)
        {
            TheIdOfTheObj = theIdOfTheObj;
            RandomProp = randomProp;
        }

        [BsonId] public ObjectId TheIdOfTheObj { get; set; }

        public string? RandomProp { get; set; }

        public int ConcurrencyToken { get; set; }
    }
}
