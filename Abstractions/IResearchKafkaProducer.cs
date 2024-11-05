using Gvz.Laboratory.ResearchService.Dto;

namespace Gvz.Laboratory.ResearchService.Abstractions
{
    public interface IResearchKafkaProducer
    {
        Task SendToKafkaAsync(List<Guid> ids, string topic);
        Task SendToKafkaAsync(ResearchDto research, string topic);
    }
}