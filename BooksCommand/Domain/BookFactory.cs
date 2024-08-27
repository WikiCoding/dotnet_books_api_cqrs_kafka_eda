using BooksCommand.Domain.ValueObjects;

namespace BooksCommand.Domain
{
    public class BookFactory : IBookFactory
    {
        public Book Create(BookId bookId, BookTitle bookTitle, BookIsReserved bookIsReserved)
        {
            Book book = new(bookId, bookTitle, bookIsReserved);

            // Moving this logic to the Command Handler to avoid event duplication when building the aggregate for updates
            //BookCreatedEvent evnt = new(bookId.Id, bookTitle.Title, false, true, DateTime.UtcNow);

            //book.RaiseBookCreatedEvent(evnt);

            return book;
        }
    }
}
