using BooksCommand.Domain.Events;
using BooksCommand.Infrastructure.Broker;
using BooksCommand.Persistence.Context;
using BooksCommand.Persistence.Datamodels;
using MediatR;
using System.Text.Json;

namespace BooksCommand.Domain.Events.EventHandlers
{
    // This class is not being used at the moment since I'm using the outbox pattern but I want to keep it here as example
    public class BookReservedEventHandler : INotificationHandler<BookReservedEvent>
    {
        private readonly KafkaProducer _kafkaProducer;
        private const string TOPIC = "create_book";
        private readonly BooksDbContext _dbContext;

        public BookReservedEventHandler(KafkaProducer kafkaProducer, BooksDbContext dbContext)
        {
            _kafkaProducer = kafkaProducer;
            _dbContext = dbContext;
        }

        public async Task Handle(BookReservedEvent notification, CancellationToken cancellationToken)
        {
            BookOutBoxDataModel outBoxDataModel = new()
            {
                BookId = notification.BookId,
                Title = notification.Title,
                IsReserved = notification.IsReserved,
                CreatedDate = DateTime.UtcNow,
                EventType = EventType.BookReservedEvent,
                ProcessedDate = DateTime.UtcNow,
            };

            _dbContext.Add(outBoxDataModel);

            // maybe dispatch message here (for now)
            string bookSerialized = JsonSerializer.Serialize(outBoxDataModel);

            await _kafkaProducer.ProduceAsync(TOPIC, bookSerialized);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
