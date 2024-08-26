using BooksCommand.Domain.ValueObjects;
using BooksCommand.Events;

namespace BooksCommand.Domain
{
    public class BookFactory : IBookFactory
    {
        public Book Create(BookId bookId, BookTitle bookTitle, BookIsReserved bookIsReserved)
        {
            Book book = new(bookId, bookTitle, bookIsReserved);

            BookCreatedEvent evnt = new(bookId.Id, bookTitle.Title, false, true, DateTime.UtcNow);

            book.RaiseBookCreatedEvent(evnt);

            return book;
        }
    }
}
