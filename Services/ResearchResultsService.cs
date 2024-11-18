using Gvz.Laboratory.ResearchService.Abstractions;
using Gvz.Laboratory.ResearchService.Dto;
using Gvz.Laboratory.ResearchService.Exceptions;
using Gvz.Laboratory.ResearchService.Models;

namespace Gvz.Laboratory.ResearchService.Services
{
    public class ResearchResultsService : IResearchResultsService
    {
        private readonly IResearchResultsRepository _researchResultsRepository;
        private readonly IKafkaProducer _kafkaProducer;
        public ResearchResultsService(IResearchResultsRepository researchResultsRepository, IKafkaProducer kafkaProducer)
        {
            _researchResultsRepository = researchResultsRepository;
            _kafkaProducer = kafkaProducer;
        }

        //public async Task<Guid> (Guid id, Guid researchId, Guid partyId, string result)
        //{

        //}

        public async Task<(List<ResearchResultModel> researchResults, int numberResearchResults)> GetResearchResultsByResearchIdForPageAsync(Guid researchId, int pageNumber)
        {
            return await _researchResultsRepository.GetResearchResultsByResearchIdForPageAsync(researchId, pageNumber);
        }

        public async Task<Guid> UpdateResearchResultAsync(Guid id, string result)
        {
            var (errors, researchResult) = ResearchResultModel.Create(id, result);
            if (errors.Count > 0)
            {
                throw new ResearchValidationException(errors);
            }

            var researchId = await _researchResultsRepository.UpdateResearchResultAsync(researchResult);

            ResearchResultsDto researchResultsDto = new ResearchResultsDto
            {
                Id = researchResult.Id,
                Result = researchResult.Result,
            };

            await _kafkaProducer.SendToKafkaAsync(researchResultsDto, "update-researchResult-topic");

            return researchId;
        }

        public async Task DeleteResearchResultsAsync(List<Guid> ids)
        {
            await _researchResultsRepository.DeleteResearchResultsAsync(ids);
            await _kafkaProducer.SendToKafkaAsync(ids, "delete-researchResult-topic");
        }
    }
}
