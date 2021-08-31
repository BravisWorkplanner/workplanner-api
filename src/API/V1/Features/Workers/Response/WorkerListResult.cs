using System.Text.Json.Serialization;

namespace API.V1.Features.Workers.Response
{
    public class WorkerListResult
    {
        [JsonPropertyName("workerId")]
        public int WorkerId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("phoneNumber")]
        public string PhoneNumber { get; set; }
    }
}