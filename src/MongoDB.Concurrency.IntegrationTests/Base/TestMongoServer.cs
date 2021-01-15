using System;
using Mongo2Go;
using MongoDB.Driver;

namespace MongoDB.Concurrency.IntegrationTests.Base
{
    public class TestMongoServer : IDisposable
    {
        private readonly string _databaseName = Guid.NewGuid().ToString("N");
        private readonly MongoDbRunner _runner;

        public TestMongoServer()
        {
            _runner = MongoDbRunner.Start();
        }

        public void Dispose()
        {
            _runner.Dispose();
        }

        public IMongoCollection<T> CreateCollection<T>()
        {
            var client = new MongoClient(_runner.ConnectionString);
            return client.GetDatabase(_databaseName).GetCollection<T>(Guid.NewGuid().ToString("N"));
        }
    }
}
