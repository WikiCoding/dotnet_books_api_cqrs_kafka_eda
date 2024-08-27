using BooksCommand.Domain;
using BooksCommand.Domain.Events;
using BooksCommand.Domain.ValueObjects;
using BooksCommand.Persistence.Datamodels;
using BooksCommand.Persistence.Repository;
using MediatR;

namespace BooksCommand.Commands
{
    public class DeleteBook
    {
        public record DeleteBookCommand(Guid Id) : IRequest;

        internal sealed class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand>
        {
            private readonly IBookRepository _bookRepository;
            private readonly IBookFactory _bookFactory;

            public DeleteBookCommandHandler(IBookRepository bookRepository, IBookFactory bookFactory)
            {
                _bookRepository = bookRepository;
                _bookFactory = bookFactory;
            }

            public async Task Handle(DeleteBookCommand request, CancellationToken cancellationToken)
            {
                // Again, I could really avoid having to build the Aggregate root to raise event and clear event list
                BookWriteDataModel? bookDm = await _bookRepository.FindBookById(request.Id);

                if (bookDm == null) throw new Exception("book not found");

                BookId bookId = new BookId(bookDm.Id);
                Book book = _bookFactory.Create(bookId, new BookTitle(bookDm.Title), new BookIsReserved() { IsReserved = bookDm.IsReserved });

                book.RaiseBookDeletedEvent(new BookDeletedEvent(bookId));
                
                await _bookRepository.DeleteBook(bookId, cancellationToken);

                book.RaiseClearEvents();
            }
        }

    }
}
