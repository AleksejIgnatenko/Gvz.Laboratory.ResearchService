using Gvz.Laboratory.ResearchService.Abstractions;
using Gvz.Laboratory.ResearchService.Dto;
using Gvz.Laboratory.ResearchService.Exceptions;
using Gvz.Laboratory.ResearchService.Models;
using OfficeOpenXml;

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

        public async Task<(List<ResearchResultModel> researchResults, int numberResearchResults)> GetResearchResultsByResearchIdForPageAsync(Guid researchId, int pageNumber)
        {
            return await _researchResultsRepository.GetResearchResultsByResearchIdForPageAsync(researchId, pageNumber);
        }

        public async Task<(List<ResearchResultModel> researchResults, int numberResearchResults)> GetResearchResultsByPartyIdForPageAsync(Guid partyId, int pageNumber)
        {
            return await _researchResultsRepository.GetResearchResultsByPartyIdForPageAsync(partyId, pageNumber);
        }

        public async Task<(List<ResearchResultModel> researchResults, int numberResearchResults)> SearchResearchResultsAsync(string searchQuery, int pageNumber)
        {
            return await _researchResultsRepository.SearchResearchResultsAsync(searchQuery, pageNumber);
        }

        public async Task<MemoryStream> ExportResearchResultsToExcelAsync()
        {
            var manufacturers = await _researchResultsRepository.GetResearchResultsAsync();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Manufacturers");

                worksheet.Cells[1, 1].Value = "Id";
                worksheet.Cells[1, 2].Value = "Название исследования";
                worksheet.Cells[1, 3].Value = "Номер партии";
                worksheet.Cells[1, 4].Value = "Результат";

                for (int i = 0; i < manufacturers.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = manufacturers[i].Id;
                    worksheet.Cells[i + 2, 2].Value = manufacturers[i].Research.ResearchName;
                    worksheet.Cells[i + 2, 3].Value = manufacturers[i].Party.BatchNumber;
                    worksheet.Cells[i + 2, 4].Value = manufacturers[i].Result;
                }

                worksheet.Cells.AutoFitColumns();

                var stream = new MemoryStream();
                await package.SaveAsAsync(stream);

                stream.Position = 0; // Сбрасываем поток
                return stream;
            }
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
