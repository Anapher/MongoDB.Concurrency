using System;
using System.Linq.Expressions;
using MongoDB.Concurrency.IntegrationTests.Base;
using MongoDB.Concurrency.IntegrationTests.Model;
using Xunit;

namespace MongoDB.Concurrency.IntegrationTests
{
    public class ObjectWithStringId : IntegrationTest<PersonWithStringId>, IClassFixture<TestMongoServer>
    {
        public ObjectWithStringId(TestMongoServer server) : base(server)
        {
        }

        protected override PersonWithStringId GetInitialObject()
        {
            return new PersonWithStringId("Vincent");
        }

        protected override Expression<Func<PersonWithStringId, int>> GetSelector()
        {
            return x => x.Version;
        }
    }
}
