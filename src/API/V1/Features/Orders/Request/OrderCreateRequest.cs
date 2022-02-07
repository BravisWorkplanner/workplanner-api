using System.Text.Json.Serialization;
using FluentValidation;

namespace API.V1.Features.Orders.Request
{
    public class OrderCreateRequest
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("customerName")]
        public string CustomerName { get; set; }

        [JsonPropertyName("customerPhoneNumber")]
        public string CustomerPhoneNumber { get; set; }

        public class OrderCreateRequestValidator : AbstractValidator<OrderCreateRequest>
        {
            public OrderCreateRequestValidator()
            {
                RuleFor(x => x.Description).NotEmpty().WithMessage("{PropertyName} can not be null");
                RuleFor(x => x.Address).NotEmpty().WithMessage("{PropertyName} can not be null");
                RuleFor(x => x.CustomerName).NotEmpty().WithMessage("{PropertyName} can not be null");
                RuleFor(x => x.CustomerPhoneNumber).NotEmpty().WithMessage("{PropertyName} can not be null");
            }
        }
    }
}