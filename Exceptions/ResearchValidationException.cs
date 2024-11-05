namespace Gvz.Laboratory.ResearchService.Exceptions
{
    public class ResearchValidationException : Exception
    {
        public Dictionary<string, string> Errors { get; set; }

        public ResearchValidationException(Dictionary<string, string> errors)
        {
            Errors = errors;
        }
    }
}
