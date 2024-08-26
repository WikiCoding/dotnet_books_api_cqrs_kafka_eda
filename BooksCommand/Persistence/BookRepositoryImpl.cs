using BookApi.Command.Persistence;
using BooksCommand.Database;
using BooksCommand.Domain;
using BooksCommand.Domain.ValueObjects;
using BooksCommand.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;

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
                IsCreationEvent = true,
                CreatedDate = DateTime.UtcNow
            };

            _dbContext.Add(writeDm);
            _dbContext.Add(outboxDm);

            await _dbContext.SaveChangesAsync(cancellationToken);

            BookCreatedEvent createdBookEvent = new(writeDm.Id, writeDm.Title, writeDm.IsReserved, outboxDm.IsCreationEvent, DateTime.UtcNow);

            // I'll use the outbox pattern so no need to create this coupling here
            //await _publisher.Publish(createdBookEvent, cancellationToken);

            return writeDm;
        }

        public async Task<BookWriteDataModel> ReserveBook(BookWriteDataModel bookDm, CancellationToken cancellationToken)
        {
            //BookId bookId = new(bookDm.Id);
            //BookTitle bookTitle = new(bookDm.Title);
            //BookIsReserved bookIsReserved = new();

            //Book book = new(bookId, bookTitle, bookIsReserved);
            
            BookOutBoxDataModel outboxDm = new()
            {
                Title = bookDm.Title,
                IsReserved = true,
                IsCreationEvent = false,
                CreatedDate = DateTime.UtcNow
            };

            _dbContext.Add(outboxDm);

            await _dbContext.SaveChangesAsync(cancellationToken);

            BookReservedEvent bookReservedEvent = new(bookDm.Id, bookDm.Title, outboxDm.IsReserved, outboxDm.IsCreationEvent, outboxDm.CreatedDate);

            //book.RaiseBookCreatedEvent(bookReservedEvent);

            // I'll use the outbox pattern so no need to create this coupling here
            //await _publisher.Publish(bookReservedEvent, cancellationToken);

            return bookDm;
        }
    }
}
