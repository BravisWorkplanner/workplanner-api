using Domain.Enums;
using System;
using System.Text.Json.Serialization;
using FluentValidation;

namespace API.V1.Features.Orders.Request
{
    public class OrderUpdateRequest
    {
        [JsonPropertyName("OrderId")]
        public int OrderId { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("startDate")]
        public DateTime? StartDate { get; set; }

        [JsonPropertyName("endDate")]
        public DateTime? EndDate { get; set; }

        [JsonPropertyName("invoiceDate")]
        public DateTime? InvoiceDate { get; set; }

        [JsonPropertyName("orderStatus")]
        public OrderStatus OrderStatus { get; set; }
    }

    public class OrderUpdateValidator : AbstractValidator<OrderUpdateRequest>
    {
        public OrderUpdateValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty().WithMessage("{PropertyName} can not be default value. Please provide correct ID");
        }
    }
}