

using FluentValidation.Results;
using Gvz.Laboratory.ResearchService.Validations;

namespace Gvz.Laboratory.ResearchService.Models
{
    public class ResearchModel
    {
        public Guid Id { get; }
        public string ResearchName { get; } = string.Empty;
        //public ProductModel Product { get; set; } = new ProductModel();
        //public List<ResearchResultModel> ResearchResults { get; set; } = new List<ResearchResultModel>();

        public ResearchModel(Guid id, string researchName)
        {
            Id = id;
            ResearchName = researchName;
        }

        public static (Dictionary<string, string> errors, ResearchModel research) Create(Guid id, string researchName, bool useValidation = true)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();

            ResearchModel research = new ResearchModel(id, researchName);
            if(!useValidation ) { return (errors, research); }

            ResearchValidation researchValidation = new ResearchValidation();
            ValidationResult validationResult = researchValidation.Validate(research);
            if (!validationResult.IsValid)
            {
                foreach (var failure in validationResult.Errors)
                {
                    errors[failure.PropertyName] = failure.ErrorMessage;
                }
            }

            return (errors, research);
        }
    }
}
