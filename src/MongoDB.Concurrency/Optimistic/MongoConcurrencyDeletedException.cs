using System;
using MongoDB.Bson;

namespace MongoDB.Concurrency.Optimistic
{
    public class MongoConcurrencyDeletedException : Exception
    {
        public MongoConcurrencyDeletedException(BsonValue objectId, int requestedVersion) : base(
            "The target object has already been deleted")
        {
            ObjectId = objectId;
            RequestedVersion = requestedVersion;
        }

        public BsonValue ObjectId { get; set; }
        public int RequestedVersion { get; set; }

        public override string ToString()
        {
            const string message = "The requested object ({0}) has already been deleted: Requested version = {1}";
            return string.Format(message, ObjectId, RequestedVersion);
        }
    }
}
