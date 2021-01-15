using MongoDB.Bson;

namespace MongoDB.Concurrency.Utils
{
    internal static class BsonValueExtensions
    {
        /// <remarks>Handles only BsonNull, empty String or empty ObjectId</remarks>
        public static bool IsNullOrEmpty(this BsonValue value)
        {
            return value.IsBsonNull || value.IsString && string.IsNullOrWhiteSpace(value.AsString) ||
                   value.IsObjectId && ObjectId.Empty.Equals(value.AsObjectId);
        }
    }
}
