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

            public CreateBookHandler(IBookRepository bookRepository)
            {
                _bookRepository = bookRepository;
            }

            public async Task<BookWriteDataModel> Handle(CreateBookCommand request, CancellationToken cancellationToken)
            {
                // create book aggregate
                BookId bookId = new(0);
                BookTitle bookTitle = new(request.Title);
                BookIsReserved bookIsReserved = new();
                Book book = new(bookId, bookTitle, bookIsReserved);

                BookWriteDataModel bookDm = await _bookRepository.SaveBook(book, cancellationToken);

                return bookDm;
            }
        }
    }
}
