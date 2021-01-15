using System.Threading;
using System.Threading.Tasks;
using MongoDB.Concurrency.Utils;
using MongoDB.Driver;

namespace MongoDB.Concurrency.Optimistic
{
    /// <summary>
    ///     Provide optimistic operations for a <see cref="IMongoCollection{TDocument}" />
    /// </summary>
    /// <typeparam name="T">The type of the entity</typeparam>
    public interface IOptimisticCollection<in T> : IFluentInterface
    {
        /// <summary>
        ///     Update an entity in database. The version of the entity must match the current version of the entity in database,
        ///     else an exception is thrown.
        /// </summary>
        /// <param name="obj">The object that should be updated</param>
        /// <param name="options">Replace options for MongoDB</param>
        /// <param name="token">The cancellation token</param>
        /// <returns>Return the update result</returns>
        /// <exception cref="MongoConcurrencyUpdatedException">
        ///     The exception is thrown if the version of the entity does not match
        ///     the version in the database
        /// </exception>
        /// <exception cref="MongoConcurrencyDeletedException">
        ///     The exception is thrown if the entity was not found in database.
        /// </exception>
        Task<ReplaceOneResult?> UpdateAsync(T obj, ReplaceOptions? options = null, CancellationToken token = default);

        /// <summary>
        ///     Delete an entity from database. If operation is executed if the version of the entity does match the version of the
        ///     entity in database, else an exception is thrown.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="options"></param>
        /// <param name="token"></param>
        /// <returns>Return the delete result</returns>
        /// <exception cref="MongoConcurrencyUpdatedException">
        ///     The exception is thrown if the version of the entity does not match
        ///     the version in the database
        /// </exception>
        /// <exception cref="MongoConcurrencyDeletedException">
        ///     The exception is thrown if the entity was not found in database.
        /// </exception>
        Task<DeleteResult?> DeleteAsync(T obj, DeleteOptions? options = null, CancellationToken token = default);
    }
}
