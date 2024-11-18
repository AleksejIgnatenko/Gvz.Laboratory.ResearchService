namespace Gvz.Laboratory.ResearchService.Entities
{
    public class PartyEntity
    {
        public Guid Id { get; set; }
        public int BatchNumber { get; set; }
        public string DateOfReceipt { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string SupplierName { get; set; } = string.Empty;
        public string ManufacturerName { get; set; } = string.Empty;
        public double BatchSize { get; set; }
        public double SampleSize { get; set; }
        public int TTN { get; set; }
        public string DocumentOnQualityAndSafety { get; set; } = string.Empty;
        public string TestReport { get; set; } = string.Empty;
        public string DateOfManufacture { get; set; } = string.Empty;
        public string ExpirationDate { get; set; } = string.Empty;
        public string Packaging { get; set; } = string.Empty;
        public string Marking { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public List<ResearchResultEntity> ResearchResults { get; set; } = new List<ResearchResultEntity>();
    }
}
