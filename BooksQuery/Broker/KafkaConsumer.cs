
using BooksQuery.Database;
using BooksQuery.Models;
using Confluent.Kafka;
using System.Text.Json;

namespace BooksQuery.Broker
{
    public class KafkaConsumer : BackgroundService
    {
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private const string TOPIC = "create_book";

        public KafkaConsumer(IConfiguration configuration, IServiceProvider serviceProvider)
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

            Console.WriteLine($"Received message {message}");

            BookReadDataModel bookReadDataModel = JsonSerializer.Deserialize<BookReadDataModel>(message)!;

            Book book = new() { Title = bookReadDataModel.Title, IsReserved = bookReadDataModel.IsReserved };

            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<BookReadDbContext>();

                dbContext!.Add(book);

                await dbContext.SaveChangesAsync(stoppingToken);
            }
        }
    }
}
