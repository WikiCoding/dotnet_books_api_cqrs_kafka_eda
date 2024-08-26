using BooksCommand.Domain.ValueObjects;

namespace BooksCommand.Events
{
    public record BookDeletedEvent(BookId BookId) : IDomainEvent;
}
