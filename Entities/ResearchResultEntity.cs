namespace Gvz.Laboratory.ResearchService.Entities
{
    public class ResearchResultEntity
    {
        public Guid Id { get; set; }
        public ResearchEntity Research {  get; set; } = new ResearchEntity();
        public string Result { get; set; } = string.Empty;
        public DateTime DateCreate { get; set; }
    }
}
