using BooksCommand.Domain.DDD;
using BooksCommand.Domain.ValueObjects;
using BooksCommand.Events;

namespace BooksCommand.Domain
{
    public class Book : IAggregateRoot
    {
        private readonly List<IDomainEvent> _events = [];
        public IReadOnlyList<IDomainEvent> DomainEvents => _events.AsReadOnly();

        public BookId Id { get; set; }
        public BookTitle Title { get; set; }
        public BookIsReserved IsReserved { get; set; }

        public Book(BookId id, BookTitle title, BookIsReserved isReserved)
        {
            Id = id;
            Title = title;
            IsReserved = isReserved;
        }

        public void RaiseBookCreatedEvent(IDomainEvent domainEvent)
        {
            _events.Add(domainEvent);
        }

        public void RaiseClearEvents()
        {
            _events.Clear();
        }
    }
}
