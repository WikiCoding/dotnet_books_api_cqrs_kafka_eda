using Amazon.Runtime.Internal;
using BooksQuery.Contracts;
using BooksQuery.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BooksQuery.Queries
{
    public class GetBookByIdQuery
    {
        public record Query(string id) : IRequest<BookResponse>;

        internal sealed class QueryHandler : IRequestHandler<Query, BookResponse>
        {
            private readonly BookReadDbContext _dbContext;

            public QueryHandler(BookReadDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<BookResponse> Handle(Query request, CancellationToken cancellationToken)
            {
                var book = await _dbContext.Books.Where(book => book.BookId == request.id).FirstOrDefaultAsync(cancellationToken);

                if (book == null) throw new ArgumentOutOfRangeException("book not found");

                return new BookResponse(book.BookId, book.Title, book.IsReserved);
            }
        }
    }
}
