using Gvz.Laboratory.ResearchService.Abstractions;
using Gvz.Laboratory.ResearchService.Dto;
using Gvz.Laboratory.ResearchService.Exceptions;
using Gvz.Laboratory.ResearchService.Models;

namespace Gvz.Laboratory.ResearchService.Services
{
    public class ResearchService : IResearchService
    {
        private readonly IResearchRepository _researchRepository;
        private readonly IResearchKafkaProducer _researchKafkaProducer;

        public ResearchService(IResearchRepository researchRepository, IResearchKafkaProducer researchKafkaProducer)
        {
            _researchRepository = researchRepository;
            _researchKafkaProducer = researchKafkaProducer;
        }

        public async Task<Guid> CreateResearchAsync(Guid id, string name, Guid productId)
        {
            var (errors, research) = ResearchModel.Create(id, name);
            if (errors.Count > 0)
            {
                throw new ResearchValidationException(errors);
            }

            var researchId = await _researchRepository.CreateResearchAsync(research, productId);

            ResearchDto researchDto = new ResearchDto
            {
                Id = research.Id,
                ResearchName = research.ResearchName,
            };

            await _researchKafkaProducer.SendToKafkaAsync(researchDto, "add-research-topic");

            return researchId;
        }

        public async Task<(List<ResearchModel> researches, int numberResearches)> GetResearchesByProductIdForPageAsync(Guid productId, int pageNumber)
        {
            return await _researchRepository.GetResearchesByProductIdForPageAsync(productId, pageNumber);
        }

        public async Task<(List<ResearchModel> researches, int numberResearches)> GetResearchesForPageAsync(int pageNumber)
        {
            return await _researchRepository.GetResearchesForPageAsync(pageNumber);
        }

        public async Task<Guid> UpdateResearchAsync(Guid id, string name, Guid productId)
        {
            var (errors, research) = ResearchModel.Create(id, name);
            if (errors.Count > 0)
            {
                throw new ResearchValidationException(errors);
            }

            var researchId = await _researchRepository.UpdateResearchAsync(research, productId);

            ResearchDto researchDto = new ResearchDto
            {
                Id = research.Id,
                ResearchName = research.ResearchName,
            };

            await _researchKafkaProducer.SendToKafkaAsync(researchDto, "update-research-topic");

            return researchId;
        }

        public async Task DeleteResearchAsync(List<Guid> ids)
        {
            await _researchRepository.DeleteResearchesAsync(ids);
            await _researchKafkaProducer.SendToKafkaAsync(ids, "delete-research-topic");
        }
    }
}
