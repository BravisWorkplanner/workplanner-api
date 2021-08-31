using System.Text.Json.Serialization;
using FluentValidation;

namespace API.V1.Features.Workers.Request
{
    public class WorkerUpdateRequest
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("phoneNumber")]
        public string PhoneNumber { get; set; }
    }

    public class WorkerUpdateValidator : AbstractValidator<WorkerUpdateRequest>
    {
        public WorkerUpdateValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} can not be default value. Please provide OrderId");
            RuleFor(x => x.Name).NotEmpty().WithMessage("{PropertyName} can not be null");
            RuleFor(x => x.Email).NotEmpty().WithMessage("{PropertyName} can not be null");
        }
    }
}