﻿using BooksCommand.Domain.Events;
using BooksCommand.Infrastructure.Broker;
using BooksCommand.Persistence.Context;
using BooksCommand.Persistence.Datamodels;
using MediatR;
using System.Text.Json;

namespace BooksCommand.Domain.Events.EventHandlers
{
    // This class is not being used at the moment since I'm using the outbox pattern but I want to keep it here as example
    public class BookCreatedEventHandler : INotificationHandler<BookCreatedEvent>
    {
        private readonly KafkaProducer _kafkaProducer;
        private const string TOPIC = "create_book";
        private readonly BooksDbContext _dbContext;

        public BookCreatedEventHandler(KafkaProducer kafkaProducer, BooksDbContext booksDbContext)
        {
            _kafkaProducer = kafkaProducer;
            _dbContext = booksDbContext;
        }

        public async Task Handle(BookCreatedEvent notification, CancellationToken cancellationToken)
        {
            BookOutBoxDataModel bookOutbox = new()
            {
                BookId = notification.BookId,
                Title = notification.Title,
                IsReserved = notification.IsReserved,
                ProcessedDate = DateTime.UtcNow,
            };

            // maybe dispatch message here (for now)
            string bookSerialized = JsonSerializer.Serialize(bookOutbox);

            await _kafkaProducer.ProduceAsync(TOPIC, bookSerialized);

            _dbContext.Add(bookOutbox);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
