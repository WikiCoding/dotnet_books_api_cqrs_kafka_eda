using Confluent.Kafka;

namespace BooksCommand.Broker
{
    public class KafkaProducer
    {
        private readonly IConfiguration _configuration;
        private readonly IProducer<Null, string> _producer;
        //private readonly ILogger<KafkaProducer> _logger;

        public KafkaProducer(IConfiguration configuration)
        {
            //_logger = logger;
            _configuration = configuration;
            var producerConfig = new ProducerConfig { BootstrapServers = _configuration["Kafka:BootstrapServers"] };

            _producer = new ProducerBuilder<Null, string>(producerConfig).Build();
        }

        public async Task<string> ProduceAsync(string topic, string message)
        {
            var kafkaMessage = new Message<Null, string>() { Value = message };

            DeliveryResult<Null, string> ack = await _producer.ProduceAsync(topic, kafkaMessage);

            //_logger.LogInformation("Kafka produced {ack.Value}", ack.Value);

            return ack.Value;
        }
    }
}
