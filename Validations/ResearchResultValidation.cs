using FluentValidation;
using Gvz.Laboratory.ResearchService.Models;

namespace Gvz.Laboratory.ResearchService.Validations
{
    public class ResearchResultValidation : AbstractValidator<ResearchResultModel>
    {
        public ResearchResultValidation()
        {
            RuleFor(x => x.Research)
                .NotEmpty().WithMessage("Результат исследования не может быть пустым.");
        }
    }
}
