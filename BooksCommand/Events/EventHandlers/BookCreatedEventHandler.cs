using BooksCommand.Broker;
using BooksCommand.Database;
using BooksCommand.Persistence;
using MediatR;
using System.Text.Json;

namespace BooksCommand.Events.EventHandlers
{
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
