using BooksCommand.Persistence.Datamodels;

namespace BooksCommand.Domain.Events
{
    public class BookReservedEvent : IDomainEvent
    {
        public Guid BookId { get; }
        public string Title { get; } = string.Empty;
        public bool IsReserved { get; } = true;
        public EventType EventType { get; }
        public DateTime CreatedDate { get; }
        public DateTime? ProcessedDate { get; set; }

        public BookReservedEvent(Guid bookId, string title, bool isReserved, EventType eventType, DateTime createdDate)
        {
            BookId = bookId;
            Title = title;
            IsReserved = isReserved;
            EventType = eventType;
            CreatedDate = createdDate;
        }
    }
}
