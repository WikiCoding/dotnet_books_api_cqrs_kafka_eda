namespace BooksQuery.Contracts
{
    public record BookResponse(string bookId, string title, bool isReserved);
}
