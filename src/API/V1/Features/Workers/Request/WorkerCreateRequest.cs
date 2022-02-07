using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using FluentValidation;

namespace API.V1.Features.Workers.Request
{
    public class WorkerCreateRequest
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("company")]
        public string Company { get; set; }

        [JsonPropertyName("phoneNumber")]
        public string PhoneNumber { get; set; }
    }

    public class WorkerCreateValidator : AbstractValidator<WorkerCreateRequest>
    {
        public WorkerCreateValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("{PropertyName} can not be null");
            RuleFor(x => x.Company).NotEmpty().WithMessage("{PropertyName} can not be null");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("{PropertyName} can not be null");
            RuleFor(x => x.PhoneNumber).
                Matches("^(([+]46)\\s*(7)|07)[02369]\\s*(\\d{4})\\s*(\\d{3})$", RegexOptions.Compiled).
                WithMessage("{PropertyName} must be in correct, swedish format.");
        }
    }
}