using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Concurrency.IntegrationTests.Base;
using MongoDB.Concurrency.IntegrationTests.Model;
using MongoDB.Concurrency.Optimistic;
using MongoDB.Driver;
using Xunit;

namespace MongoDB.Concurrency.IntegrationTests
{
    public class UpsertObject : BaseIntegrationTest<PersonWithIntId>, IClassFixture<TestMongoServer>
    {
        private readonly IMongoCollection<PersonWithIntId> _collection;

        public UpsertObject(TestMongoServer server)
        {
            _collection = server.CreateCollection<PersonWithIntId>();
        }

        [Fact]
        public async Task Update_EntityDoesNotExist_ThrowError()
        {
            var person = GetInitialObject();

            await Assert.ThrowsAsync<MongoConcurrencyDeletedException>(async () =>
            {
                await _collection.Optimistic(x => x.Version).UpdateAsync(person);
            });
        }

        [Fact]
        public async Task Upsert_EntityDoesNotExist_CreateEntity()
        {
            var person = GetInitialObject();

            await _collection.Optimistic(x => x.Version).UpdateAsync(person, new ReplaceOptions {IsUpsert = true});

            await VerifyUpdate(_collection, person, int.MinValue);
        }

        [Fact]
        public async Task Upsert_EntityExistsAndRaceCondition_ThrowError()
        {
            var initialObject = await SaveInitialObject(_collection);
            var sourceVersion = initialObject.Version;

            initialObject.RandomProp = "hey";
            initialObject.Version = 3;

            await Assert.ThrowsAsync<MongoConcurrencyUpdatedException>(async () =>
            {
                await _collection.Optimistic(GetSelector())
                    .UpdateAsync(initialObject, new ReplaceOptions {IsUpsert = true});
            });

            await VerifyNotChanged(_collection, initialObject, sourceVersion);
        }

        [Fact]
        public async Task Upsert_EntityExists_UpdateEntity()
        {
            var initialObject = await SaveInitialObject(_collection);

            initialObject.RandomProp = "hey";
            var sourceVersion = initialObject.Version;

            await _collection.Optimistic(GetSelector())
                .UpdateAsync(initialObject, new ReplaceOptions {IsUpsert = true});

            await VerifyUpdate(_collection, initialObject, sourceVersion);

            initialObject.RandomProp = "world";
            sourceVersion = initialObject.Version;

            await _collection.Optimistic(GetSelector())
                .UpdateAsync(initialObject, new ReplaceOptions {IsUpsert = true});

            await VerifyUpdate(_collection, initialObject, sourceVersion);
        }

        protected override PersonWithIntId GetInitialObject()
        {
            return new PersonWithIntId(1, "Mahlzeit");
        }

        protected override Expression<Func<PersonWithIntId, int>> GetSelector()
        {
            return x => x.Version;
        }
    }
}
