using BooksCommand.Domain.ValueObjects;

namespace BooksCommand.Domain.Events
{
    public record BookDeletedEvent(BookId BookId) : IDomainEvent;
}
