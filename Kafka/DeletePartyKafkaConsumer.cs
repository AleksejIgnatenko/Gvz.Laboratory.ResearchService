using Confluent.Kafka;
using Gvz.Laboratory.ResearchService.Abstractions;
using Serilog;
using System.Text.Json;

namespace Gvz.Laboratory.ResearchService.Kafka
{
    public class DeletePartyKafkaConsumer : IHostedService
    {
        private readonly ConsumerConfig _config;
        private IConsumer<Ignore, string> _consumer;
        private CancellationTokenSource _cts;
        private readonly IPartyRepository _partyRepository;
        public DeletePartyKafkaConsumer(ConsumerConfig config, IPartyRepository partyRepository)
        {
            _config = config;
            _partyRepository = partyRepository;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _consumer = new ConsumerBuilder<Ignore, string>(_config).Build();
            _consumer.Subscribe("delete-party-topic");
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
                        var deleteParties = JsonSerializer.Deserialize<List<Guid>>(cr.Message.Value)
                            ?? throw new InvalidOperationException("Deserialization failed: Party is null.");
                        await _partyRepository.DeletePartiesAsync(deleteParties);
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
