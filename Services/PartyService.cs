using Gvz.Laboratory.ResearchService.Abstractions;
using Gvz.Laboratory.ResearchService.Dto;
using System.Drawing;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace Gvz.Laboratory.ResearchService.Services
{
    public class PartyService : IPartyService
    {
        private readonly IPartyRepository _partyRepository;
        private readonly IResearchRepository _researchRepository;
        private readonly IResearchResultsRepository _researchResultsRepository;
        private readonly IProductRepository _productRepository;

        public PartyService(IPartyRepository partyRepository, IResearchRepository researchRepository, IResearchResultsRepository researchResultsRepository, IProductRepository productRepository)
        {
            _partyRepository = partyRepository;
            _researchRepository = researchRepository;
            _researchResultsRepository = researchResultsRepository;
            _productRepository = productRepository;
        }

        public async Task<Guid> CreatePartyAsync(PartyDto partyDto)
        {
            var addPartyDtoId = await _partyRepository.CreatePartyAsync(partyDto);

            await _researchResultsRepository.CreateResearchResultsAsync(partyDto.Id, partyDto.ProductId);

            return partyDto.Id;
        }

        public async Task<Guid> UpdatePartyAsync(PartyDto partyDto)
        {
            var updatePartyEntity = await _partyRepository.UpdatePartyAsync(partyDto);
            if(!partyDto.ProductName.Equals(updatePartyEntity.ProductName)) //Если продукт изменен создаем новый список результатов исследований
            {
                await _researchResultsRepository.CreateResearchResultsAsync(partyDto.Id, partyDto.ProductId);
            }

            return partyDto.Id;
        }

        public async Task<MemoryStream> CreationOfAQualityAndSafetyCertificateAsync(Guid partyId)
        {
            var party = await _partyRepository.GetPartiesAsync(partyId);
            var product = await _productRepository.GetProductByNameAsync(party.ProductName);

            var memoryStream = new MemoryStream(); // Создаем поток без использования using

            var fontSize = 12;
            var fontName = "Times New Roman";

            using (var document = DocX.Create(memoryStream))
            {
                document.InsertParagraph("ОАО «Гомельский винодельческий завод»")
                    .FontSize(fontSize)
                    .Font(fontName)
                    .Alignment = Alignment.left;

                document.InsertParagraph("Производственная лаборатория")
                    .FontSize(fontSize)
                    .Font(fontName)
                    .Alignment = Alignment.left;

                document.InsertParagraph("");

                document.InsertParagraph($"УДОСТОВЕРЕНИЕ КАЧЕСТВА И БЕЗОПАСНОСТИ {party.BatchNumber}")
                    .FontSize(fontSize)
                    .Font(fontName)
                    .Bold()
                    .Alignment = Alignment.center;

                document.InsertParagraph("");


                document.InsertParagraph($"Отправитель: ")
                    .FontSize(fontSize)
                    .Font(fontName)

                    .Append(party.SupplierName)
                    .FontSize(fontSize)
                    .Font(fontName)
                    .UnderlineStyle(UnderlineStyle.singleLine)
                    .Alignment = Alignment.left;

                document.InsertParagraph($"Накладная: ")
                    .FontSize(fontSize)
                    .Font(fontName)

                    .Append($"№ {party.TTN}")
                    .UnderlineStyle(UnderlineStyle.singleLine)
                    .FontSize(fontSize)
                    .Font(fontName)
                    .Alignment = Alignment.left;

                document.InsertParagraph($"Получатель: ")
                    .FontSize(fontSize)
                    .Font(fontName)

                    .Append("ОАО «Гомельский винодельческий завод»")
                    .UnderlineStyle(UnderlineStyle.singleLine)
                    .FontSize(fontSize)
                    .Font(fontName)

                    .Append($"   Количество, {product.UnitsOfMeasurement} ")
                    .FontSize(fontSize)
                    .Font(fontName)

                    .Append($":{party.BatchSize}")
                    .FontSize(fontSize)
                    .Font(fontName)
                    .UnderlineStyle(UnderlineStyle.singleLine)
                    .Alignment = Alignment.left;

                document.InsertParagraph("");

                // Добавление таблицы
                // Определим количество строк и столбцов
                int rowCount = 3;
                int columnCount = 4 + party.ResearchResult.Count; 
                var table = document.InsertTable(rowCount, columnCount);

                // Задаем заголовки для таблицы
                table.Rows[0].Cells[0].Paragraphs[0].Append("№\nп/п")
                    .Bold()
                    .FontSize(fontSize)
                    .Font(fontName)
                    .Alignment = Alignment.center;
                // Изменение нижней границы
                table.Rows[0].Cells[0].SetBorder(TableCellBorderType.Bottom, new Border(BorderStyle.Tcbs_outset, 0, 0, Color.Transparent));

                table.Rows[0].Cells[1].Paragraphs[0].Append("Дата поступления")
                    .Bold()
                    .FontSize(fontSize)
                    .Font(fontName)
                    .Alignment = Alignment.center;
                // Изменение нижней границы
                table.Rows[0].Cells[1].SetBorder(TableCellBorderType.Bottom, new Border(BorderStyle.Tcbs_outset, 0, 0, Color.Transparent));


                table.Rows[0].Cells[2].Paragraphs[0].Append("Наименование сырья")
                    .Bold()
                    .FontSize(fontSize)
                    .Font(fontName)
                    .Alignment = Alignment.center;
                // Изменение нижней границы
                table.Rows[0].Cells[2].SetBorder(TableCellBorderType.Bottom, new Border(BorderStyle.Tcbs_outset, 0, 0, Color.Transparent));

                table.Rows[0].Cells[3].Paragraphs[0].Append("Органолептические показатели")
                    .Bold()
                    .FontSize(fontSize)
                    .Font(fontName)
                    .Alignment = Alignment.center;
                table.Rows[0].MergeCells(3, party.ResearchResult.Count + 2);

                for (int i = 0; i < party.ResearchResult.Count; i++)
                {
                    table.Rows[1].Cells[i + 3].Paragraphs[0].Append(party.ResearchResult[i].Research.ResearchName)
                        .Bold()
                        .FontSize(fontSize)
                        .Font(fontName)
                        .Alignment = Alignment.center;//Для результатов исследования
                }

                table.Rows[0].Cells[party.ResearchResult.Count + 2].Paragraphs[0].Append("Соответствие ТНПА")
                    .Bold()
                    .FontSize(fontSize)
                    .Font(fontName)
                    .Alignment = Alignment.center;

                // Изменение нижней границы
                table.Rows[0].Cells[party.ResearchResult.Count + 2].SetBorder(TableCellBorderType.Bottom, new Border(BorderStyle.Tcbs_outset, 0, 0, Color.Transparent));

                document.InsertParagraph("");

                // Заполнение таблицы данными
                table.Rows[2].Cells[0].Paragraphs[0].Append(party.BatchNumber.ToString())
                    .FontSize(fontSize)
                    .Font(fontName)
                    .Alignment = Alignment.center;

                table.Rows[2].Cells[1].Paragraphs[0].Append(party.DateOfReceipt)
                    .FontSize(fontSize)
                    .Font(fontName)
                    .Alignment = Alignment.center;

                table.Rows[2].Cells[2].Paragraphs[0].Append(party.ProductName)
                    .FontSize(fontSize)
                    .Font(fontName)
                    .Alignment = Alignment.center;

                for (int i = 0; i < party.ResearchResult.Count; i++)
                {
                    table.Rows[2].Cells[i + 3].Paragraphs[0].Append(party.ResearchResult[i].Result)
                        .FontSize(fontSize)
                        .Font(fontName)
                        .Alignment = Alignment.center;//Результаты исследования
                }

                table.Rows[2].Cells[party.ResearchResult.Count + 3].Paragraphs[0].Append("Соответствует требованиям ГОСТ 33222-2015")
                    .FontSize(fontSize)
                    .Font(fontName)
                    .Alignment = Alignment.center;

                document.InsertParagraph("");

                document.Save();
            }

            memoryStream.Position = 0;
            return memoryStream; 
        }
    }
}
