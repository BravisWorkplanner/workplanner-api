using System.Text.Json.Serialization;
using Domain.Entities;
using Domain.Enums;

namespace API.V1.Features.Orders.Response
{
    public class ExpenseResult
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("product")]
        public Product Product { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("price")]
        public double Price { get; set; }

        [JsonPropertyName("worker")]
        public Worker Worker { get; set; }
    }
}