using Domain.Enums;
using System;
using System.Text.Json.Serialization;

namespace API.V1.Features.Orders.Response
{
    public class OrderListResult
    {
        [JsonPropertyName("orderId")]
        public int OrderId { get; set; }

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
}