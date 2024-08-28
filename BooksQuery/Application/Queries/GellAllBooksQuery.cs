using BooksQuery.Database;
using BooksQuery.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BooksQuery.Application.Queries
{
    public class GellAllBooksQuery
    {
        public record Query(int page = 1, int pageSize = 10) : IRequest<IEnumerable<Book>>;

        public class QueryHandler : IRequestHandler<Query, IEnumerable<Book>>
        {
            private readonly BookReadDbContext _dbContext;

            public QueryHandler(BookReadDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<IEnumerable<Book>> Handle(Query request, CancellationToken cancellationToken)
            {
                var dbQuery = _dbContext.Books.AsQueryable();

                // paging
                var skip = (request.page - 1) * request.pageSize;
                var take = request.pageSize;

                //var result = await dbQuery.Skip(skip).Take(take).ToListAsync(cancellationToken);

                //return await _dbContext.Books.ToListAsync();
                return await dbQuery.Skip(skip).Take(take).ToListAsync(cancellationToken); ;
            }
        }
    }
}
