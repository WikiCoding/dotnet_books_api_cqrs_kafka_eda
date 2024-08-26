using BookApi.Command.Persistence;
using BooksCommand.Database;
using BooksCommand.Domain;
using BooksCommand.Domain.ValueObjects;
using BooksCommand.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BooksCommand.Persistence
{
    public class BookRepositoryImpl : IBookRepository
    {
        private readonly BooksDbContext _dbContext;
        private readonly IPublisher _publisher;

        public BookRepositoryImpl(BooksDbContext dbContext, IPublisher publisher)
        {
            _dbContext = dbContext;
            _publisher = publisher;
        }

        public async Task<BookWriteDataModel?> FindBookById(Guid bookId)
        {
            return await _dbContext.Books.Where(book => book.Id == bookId).FirstOrDefaultAsync();
        }

        public async Task<BookWriteDataModel> SaveBook(Book aggregateRoot, CancellationToken cancellationToken)
        {
            BookWriteDataModel writeDm = new() { Title = aggregateRoot.Title.Title, IsReserved = false };

            BookOutBoxDataModel outboxDm = new() 
            { 
                Title = writeDm.Title, 
                IsReserved = writeDm.IsReserved, 
                EventType = EventType.BookCreatedEvent,
                CreatedDate = DateTime.UtcNow
            };

            _dbContext.Add(writeDm);
            _dbContext.Add(outboxDm);

            await _dbContext.SaveChangesAsync(cancellationToken);

            BookCreatedEvent createdBookEvent = new(writeDm.Id, writeDm.Title, writeDm.IsReserved, outboxDm.EventType, DateTime.UtcNow);

            // I'll use the outbox pattern so no need to create this coupling here
            //await _publisher.Publish(createdBookEvent, cancellationToken);

            return writeDm;
        }

        public async Task<BookOutBoxDataModel> ReserveBook(Book book, CancellationToken cancellationToken)
        {            
            BookOutBoxDataModel outboxDm = new()
            {
                BookId = book.Id.Id,
                Title = book.Title.Title,
                IsReserved = true,
                EventType = EventType.BookReservedEvent,
                CreatedDate = DateTime.UtcNow
            };

            _dbContext.Add(outboxDm);

            await _dbContext.SaveChangesAsync(cancellationToken);

            // I'll use the outbox pattern so no need to create this coupling here
            //await _publisher.Publish(bookReservedEvent, cancellationToken);

            return outboxDm;
        }

        public async Task DeleteBook(BookId bookId, CancellationToken cancellationToken)
        {
            BookWriteDataModel? bookDm = await FindBookById(bookId.Id);

            if (bookDm == null) throw new ArgumentOutOfRangeException("book not found");

            var outboxDm = new BookOutBoxDataModel() 
            { 
                BookId = bookDm.Id,
                Title = bookDm.Title,
                IsReserved = bookDm.IsReserved,
                CreatedDate = DateTime.UtcNow,
                EventType = EventType.BookDeletedEvent,

            };

            _dbContext.Remove(bookDm);

            _dbContext.OutBoxModels.Add(outboxDm);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
