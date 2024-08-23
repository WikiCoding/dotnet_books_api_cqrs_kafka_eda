using BooksQuery.Database;
using BooksQuery.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BooksQuery.Queries
{
    public class GellAllBooksQuery
    {
        public class Query : IRequest<IEnumerable<Book>>
        {

        }

        public class QueryHandler : IRequestHandler<Query, IEnumerable<Book>>
        {
            private readonly BookReadDbContext _dbContext;

            public QueryHandler(BookReadDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<IEnumerable<Book>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _dbContext.Books.ToListAsync();
            }
        }
    }
}
