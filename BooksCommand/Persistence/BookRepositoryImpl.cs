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

        public async Task<BookWriteDataModel?> FindBookById(int bookId)
        {
            return await _dbContext.Books.Where(book => book.Id == bookId).FirstOrDefaultAsync();
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

            CreatedBookEvent createdBookEvent = new() 
            { 
                BookId = bookWriteDataModel.Id, 
                Title = bookWriteDataModel.Title, 
                IsReserved = bookWriteDataModel.IsReserved, 
                CreatedDate = DateTime.UtcNow 
            };

            aggregateRoot.RaiseBookCreatedEvent(createdBookEvent);

            await _publisher.Publish(createdBookEvent, cancellationToken);

            return bookWriteDataModel;
        }

        public async Task<BookWriteDataModel> ReserveBook(BookWriteDataModel bookDm, CancellationToken cancellationToken)
        {
            BookId bookId = new(bookDm.Id);
            BookTitle bookTitle = new(bookDm.Title);
            BookIsReserved bookIsReserved = new();

            Book book = new(bookId, bookTitle, bookIsReserved);
            
            BookOutBoxDataModel bookOutBoxDataModel = new()
            {
                Title = bookDm.Title,
                IsReserved = true,
                IsCreationEvent = false,
                CreatedDate = DateTime.UtcNow
            };

            _dbContext.Add(bookOutBoxDataModel);

            await _dbContext.SaveChangesAsync(cancellationToken);

            BookReservedEvent bookReservedEvent = new()
            {
                BookId = bookDm.Id,
                Title= bookDm.Title,
                IsReserved = true,
                CreatedDate= DateTime.UtcNow
            };
            book.RaiseBookCreatedEvent(bookReservedEvent);

            await _publisher.Publish(bookReservedEvent, cancellationToken);

            return bookDm;
        }
    }
}
