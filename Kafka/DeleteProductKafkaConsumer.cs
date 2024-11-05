using Confluent.Kafka;
using Gvz.Laboratory.ResearchService.Abstractions;
using Gvz.Laboratory.ResearchService.Dto;
using Serilog;
using System.Text.Json;

namespace Gvz.Laboratory.ResearchService.Kafka
{
    public class DeleteProductKafkaConsumer : IHostedService
    {
        private readonly ConsumerConfig _config;
        private IConsumer<Ignore, string> _consumer;
        private CancellationTokenSource _cts;
        private readonly IProductRepository _productRepository;

        public DeleteProductKafkaConsumer(ConsumerConfig config, IProductRepository productRepository)
        {
            _config = config;
            _productRepository = productRepository;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _consumer = new ConsumerBuilder<Ignore, string>(_config).Build();

            _consumer.Subscribe("delete-product-topic");

            Task.Run(() => ConsumeMessages(cancellationToken));

            return Task.CompletedTask;
        }

        private async void ConsumeMessages(CancellationToken cancellationToken)
        {
            try
            {
                while (true)
                {
                    try
                    {
                        var cr = _consumer.Consume(cancellationToken);

                        var deleteProducts = JsonSerializer.Deserialize<List<Guid>>(cr.Message.Value)
                            ?? throw new InvalidOperationException("Deserialization failed: Product is null.");

                        await _productRepository.DeleteProductsAsync(deleteProducts);

                    }
                    catch (ConsumeException e)
                    {
                        Log.Error($"Error occurred: {e.Error.Reason}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _consumer.Close();
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex.Message);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
