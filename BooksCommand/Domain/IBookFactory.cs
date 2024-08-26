using BooksCommand.Domain.ValueObjects;

namespace BooksCommand.Domain
{
    public interface IBookFactory
    {
        Book Create(BookId bookId, BookTitle bookTitle, BookIsReserved bookIsReserved);
    }
}
