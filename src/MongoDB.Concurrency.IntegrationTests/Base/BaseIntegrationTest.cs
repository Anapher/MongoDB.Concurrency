using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Concurrency.Optimistic;
using MongoDB.Concurrency.Utils;
using MongoDB.Driver;
using Newtonsoft.Json;
using Xunit;

namespace MongoDB.Concurrency.IntegrationTests.Base
{
    public abstract class BaseIntegrationTest<T> where T : class
    {
        protected abstract T GetInitialObject();

        protected abstract Expression<Func<T, int>> GetSelector();

        internal PropertySelector<T, int> GetPropertySelector()
        {
            return new PropertySelector<T, int>(GetSelector());
        }

        protected async Task<T> SaveInitialObject(IMongoCollection<T> collection)
        {
            var initialObject = GetInitialObject();
            await collection.InsertOneAsync(initialObject);

            var result = await GetCurrentVersionOfObject(collection, initialObject);
            Assert.NotNull(result);

            return result!;
        }

        protected async Task<T> UpdateAndVerify(IMongoCollection<T> collection, T entity)
        {
            var sourceVersion = GetPropertySelector().GetValue(entity);
            await collection.Optimistic(GetSelector()).UpdateAsync(entity);

            return await VerifyUpdate(collection, entity, sourceVersion);
        }

        protected async Task DeleteAndVerify(IMongoCollection<T> collection, T person)
        {
            await collection.Optimistic(GetSelector()).DeleteAsync(person);

            await VerifyDelete(collection, person);
        }

        protected async Task<T> VerifyUpdate(IMongoCollection<T> collection, T obj, int sourceVersion)
        {
            var createdObject = await GetCurrentVersionOfObject(collection, obj);

            Assert.NotNull(createdObject);
            Assert.NotEqual(sourceVersion, GetPropertySelector().GetValue(createdObject!));

            // deep equal
            Assert.Equal(JsonConvert.SerializeObject(createdObject), JsonConvert.SerializeObject(obj));

            return createdObject!;
        }

        protected async Task VerifyNotChanged(IMongoCollection<T> collection, T obj, int version)
        {
            var currentVersion = await GetCurrentVersionOfObject(collection, obj);
            if (currentVersion == null)
            {
                Assert.NotNull(currentVersion);
                return;
            }

            Assert.Equal(version, GetPropertySelector().GetValue(currentVersion));
        }

        private async Task VerifyDelete(IMongoCollection<T> collection, T obj)
        {
            var deletedObject = await GetCurrentVersionOfObject(collection, obj);
            Assert.Null(deletedObject);
        }

        protected async Task<T?> GetCurrentVersionOfObject(IMongoCollection<T> collection, T source)
        {
            var parameters = new ConcurrencyParameters<T>(source, false, GetPropertySelector());
            return await collection.Find(parameters.QueryById).FirstOrDefaultAsync();
        }
    }
}
