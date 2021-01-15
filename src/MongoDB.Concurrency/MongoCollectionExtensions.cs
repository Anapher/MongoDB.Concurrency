using System;
using System.Linq.Expressions;
using MongoDB.Concurrency.Optimistic;
using MongoDB.Concurrency.Utils;
using MongoDB.Driver;

namespace MongoDB.Concurrency
{
    public static class MongoCollectionExtensions
    {
        /// <summary>
        ///     Initialize optimistic operations on the database using a <see cref="IVersionedEntity" />
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <param name="collection">The collection</param>
        /// <returns>Return operations that handle concurrency</returns>
        public static IOptimisticCollection<T> Optimistic<T>(this IMongoCollection<T> collection)
            where T : class, IVersionedEntity
        {
            return Optimistic(collection, x => x.Version);
        }

        /// <summary>
        ///     Initialize optimistic operations on the database using a selector for the version
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <param name="collection">The collection</param>
        /// <param name="versionSelector">
        ///     Select the version property of the entity. This property must not be nested, it must be a
        ///     property of <see cref="T" />
        /// </param>
        /// <returns>Return operations that handle concurrency</returns>
        public static IOptimisticCollection<T> Optimistic<T>(this IMongoCollection<T> collection,
            Expression<Func<T, int>> versionSelector) where T : class
        {
            return new OptimisticCollection<T>(collection, new PropertySelector<T, int>(versionSelector));
        }
    }
}
