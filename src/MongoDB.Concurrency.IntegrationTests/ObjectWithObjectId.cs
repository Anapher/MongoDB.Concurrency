using System;
using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Concurrency.IntegrationTests.Base;
using MongoDB.Concurrency.IntegrationTests.Model;
using Xunit;

namespace MongoDB.Concurrency.IntegrationTests
{
    public class ObjectWithObjectId : IntegrationTest<PersonWithObjectId>, IClassFixture<TestMongoServer>
    {
        public ObjectWithObjectId(TestMongoServer server) : base(server)
        {
        }

        protected override PersonWithObjectId GetInitialObject()
        {
            return new PersonWithObjectId(ObjectId.GenerateNewId(), "big yikes");
        }

        protected override Expression<Func<PersonWithObjectId, int>> GetSelector()
        {
            return x => x.ConcurrencyToken;
        }
    }
}
