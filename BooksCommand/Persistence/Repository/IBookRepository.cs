using BookApi.Command.Domain.DDD;
using BooksCommand.Domain;
using BooksCommand.Domain.ValueObjects;
using BooksCommand.Persistence.Datamodels;

namespace BooksCommand.Persistence.Repository
{
    public interface IBookRepository : IRepository<Book, BookWriteDataModel>
    {
        Task<BookWriteDataModel?> FindBookById(Guid bookId);
        Task<BookOutBoxDataModel> ReserveBook(Book book, CancellationToken cancellationToken);
        Task DeleteBook(BookId bookId, CancellationToken cancellationToken);
    }
}
