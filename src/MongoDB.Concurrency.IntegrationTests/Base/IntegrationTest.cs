using System.Threading.Tasks;
using MongoDB.Concurrency.IntegrationTests.Model;
using MongoDB.Concurrency.Optimistic;
using MongoDB.Driver;
using Xunit;

namespace MongoDB.Concurrency.IntegrationTests.Base
{
    public abstract class IntegrationTest<T> : BaseIntegrationTest<T> where T : class, IBaseConcurrencyObject
    {
        protected IntegrationTest(TestMongoServer server)
        {
            Collection = server.CreateCollection<T>();
        }

        public IMongoCollection<T> Collection { get; }

        [Fact]
        public async Task UpdateAsync_GivenValidUpdates_UpdateVersion()
        {
            var initialObject = await SaveInitialObject(Collection);

            initialObject.RandomProp = "hey";

            await UpdateAndVerify(Collection, initialObject);

            initialObject.RandomProp = "world";

            await UpdateAndVerify(Collection, initialObject);
        }

        [Fact]
        public async Task UpdateAsync_GivenInvalidVersion_ThrowExceptionAndDontUpdate()
        {
            var initialObject = await SaveInitialObject(Collection);
            var sourceVersion = GetPropertySelector().GetValue(initialObject);

            initialObject.RandomProp = "hey";
            GetPropertySelector().SetValue(initialObject, 3);

            await Assert.ThrowsAsync<MongoConcurrencyUpdatedException>(async () =>
            {
                await Collection.Optimistic(GetSelector()).UpdateAsync(initialObject);
            });

            await VerifyNotChanged(Collection, initialObject, sourceVersion);
        }

        [Fact]
        public async Task UpdateAsync_GivenNonExistingId_ThrowException()
        {
            var initialObject = GetInitialObject();

            await Assert.ThrowsAsync<MongoConcurrencyDeletedException>(async () =>
            {
                await Collection.Optimistic(GetSelector()).UpdateAsync(initialObject);
            });
        }

        [Fact]
        public async Task DeleteAsync_GivenValidVersion_DeleteObject()
        {
            var initialObject = await SaveInitialObject(Collection);

            await DeleteAndVerify(Collection, initialObject);
        }

        [Fact]
        public async Task DeleteAsync_GivenInvalidVersion_ThrowExceptionAndDontDelete()
        {
            var initialObject = await SaveInitialObject(Collection);
            var sourceVersion = GetPropertySelector().GetValue(initialObject);

            GetPropertySelector().SetValue(initialObject, 32);

            await Assert.ThrowsAsync<MongoConcurrencyUpdatedException>(async () =>
            {
                await Collection.Optimistic(GetSelector()).DeleteAsync(initialObject);
            });

            var entity = await GetCurrentVersionOfObject(Collection, initialObject);
            Assert.NotNull(entity);
            Assert.Equal(sourceVersion, GetPropertySelector().GetValue(entity!));
        }
    }
}
