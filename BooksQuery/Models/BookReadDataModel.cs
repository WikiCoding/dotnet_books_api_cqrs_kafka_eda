namespace BooksQuery.Models
{
    public class BookReadDataModel
    {
        public int StreamId { get; set; }
        public string BookId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public bool IsReserved { get; set; } = false;
        public bool IsCreationEvent { get; set; } = true;
        //public bool IsUpdateEvent { get; set; } = false;
        //public bool IsDeleteEvent { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ProcessedDate { get; set; }
    }
}
