using BookApi.Command.Persistence;
using BooksCommand.Persistence;
using MediatR;

namespace BooksCommand.Commands
{
    public class ReserveBook
    {
        public class ReserveBookCommand : IRequest<BookWriteDataModel>
        {
            public int BookId { get; init; }
        }

        public class ReverveBookHanlder : IRequestHandler<ReserveBookCommand, BookWriteDataModel>
        {
            private readonly IBookRepository _bookRepository;

            public ReverveBookHanlder(IBookRepository bookRepository)
            {
                _bookRepository = bookRepository;
            }

            public async Task<BookWriteDataModel> Handle(ReserveBookCommand request, CancellationToken cancellationToken) 
            {
                BookWriteDataModel? bookDm = await _bookRepository.FindBookById(request.BookId);

                if (bookDm == null) { throw new Exception("book not found"); }

                bookDm.IsReserved = true;

                BookWriteDataModel bookWriteDataModel = await _bookRepository.ReserveBook(bookDm, cancellationToken);

                return bookWriteDataModel;
            }
        }
    }
}
