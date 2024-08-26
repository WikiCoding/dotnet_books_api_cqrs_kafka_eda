using BooksCommand.Broker;
using BooksCommand.Database;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace BooksCommand.Outbox
{
    public class OutbooxPublisher : BackgroundService
    {
        private readonly KafkaProducer _kafkaProducer;
        private const string TOPIC = "create_book";
        private readonly IServiceProvider _serviceProvider;

        public OutbooxPublisher(KafkaProducer kafkaProducer, IServiceProvider serviceProvider)
        {
            _kafkaProducer = kafkaProducer;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await ProcessOutbox(stoppingToken);

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }

        private async Task ProcessOutbox(CancellationToken stoppingToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<BooksDbContext>();

                if (dbContext is null) throw new Exception("dbcontext is null");

                var outboxEventListToProcess = await dbContext.OutBoxModels.Where(ob => ob.ProcessedDate == null).ToListAsync(stoppingToken);

                foreach (var outboxEvent in outboxEventListToProcess)
                {
                    // since the title is also unique: (this needs refactoring, like this is not ok...)
                    var bookDm = await dbContext.Books.Where(book => book.Title == outboxEvent.Title).FirstAsync();

                    outboxEvent.BookId = bookDm.Id;
                    outboxEvent.ProcessedDate = DateTime.UtcNow;

                    string bookSerialized = JsonSerializer.Serialize(outboxEvent);

                    await _kafkaProducer.ProduceAsync(TOPIC, bookSerialized);

                    await dbContext.SaveChangesAsync(stoppingToken);
                }
            }
        }
    }
}
