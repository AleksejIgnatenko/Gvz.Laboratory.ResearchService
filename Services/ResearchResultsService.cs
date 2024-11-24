using Gvz.Laboratory.ResearchService.Abstractions;
using Gvz.Laboratory.ResearchService.Dto;
using Gvz.Laboratory.ResearchService.Exceptions;
using Gvz.Laboratory.ResearchService.Models;
using OfficeOpenXml;
using Xceed.Words.NET;

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

            using (var memoryStream = new MemoryStream())
            {
                // Создаем новый документ Word
                using (var document = DocX.Create(memoryStream))
                {
                    // Добавляем заголовок
                    document.InsertParagraph("Результаты исследований").FontSize(20).Bold().SpacingAfter(20);

                    // Создаем таблицу
                    var table = document.InsertTable(manufacturers.Count + 1, 4);
                    table.Rows[0].Cells[0].Paragraphs[0].Append("Id").Bold();
                    table.Rows[0].Cells[1].Paragraphs[0].Append("Название исследования").Bold();
                    table.Rows[0].Cells[2].Paragraphs[0].Append("Номер партии").Bold();
                    table.Rows[0].Cells[3].Paragraphs[0].Append("Результат").Bold();

                    // Заполняем таблицу данными
                    for (int i = 0; i < manufacturers.Count; i++)
                    {
                        table.Rows[i + 1].Cells[0].Paragraphs[0].Append(manufacturers[i].Id.ToString());
                        table.Rows[i + 1].Cells[1].Paragraphs[0].Append(manufacturers[i].Research.ResearchName);
                        table.Rows[i + 1].Cells[2].Paragraphs[0].Append(manufacturers[i].Party.BatchNumber.ToString());
                        table.Rows[i + 1].Cells[3].Paragraphs[0].Append(manufacturers[i].Result);
                    }

                    // Сохраняем документ
                    document.Save();
                }

                // Сбрасываем позицию потока
                memoryStream.Position = 0;
                return memoryStream;
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
