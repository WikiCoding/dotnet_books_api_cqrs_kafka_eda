using BookApi.Command.Persistence;
using BooksCommand.Database;
using BooksCommand.Domain;

namespace BooksCommand.Persistence
{
    public class BookRepositoryImpl : IBookRepository
    {
        private readonly BooksDbContext _dbContext;

        public BookRepositoryImpl(BooksDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BookWriteDataModel> SaveBook(Book aggregateRoot, CancellationToken cancellationToken)
        {
            BookWriteDataModel bookWriteDataModel = new() { Title = aggregateRoot.Title.Title, IsReserved = false };

            BookOutBoxDataModel bookOutBoxDataModel = new() 
            { 
                Title = bookWriteDataModel.Title, 
                IsReserved = bookWriteDataModel.IsReserved, 
                IsCreationEvent = true,
                CreatedDate = DateTime.UtcNow
            };

            _dbContext.Add(bookWriteDataModel);
            _dbContext.Add(bookOutBoxDataModel);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return bookWriteDataModel;
        }
    }
}
