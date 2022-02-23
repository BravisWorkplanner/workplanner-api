using System.Text.Json.Serialization;

namespace API.V1.Features.Expenses.Responses
{
    public class ExpenseListResult
    {
        [JsonPropertyName("productType")]
        public string ProductType { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("price")]
        public double Price { get; set; }

        [JsonPropertyName("orderId")]
        public int OrderId { get; set; }

        [JsonPropertyName("objectNumber")]
        public string ObjectNumber { get; set; }

        [JsonPropertyName("workerId")]
        public int WorkerId { get; set; }

        [JsonPropertyName("workerName")]
        public string WorkerName { get; set; }
    }
}