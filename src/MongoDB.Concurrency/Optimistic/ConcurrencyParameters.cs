using System;
using MongoDB.Bson;
using MongoDB.Concurrency.Utils;
using MongoDB.Driver;

namespace MongoDB.Concurrency.Optimistic
{
    internal class ConcurrencyParameters<T> where T : class
    {
        public ConcurrencyParameters(T obj, bool isUpdate, PropertySelector<T, int> versionSelector)
        {
            IsUpdate = isUpdate;
            RequestedVersion = versionSelector.GetValue(obj);

            if (isUpdate) versionSelector.SetValue(obj, RequestedVersion + 1);

            var bson = obj.ToBsonDocument();

            Id = bson["_id"];

            if (Id.IsNullOrEmpty())
                throw new InvalidOperationException(
                    "Optimistic Update can only be used with documents that have an Id.");

            QueryById = Builders<T>.Filter.Eq("_id", Id);
            QueryWithVersion = Builders<T>.Filter.And(QueryById,
                Builders<T>.Filter.Eq(versionSelector.Selector, RequestedVersion));
        }

        public bool IsUpdate { get; }
        public BsonValue Id { get; }
        public int RequestedVersion { get; }

        public FilterDefinition<T> QueryWithVersion { get; }
        public FilterDefinition<T> QueryById { get; }
    }
}
