using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BooksQuery.Models
{
    public class Book
    {
        //[BsonId]
        //public ObjectId Id { get; set; }
        [BsonElement("title")]
        public string Title { get; set; } = string.Empty;
        [BsonElement("reserved")]
        public bool IsReserved { get; set; } = false;
        [BsonElement("event_id")]
        public string BookId { get; set; } = string.Empty;
    }
}
