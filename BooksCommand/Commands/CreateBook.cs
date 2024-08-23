using BookApi.Command.Persistence;
using BooksCommand.Broker;
using BooksCommand.Domain;
using BooksCommand.Domain.ValueObjects;
using BooksCommand.Events;
using BooksCommand.Persistence;
using Confluent.Kafka;
using MediatR;
using System.Text.Json;

namespace BooksCommand.Commands
{
    public class CreateBook
    {
        public class CreateBookCommand : IRequest<BookWriteDataModel>
        {
            public string Title { get; set; } = string.Empty;
        }

        public class CreateBookHandler : IRequestHandler<CreateBookCommand, BookWriteDataModel>
        {
            private readonly IBookRepository _bookRepository;
            private readonly KafkaProducer _kafkaProducer;
            private const string TOPIC = "create_book";
            private readonly IConfiguration _configuration;

            public CreateBookHandler(IBookRepository bookRepository, IConfiguration configuration, KafkaProducer kafkaProducer)
            {
                _bookRepository = bookRepository;
                _configuration = configuration;
                _kafkaProducer = kafkaProducer;
            }

            public async Task<BookWriteDataModel> Handle(CreateBookCommand request, CancellationToken cancellationToken)
            {
                // create book aggregate
                BookId bookId = new(0);
                BookTitle bookTitle = new(request.Title);
                BookIsReserved bookIsReserved = new();
                Book book = new(bookId, bookTitle, bookIsReserved);

                // save book and receive book data model
                BookWriteDataModel bookDm = await _bookRepository.SaveBook(book, cancellationToken);

                // raise event
                book.RaiseBookCreatedEvent(new CreatedBookEvent() { BookId = bookDm.Id, Title = bookDm.Title, IsReserved = bookDm.IsReserved, CreatedDate = DateTime.Now });

                // maybe dispatch message here (for now)
                string bookSerialized = JsonSerializer.Serialize(bookDm);
                Console.WriteLine(bookSerialized);
                //KafkaProducer kafkaProducer = new(_configuration);
                string ack = await _kafkaProducer.ProduceAsync(TOPIC, bookSerialized);

                //Console.WriteLine("received ack: {ack}", ack);

                // return datamodel
                return bookDm;
            }
        }
    }
}
