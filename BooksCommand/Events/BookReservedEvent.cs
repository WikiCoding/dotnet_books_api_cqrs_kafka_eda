﻿namespace BooksCommand.Events
{
    public class BookReservedEvent : IDomainEvent
    {
        public Guid BookId { get; }
        public string Title { get; } = string.Empty;
        public bool IsReserved { get; } = true;
        public bool IsUpdateEvent { get; } = true;
        public DateTime CreatedDate { get; }
        public DateTime? ProcessedDate { get; set; }

        public BookReservedEvent(Guid bookId, string title, bool isReserved, bool isUpdateEvent, DateTime createdDate)
        {
            BookId = bookId;
            Title = title;
            IsReserved = isReserved;
            IsUpdateEvent = isUpdateEvent;
            CreatedDate = createdDate;
        }
    }
}
