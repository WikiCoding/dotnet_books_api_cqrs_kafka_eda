
using BooksQuery.Database;
using BooksQuery.Models;
using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace BooksQuery.Broker
{
    public class KafkaConsumer : BackgroundService
    {
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private const string TOPIC = "create_book";
        private readonly ILogger<KafkaConsumer> _logger;

        public KafkaConsumer(IConfiguration configuration, IServiceProvider serviceProvider, ILogger<KafkaConsumer> logger)
        {
            _configuration = configuration;
            _serviceProvider = serviceProvider;

            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                GroupId = "BookQueryConsumerGroup",
                AutoOffsetReset = AutoOffsetReset.Earliest,
            };

            _consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer.Subscribe(TOPIC);

            while (!stoppingToken.IsCancellationRequested)
            {
                await ConsumeMessage(stoppingToken);

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }

            _consumer.Close();
        }

        private async Task ConsumeMessage(CancellationToken stoppingToken)
        {
            var consumeResult = _consumer.Consume(TimeSpan.FromSeconds(3));

            if (consumeResult is null) { return; }

            var message = consumeResult.Message.Value;

            _logger.LogWarning("Received message {message}", message);

            BookReadDataModel bookReadDataModel = JsonSerializer.Deserialize<BookReadDataModel>(message)!;

            Book book = new() { Title = bookReadDataModel.Title, IsReserved = bookReadDataModel.IsReserved, EventId = bookReadDataModel.BookId };

            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<BookReadDbContext>();

                if (bookReadDataModel.IsCreationEvent)
                {
                    dbContext!.Add(book);
                    await dbContext.SaveChangesAsync(stoppingToken);
                }
                else
                {
                    var bookToUpdate = await dbContext!.Books.Where(book => book.EventId == bookReadDataModel.BookId).FirstOrDefaultAsync();

                    _logger.LogWarning("Book to Update: {bookToUpdate.EventId}", bookToUpdate.EventId);
                    _logger.LogWarning("Book to Update: {bookToUpdate.EventId}", bookReadDataModel.BookId);

                    bookToUpdate!.IsReserved = true;

                    int rowsAffected = await dbContext.SaveChangesAsync(stoppingToken);

                    _logger.LogWarning("rows affected {rowsAffected}", rowsAffected );
                }
            }
        }
    }
}
