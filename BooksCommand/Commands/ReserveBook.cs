using BookApi.Command.Persistence;
using BooksCommand.Domain;
using BooksCommand.Domain.ValueObjects;
using BooksCommand.Events;
using BooksCommand.Persistence;
using MediatR;

namespace BooksCommand.Commands
{
    public class ReserveBook
    {
        public class ReserveBookCommand : IRequest<BookOutBoxDataModel>
        {
            public Guid BookId { get; init; }
        }

        public class ReverveBookHanlder : IRequestHandler<ReserveBookCommand, BookOutBoxDataModel>
        {
            private readonly IBookRepository _bookRepository;
            private readonly IBookFactory _bookFactory;

            public ReverveBookHanlder(IBookRepository bookRepository, IBookFactory bookFactory)
            {
                _bookRepository = bookRepository;
                _bookFactory = bookFactory;
            }

            public async Task<BookOutBoxDataModel> Handle(ReserveBookCommand request, CancellationToken cancellationToken) 
            {
                // here I should build the aggregate root, raise the proper domain event and then continue the logic as at CreateBook
                BookWriteDataModel? bookDm = await _bookRepository.FindBookById(request.BookId);

                if (bookDm == null) { throw new Exception("book not found"); }
                BookId bookId = new(bookDm.Id);
                BookTitle bookTitle = new(bookDm.Title);
                BookIsReserved bookIsReserved = new() { IsReserved = bookDm.IsReserved };

                Book book = _bookFactory.Create(bookId, bookTitle, bookIsReserved);

                BookReservedEvent bookReservedEvent = new(bookDm.Id, bookDm.Title, bookDm.IsReserved, EventType.BookReservedEvent, DateTime.UtcNow);

                // enforcing business rules on the domain.
                // The validation rules could be done directly here to simplify but I want to do it on the aggregate root this time for demo purposes.
                book.RaiseBookReservedEvent(bookReservedEvent);

                bookDm.IsReserved = true;

                BookOutBoxDataModel bookOutBoxDataModel = await _bookRepository.ReserveBook(book, cancellationToken);

                book.RaiseClearEvents();

                return bookOutBoxDataModel;
            }
        }
    }
}
