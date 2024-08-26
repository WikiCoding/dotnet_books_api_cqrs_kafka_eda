using BooksCommand.Persistence;

namespace BooksCommand.Events
{
    public class BookCreatedEvent : IDomainEvent
    {
        //public int StreamId { get; set; }
        public Guid BookId { get; }
        public string Title { get; } = string.Empty;
        public bool IsReserved { get; } = false;
        public EventType EventType { get; }
        public DateTime CreatedDate { get; }
        public DateTime? ProcessedDate { get; set; }

        public BookCreatedEvent(Guid bookId, string title, bool isReserved, EventType eventType, DateTime createdDate)
        {
            BookId = bookId;
            Title = title;
            IsReserved = isReserved;
            EventType = eventType;
            CreatedDate = createdDate;
        }
    }
}
