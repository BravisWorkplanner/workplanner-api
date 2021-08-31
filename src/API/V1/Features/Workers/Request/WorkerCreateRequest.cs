using System.Text.Json.Serialization;
using FluentValidation;

namespace API.V1.Features.Workers.Request
{
    public class WorkerCreateRequest
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("phoneNumber")]
        public string PhoneNumber { get; set; }
    }

    public class WorkerCreateValidator : AbstractValidator<WorkerCreateRequest>
    {
        public WorkerCreateValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("{PropertyName} can not be null");
            RuleFor(x => x.Email).NotEmpty().WithMessage("{PropertyName} can not be null");
        }
    }
}