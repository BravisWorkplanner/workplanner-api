using System.Text.Json.Serialization;

namespace API.V1.Features.Orders.Response
{
    public class ExpenseResult
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("product")]
        public string Product { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("price")]
        public double Price { get; set; }

        [JsonPropertyName("worker")]
        public string Worker { get; set; }
    }
}