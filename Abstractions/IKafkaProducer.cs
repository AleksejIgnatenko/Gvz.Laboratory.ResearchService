using Gvz.Laboratory.ResearchService.Dto;

namespace Gvz.Laboratory.ResearchService.Abstractions
{
    public interface IKafkaProducer
    {
        Task SendToKafkaAsync(List<Guid> ids, string topic);
        Task SendToKafkaAsync(ResearchDto research, string topic);
        Task SendToKafkaAsync(ResearchResultsDto researchResults, string topic);
    }
}