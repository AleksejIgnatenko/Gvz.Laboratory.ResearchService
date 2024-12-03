using Gvz.Laboratory.ResearchService.Abstractions;
using Gvz.Laboratory.ResearchService.Dto;
using Gvz.Laboratory.ResearchService.Exceptions;
using Gvz.Laboratory.ResearchService.Models;
using OfficeOpenXml;

namespace Gvz.Laboratory.ResearchService.Services
{
    public class ResearchService : IResearchService
    {
        private readonly IResearchRepository _researchRepository;
        private readonly IResearchResultsService _researchResultsService;
        private readonly IKafkaProducer _researchKafkaProducer;

        public ResearchService(IResearchRepository researchRepository, IKafkaProducer researchKafkaProducer, IResearchResultsService resultsService)
        {
            _researchRepository = researchRepository;
            _researchKafkaProducer = researchKafkaProducer;
            _researchResultsService = resultsService;
        }

        public async Task<Guid> CreateResearchAsync(Guid id, string name, Guid productId)
        {
            var (errors, research) = ResearchModel.Create(id, name);
            if (errors.Count > 0)
            {
                throw new ResearchValidationException(errors);
            }

            var researchId = await _researchRepository.CreateResearchAsync(research, productId);

            var researchResultId = await _researchResultsService.AddResearchResultToParties(research.Id, productId);

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

        public async Task<(List<ResearchModel> researches, int numberResearches)> SearchResearchesAsync(string searchQuery, int pageNumber)
        {
            return await _researchRepository.SearchResearchesAsync(searchQuery, pageNumber);
        }

        public async Task<MemoryStream> ExportResearchesToExcelAsync()
        {
            var manufacturers = await _researchRepository.GetResearchesAsync();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Manufacturers");

                worksheet.Cells[1, 1].Value = "Id";
                worksheet.Cells[1, 2].Value = "Название исследования";
                worksheet.Cells[1, 3].Value = "Продукт";

                for (int i = 0; i < manufacturers.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = manufacturers[i].Id;
                    worksheet.Cells[i + 2, 2].Value = manufacturers[i].ResearchName;
                    worksheet.Cells[i + 2, 3].Value = manufacturers[i].Product.ProductName;
                }

                worksheet.Cells.AutoFitColumns();

                var stream = new MemoryStream();
                await package.SaveAsAsync(stream);

                stream.Position = 0; // Сбрасываем поток
                return stream;
            }
        }

        public async Task<Guid> UpdateResearchAsync(Guid id, string name, Guid productId)
        {
            var (errors, research) = ResearchModel.Create(id, name);
            if (errors.Count > 0)
            {
                throw new ResearchValidationException(errors);
            }

            var researchEntity = await _researchRepository.UpdateResearchAsync(research, productId);

            if (researchEntity.Product.Id != productId)
            {
                var researchResultId = await _researchResultsService.AddResearchResultToParties(research.Id, productId);
            }

            ResearchDto researchDto = new ResearchDto
            {
                Id = research.Id,
                ResearchName = research.ResearchName,
            };

            await _researchKafkaProducer.SendToKafkaAsync(researchDto, "update-research-topic");

            return researchEntity.Id;
        }

        public async Task DeleteResearchAsync(List<Guid> ids)
        {
            await _researchRepository.DeleteResearchesAsync(ids);
            await _researchKafkaProducer.SendToKafkaAsync(ids, "delete-research-topic");
        }
    }
}
