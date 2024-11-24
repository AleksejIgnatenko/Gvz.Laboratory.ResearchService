

using FluentValidation.Results;
using Gvz.Laboratory.ResearchService.Validations;

namespace Gvz.Laboratory.ResearchService.Models
{
    public class ResearchResultModel
    {
        public Guid Id { get; }
        public ResearchModel Research { get; set; } = new ResearchModel();
        public PartyModel Party { get; } = new PartyModel();
        public string Result { get; } = string.Empty;

        public ResearchResultModel(Guid id, string result)
        {
            Id = id;
            Result = result;
        }

        public ResearchResultModel(Guid id, ResearchModel research, string result)
        {
            Id = id;
            Research = research;
            Result = result;
        }

        public ResearchResultModel(Guid id, ResearchModel research, PartyModel party, string result)
        {
            Id = id;
            Research = research;
            Party = party;
            Result = result;
        }

        public static (Dictionary<string, string> errors, ResearchResultModel researchResult) Create(Guid id, string result, bool useValidation = true)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();

            ResearchResultModel researchResult = new ResearchResultModel(id, result);
            if (!useValidation) { return (errors, researchResult); }

            ResearchResultValidation researchResultValidation = new ResearchResultValidation();
            ValidationResult validationResult = researchResultValidation.Validate(researchResult);
            if (!validationResult.IsValid)
            {
                foreach (var failure in validationResult.Errors)
                {
                    errors[failure.PropertyName] = failure.ErrorMessage;
                }
            }

            return (errors, researchResult);
        }

        public static (Dictionary<string, string> errors, ResearchResultModel researchResult) Create(Guid id, ResearchModel research, string result, bool useValidation = true)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();

            ResearchResultModel researchResult = new ResearchResultModel(id, research, result);
            if (!useValidation) { return (errors, researchResult); }

            ResearchResultValidation researchResultValidation = new ResearchResultValidation();
            ValidationResult validationResult = researchResultValidation.Validate(researchResult);
            if (!validationResult.IsValid)
            {
                foreach (var failure in validationResult.Errors)
                {
                    errors[failure.PropertyName] = failure.ErrorMessage;
                }
            }

            return (errors, researchResult);
        }

        public static (Dictionary<string, string> errors, ResearchResultModel researchResult) Create(Guid id, ResearchModel research,  PartyModel party, string result, bool useValidation = true)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();

            ResearchResultModel researchResult = new ResearchResultModel(id, research, party, result);
            if (!useValidation) { return (errors, researchResult); }

            ResearchResultValidation researchResultValidation = new ResearchResultValidation();
            ValidationResult validationResult = researchResultValidation.Validate(researchResult);
            if (!validationResult.IsValid)
            {
                foreach (var failure in validationResult.Errors)
                {
                    errors[failure.PropertyName] = failure.ErrorMessage;
                }
            }

            return (errors, researchResult);
        }
    }
}
