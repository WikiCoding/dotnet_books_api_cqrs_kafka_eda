namespace BooksQuery.Contracts
{
    public record BookResponseWithPaging(IEnumerable<BookResponse> BookResponses, Paging Paging);

}
