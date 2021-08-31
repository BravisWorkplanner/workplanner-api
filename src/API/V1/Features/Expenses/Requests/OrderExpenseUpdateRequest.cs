using System.Text.Json.Serialization;
using FluentValidation;

namespace API.V1.Features.Expenses.Requests
{
    public class OrderExpenseUpdateRequest
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("price")]
        public double Price { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    public class OrderExpenseUpdateValidator : AbstractValidator<OrderExpenseUpdateRequest>
    {
        public OrderExpenseUpdateValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} can not be null");
            RuleFor(x => x.Price).NotEmpty().WithMessage("{PropertyName} can not be default");
        }
    }
}