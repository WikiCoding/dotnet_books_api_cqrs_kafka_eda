using BooksCommand.Broker;
using BooksCommand.Persistence;
using MediatR;
using System.Text.Json;

namespace BooksCommand.Events
{
    public class BookCreatedEventHandler : INotificationHandler<CreatedBookEvent>
    {
        private readonly KafkaProducer _kafkaProducer;
        private const string TOPIC = "create_book";

        public BookCreatedEventHandler(KafkaProducer kafkaProducer)
        {
            _kafkaProducer = kafkaProducer;
        }

        public async Task Handle(CreatedBookEvent notification, CancellationToken cancellationToken)
        {
            BookWriteDataModel bookDm = new()
            {
                Id = notification.BookId,
                Title = notification.Title,
                IsReserved = notification.IsReserved,
            };

            // maybe dispatch message here (for now)
            string bookSerialized = JsonSerializer.Serialize(bookDm);

            await _kafkaProducer.ProduceAsync(TOPIC, bookSerialized);
        }
    }
}
