using Confluent.Kafka;
using Gvz.Laboratory.ResearchService.Abstractions;
using Gvz.Laboratory.ResearchService.Dto;
using System.Text.Json;

namespace Gvz.Laboratory.ResearchService.Kafka
{
    public class ResearchKafkaProducer : IResearchKafkaProducer
    {
        private readonly IProducer<Null, string> _producer;

        public ResearchKafkaProducer(IProducer<Null, string> producer)
        {
            _producer = producer;
        }

        public async Task SendToKafkaAsync(ResearchDto research, string topic)
        {
            var serializedResearch = JsonSerializer.Serialize(research);
            await _producer.ProduceAsync(topic, new Message<Null, string> { Value = serializedResearch });
        }

        public async Task SendToKafkaAsync(List<Guid> ids, string topic)
        {
            var serializedId = JsonSerializer.Serialize(ids);
            await _producer.ProduceAsync(topic, new Message<Null, string> { Value = serializedId });
        }
    }
}
