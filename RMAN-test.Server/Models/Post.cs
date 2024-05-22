using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace RMAN_test.Server.Models
{
    public class Post
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Content { get; set; }

        public string UserId { get; set; } // Assuming each post is associated with a user

    }
}
