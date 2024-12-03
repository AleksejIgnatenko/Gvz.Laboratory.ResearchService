using FluentValidation;
using Gvz.Laboratory.ResearchService.Models;

namespace Gvz.Laboratory.ResearchService.Validations
{
    public class ResearchResultValidation : AbstractValidator<ResearchResultModel>
    {
        public ResearchResultValidation()
        {
            RuleFor(x => x.Result)
                .NotEmpty().WithMessage("Результат исследования не может быть пустым.")
                .MaximumLength(128).WithMessage("Результат исследования не должен превышать 128 символов.");
        }
    }
}
