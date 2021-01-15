using System;
using MongoDB.Bson;

namespace MongoDB.Concurrency.Optimistic
{
    public class MongoConcurrencyUpdatedException : Exception
    {
        public MongoConcurrencyUpdatedException(BsonValue objectId, int requestedVersion, int currentVersion) : base(
            "The target object has already been modified")
        {
            ObjectId = objectId;
            RequestedVersion = requestedVersion;
            CurrentVersion = currentVersion;
        }

        public BsonValue ObjectId { get; }
        public int RequestedVersion { get; set; }
        public int CurrentVersion { get; set; }

        public override string ToString()
        {
            return
                $"The requested object ({ObjectId}) has already been modified: Requested version = {RequestedVersion} - Current version = {CurrentVersion}";
        }
    }
}
