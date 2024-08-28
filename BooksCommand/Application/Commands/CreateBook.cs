using BooksCommand.Domain;
using BooksCommand.Domain.Events;
using BooksCommand.Domain.ValueObjects;
using BooksCommand.Persistence.Datamodels;
using BooksCommand.Persistence.Repository;
using MediatR;

namespace BooksCommand.Application.Commands
{
    public class CreateBook
    {
        public class CreateBookCommand : IRequest<BookWriteDataModel>
        {
            public string Title { get; init; } = string.Empty;
        }

        public class CreateBookHandler : IRequestHandler<CreateBookCommand, BookWriteDataModel>
        {
            private readonly IBookRepository _bookRepository;
            private readonly IBookFactory _bookFactory;

            public CreateBookHandler(IBookRepository bookRepository, IBookFactory bookFactory)
            {
                _bookRepository = bookRepository;
                _bookFactory = bookFactory;
            }

            public async Task<BookWriteDataModel> Handle(CreateBookCommand request, CancellationToken cancellationToken)
            {
                // create book aggregate
                BookId bookId = new(Guid.NewGuid());
                BookTitle bookTitle = new(request.Title);
                BookIsReserved bookIsReserved = new();
                Book book = _bookFactory.Create(bookId, bookTitle, bookIsReserved);

                BookCreatedEvent evnt = new(bookId.Id, bookTitle.Title, false, EventType.BookCreatedEvent, DateTime.UtcNow);

                // enforcing business rules on the domain
                // The validation rules could be done directly here to simplify but I want to do it on the aggregate root this time for demo purposes.
                book.RaiseBookCreatedEvent(evnt);

                BookWriteDataModel bookDm = await _bookRepository.SaveBook(book, cancellationToken);

                book.RaiseClearEvents();

                return bookDm;
            }
        }
    }
}
