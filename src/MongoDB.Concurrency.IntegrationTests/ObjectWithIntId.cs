using System;
using System.Linq.Expressions;
using MongoDB.Concurrency.IntegrationTests.Base;
using MongoDB.Concurrency.IntegrationTests.Model;
using Xunit;

namespace MongoDB.Concurrency.IntegrationTests
{
    public class ObjectWithIntId : IntegrationTest<PersonWithIntId>, IClassFixture<TestMongoServer>
    {
        public ObjectWithIntId(TestMongoServer server) : base(server)
        {
        }

        protected override PersonWithIntId GetInitialObject()
        {
            return new PersonWithIntId(45, string.Empty);
        }

        protected override Expression<Func<PersonWithIntId, int>> GetSelector()
        {
            return x => x.Version;
        }
    }
}
