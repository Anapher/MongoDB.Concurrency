using System;
using System.Linq.Expressions;
using MongoDB.Concurrency.IntegrationTests.Base;
using MongoDB.Concurrency.IntegrationTests.Model;
using Xunit;

namespace MongoDB.Concurrency.IntegrationTests
{
    public class ObjectWithHugeStartVersion : IntegrationTest<PersonWithIntId>, IClassFixture<TestMongoServer>
    {
        public ObjectWithHugeStartVersion(TestMongoServer server) : base(server)
        {
        }

        protected override PersonWithIntId GetInitialObject()
        {
            return new PersonWithIntId(435, "asd") {Version = 435345};
        }

        protected override Expression<Func<PersonWithIntId, int>> GetSelector()
        {
            return x => x.Version;
        }
    }
}
