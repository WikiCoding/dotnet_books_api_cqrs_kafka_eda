namespace BooksQuery.Models
{
    public class BookReadDataModel
    {
        public int StreamId { get; set; }
        public string BookId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public bool IsReserved { get; set; } = false;
        public EventType EventType { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ProcessedDate { get; set; }
    }
}
