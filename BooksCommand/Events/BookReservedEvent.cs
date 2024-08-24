namespace BooksCommand.Events
{
    public class BookReservedEvent : IDomainEvent
    {
        public Guid BookId { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsReserved { get; set; } = true;
        public bool IsUpdateEvent { get; set; } = true;
        public DateTime CreatedDate { get; set; }
        public DateTime? ProcessedDate { get; set; }
    }
}
