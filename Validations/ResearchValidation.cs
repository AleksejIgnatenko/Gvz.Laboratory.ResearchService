using FluentValidation;
using Gvz.Laboratory.ResearchService.Models;

namespace Gvz.Laboratory.ResearchService.Validations
{
    public class ResearchValidation : AbstractValidator<ResearchModel>
    {
        public ResearchValidation()
        {
            RuleFor(x => x.ResearchName)
                .NotEmpty().WithMessage("Название исследования не может быть пустым.")
                .MaximumLength(128).WithMessage("Название исследования не должно превышать 128 символов.");
        }
    }
}
