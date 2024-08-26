﻿using BooksCommand.Domain.DDD;
using BooksCommand.Domain.ValueObjects;
using BooksCommand.Events;

namespace BooksCommand.Domain
{
    public class Book : IAggregateRoot
    {
        private readonly List<IDomainEvent> _events = [];
        public IReadOnlyList<IDomainEvent> DomainEvents => _events.AsReadOnly();

        public BookId Id { get; private set; }
        public BookTitle Title { get; private set; }
        public BookIsReserved IsReserved { get; private set; }

        public Book(BookId id, BookTitle title, BookIsReserved isReserved)
        {
            Id = id;
            Title = title;
            IsReserved = isReserved;
        }

        public void RaiseBookCreatedEvent(BookCreatedEvent domainEvent)
        {
            // handle any business logic related with this Aggregate.
            _events.Add(domainEvent);
        }

        public void RaiseBookReservedEvent(BookReservedEvent domainEvent)
        {
            // handle any business logic related with this Aggregate.
            _events.Add(domainEvent);
        }

        public void RaiseClearEvents()
        {
            _events.Clear();
        }
    }
}
