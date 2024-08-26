namespace BooksCommand.Events
{
    public class BookCreatedEvent : IDomainEvent
    {
        //public int StreamId { get; set; }
        public Guid BookId { get; }
        public string Title { get; } = string.Empty;
        public bool IsReserved { get; } = false;
        public bool IsCreationEvent { get; } = true;
        public DateTime CreatedDate { get; }
        public DateTime? ProcessedDate { get; set; }

        public BookCreatedEvent(Guid bookId, string title, bool isReserved, bool isCreationEvent, DateTime createdDate)
        {
            BookId = bookId;
            Title = title;
            IsReserved = isReserved;
            IsCreationEvent = isCreationEvent;
            CreatedDate = createdDate;
        }
    }
}
