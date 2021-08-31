using Domain.Enums;
using System;
using System.Text.Json.Serialization;
using FluentValidation;

namespace API.V1.Features.Orders.Request
{
    public class OrderCreateRequest
    {
        [JsonPropertyName("objectNumber")]
        public string ObjectNumber { get; set; }

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

    public class OrderCreateValidator : AbstractValidator<OrderCreateRequest>
    {
        public OrderCreateValidator()
        {
            RuleFor(x => x.ObjectNumber).NotEmpty().WithMessage("{PropertyName} can not be null");
        }
    }
}