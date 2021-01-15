﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Concurrency.Utils;
using MongoDB.Driver;

namespace MongoDB.Concurrency.Optimistic
{
    internal class OptimisticCollection<T> : IOptimisticCollection<T> where T : class
    {
        private readonly PropertySelector<T, int> _versionSelector;

        public OptimisticCollection(IMongoCollection<T> collection, PropertySelector<T, int> versionSelector)
        {
            _versionSelector = versionSelector;
            Collection = collection;
        }

        public IMongoCollection<T> Collection { get; set; }

        public async Task<DeleteResult?> DeleteAsync(T obj, DeleteOptions? options = null,
            CancellationToken token = default)
        {
            var parameters = new ConcurrencyParameters<T>(obj, false, _versionSelector);
            var result = await Collection.DeleteOneAsync(parameters.QueryWithVersion, options, token);

            await CheckConcurrencyResult(result.DeletedCount, parameters);
            return result;
        }

        public async Task<ReplaceOneResult?> UpdateAsync(T obj, ReplaceOptions? options = null,
            CancellationToken token = default)
        {
            ConcurrencyParameters<T>? parameters = null;

            try
            {
                parameters = new ConcurrencyParameters<T>(obj, true, _versionSelector);

                var result = await Collection.ReplaceOneAsync(parameters.QueryWithVersion, obj, options, token);

                await CheckConcurrencyResult(result.ModifiedCount, parameters);
                return result;
            }
            catch (Exception)
            {
                if (parameters != null) _versionSelector.SetValue(obj, parameters.RequestedVersion);
                throw;
            }
        }

        private async Task CheckConcurrencyResult(long affected, ConcurrencyParameters<T> parameters)
        {
            if (affected == 1) return;

            var existingObject = await (await Collection.FindAsync(parameters.QueryById,
                new FindOptions<T>
                {
                    Projection =
                        Builders<T>.Projection.Include(new ExpressionFieldDefinition<T>(_versionSelector.Selector)),
                })).FirstOrDefaultAsync();

            if (existingObject != null)
                throw new MongoConcurrencyUpdatedException(parameters.Id, parameters.RequestedVersion,
                    _versionSelector.GetValue(existingObject));

            if (parameters.IsUpdate)
                throw new MongoConcurrencyDeletedException(parameters.Id, parameters.RequestedVersion);
        }
    }
}
