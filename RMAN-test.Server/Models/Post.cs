using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;

namespace RMAN_test.Server.Models
{
    public class Post
    {
        [BsonId]
        [BsonGuidRepresentation(GuidRepresentation.Standard)] // Ensures UUID (subType 04) representation
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Content { get; set; }
        public Guid UserId { get; set; } // Ensure UserId is also of type Guid

    }
}
