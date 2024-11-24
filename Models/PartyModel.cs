namespace Gvz.Laboratory.ResearchService.Models
{
    public class PartyModel
    {
        public Guid Id { get; }
        public int BatchNumber { get; }
        public string DateOfReceipt { get; }
        public string ProductName { get; } = string.Empty;
        public string SupplierName { get; } = string.Empty;
        public string ManufacturerName { get; } = string.Empty;
        public double BatchSize { get; }
        public double SampleSize { get; }
        public int TTN { get; }
        public string DocumentOnQualityAndSafety { get; } = string.Empty;
        public string TestReport { get; } = string.Empty;
        public string DateOfManufacture { get; }
        public string ExpirationDate { get; }
        public string Packaging { get; } = string.Empty;
        public string Marking { get; } = string.Empty;
        public string Result { get; } = string.Empty;
        public string Surname { get; } = string.Empty;
        public string Note { get; } = string.Empty;
        public List<ResearchResultModel> ResearchResult { get; } = new List<ResearchResultModel>();

        public PartyModel()
        {
        }

        public PartyModel(Guid id, int batchNumber, string dateOfReceipt, string productName, string supplierName,
            string manufacturerName, double batchSize, double sampleSize, int ttn, string documentOnQualityAndSafety,
            string testReport, string dateOfManufacture, string expirationDate, string packaging, string marking,
            string result, string surname, string note)
        {
            Id = id;
            BatchNumber = batchNumber;
            DateOfReceipt = dateOfReceipt;
            ProductName = productName;
            SupplierName = supplierName;
            ManufacturerName = manufacturerName;
            BatchSize = batchSize;
            SampleSize = sampleSize;
            TTN = ttn;
            DocumentOnQualityAndSafety = documentOnQualityAndSafety;
            TestReport = testReport;
            DateOfManufacture = dateOfManufacture;
            ExpirationDate = expirationDate;
            Packaging = packaging;
            Marking = marking;
            Result = result;
            Note = note;
            Surname = surname;
        }

        public PartyModel(Guid id, int batchNumber, string dateOfReceipt, string productName, string supplierName,
            string manufacturerName, double batchSize, double sampleSize, int ttn, string documentOnQualityAndSafety,
            string testReport, string dateOfManufacture, string expirationDate, string packaging, string marking,
            string result, string surname, string note, List<ResearchResultModel> researchResult)
        {
            Id = id;
            BatchNumber = batchNumber;
            DateOfReceipt = dateOfReceipt;
            ProductName = productName;
            SupplierName = supplierName;
            ManufacturerName = manufacturerName;
            BatchSize = batchSize;
            SampleSize = sampleSize;
            TTN = ttn;
            DocumentOnQualityAndSafety = documentOnQualityAndSafety;
            TestReport = testReport;
            DateOfManufacture = dateOfManufacture;
            ExpirationDate = expirationDate;
            Packaging = packaging;
            Marking = marking;
            Surname = surname;
            Note = note;
            ResearchResult = researchResult;
        }

        public static PartyModel Create(Guid id, int batchNumber, string dateOfReceipt, string productName, string supplierName,
            string manufacturerName, double batchSize, double sampleSize, int ttn, string documentOnQualityAndSafety,
            string testReport, string dateOfManufacture, string expirationDate, string packaging, string marking,
            string result, string surname, string note)
        {
            return new PartyModel(id, batchNumber, dateOfReceipt, productName, supplierName, manufacturerName,
                batchSize, sampleSize, ttn, documentOnQualityAndSafety, testReport, dateOfManufacture, expirationDate,
                packaging, marking, result, surname, note);
        }

        public static PartyModel Create(Guid id, int batchNumber, string dateOfReceipt, string productName, string supplierName,
            string manufacturerName, double batchSize, double sampleSize, int ttn, string documentOnQualityAndSafety,
            string testReport, string dateOfManufacture, string expirationDate, string packaging, string marking,
            string result, string surname, string note, List<ResearchResultModel> researchResult)
        {
            return new PartyModel(id, batchNumber, dateOfReceipt, productName, supplierName, manufacturerName,
                batchSize, sampleSize, ttn, documentOnQualityAndSafety, testReport, dateOfManufacture, expirationDate,
                packaging, marking, result, surname, note, researchResult);
        }
    }
}
