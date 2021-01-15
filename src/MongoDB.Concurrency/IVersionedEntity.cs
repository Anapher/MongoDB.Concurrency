namespace MongoDB.Concurrency
{
    /// <summary>
    ///     Defines an entity with a version as concurrency token to apply optimistic concurrency
    /// </summary>
    public interface IVersionedEntity
    {
        /// <summary>
        ///     A version field that is updated every time the entity is replaced in database.
        /// </summary>
        int Version { get; }
    }
}
