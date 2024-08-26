using BooksCommand.Events;

namespace BooksCommand.Domain.DDD
{
    public interface IAggregateRoot
    {
        void RaiseBookCreatedEvent(BookCreatedEvent domainEvent);
        void RaiseBookReservedEvent(BookReservedEvent domainEvent);
        void RaiseClearEvents();
    }
}
