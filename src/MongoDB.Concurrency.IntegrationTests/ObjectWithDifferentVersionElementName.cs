using System;
using System.Linq.Expressions;
using MongoDB.Concurrency.IntegrationTests.Base;
using MongoDB.Concurrency.IntegrationTests.Model;
using Xunit;

namespace MongoDB.Concurrency.IntegrationTests
{
    public class ObjectWithDifferentVersionElementName : IntegrationTest<PersonWithDifferentVersionElementName>,
        IClassFixture<TestMongoServer>
    {
        public ObjectWithDifferentVersionElementName(TestMongoServer server) : base(server)
        {
        }

        protected override PersonWithDifferentVersionElementName GetInitialObject()
        {
            return new PersonWithDifferentVersionElementName(34);
        }

        protected override Expression<Func<PersonWithDifferentVersionElementName, int>> GetSelector()
        {
            return x => x.Version;
        }
    }
}
