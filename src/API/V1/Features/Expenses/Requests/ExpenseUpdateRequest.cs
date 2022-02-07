using System.Text.Json.Serialization;
using FluentValidation;

namespace API.V1.Features.Expenses.Requests
{
    public class ExpenseUpdateRequest
    {
        [JsonPropertyName("expenseId")]
        public int ExpenseId { get; set; }

        [JsonPropertyName("price")]
        public double Price { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("productId")]
        public int ProductId { get; set; }

        [JsonPropertyName("invoiceId")]
        public string InvoiceId { get; set; }
    }

    public class OrderExpenseUpdateValidator : AbstractValidator<ExpenseUpdateRequest>
    {
        public OrderExpenseUpdateValidator()
        {
            RuleFor(x => x.ExpenseId).NotEmpty().WithMessage("{PropertyName} can not be null");
            RuleFor(x => x.Price).NotEmpty().WithMessage("{PropertyName} can not be null or default");
            RuleFor(x => x.ProductId).NotEmpty().WithMessage("{PropertyName} can not be null or default");
            RuleFor(x => x.Description).NotEmpty().WithMessage("{PropertyName} can not be null or default");
        }
    }
}