
using BookApi.Command.Domain.DDD;
using BooksCommand.Domain;
using BooksCommand.Persistence;

namespace BookApi.Command.Persistence
{
    public interface IBookRepository : IRepository<Book, BookWriteDataModel>
    {
        Task<BookWriteDataModel?> FindBookById(Guid bookId);
        Task<BookWriteDataModel> ReserveBook(BookWriteDataModel bookDm, CancellationToken cancellationToken);
    }
}
