using System.Text.Json.Serialization;
using FluentValidation;

namespace API.V1.Features.Expenses.Requests
{
    public class OrderExpenseCreateRequest
    {
        [JsonPropertyName("orderId")]
        public int OrderId { get; set; }

        [JsonPropertyName("workerId")]
        public int WorkerId { get; set; }

        [JsonPropertyName("price")]
        public double Price { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    public class OrderExpenseCreateValidator : AbstractValidator<OrderExpenseCreateRequest>
    {
        public OrderExpenseCreateValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty().WithMessage("{PropertyName} can not be null");
            RuleFor(x => x.WorkerId).NotEmpty().WithMessage("{PropertyName} can not be null");
            RuleFor(x => x.Price).NotEmpty().WithMessage("{PropertyName} can not be null");
        }
    }
}