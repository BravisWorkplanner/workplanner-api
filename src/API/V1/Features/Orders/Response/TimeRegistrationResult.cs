using System;
using System.Text.Json.Serialization;

namespace API.V1.Features.Orders.Response
{
    public class TimeRegistrationResult
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("Day")]
        public DateTime Day { get; set; }

        [JsonPropertyName("week")]
        public string Week { get; set; }

        [JsonPropertyName("worker")]
        public string Worker { get; set; }

        [JsonPropertyName("hours")]
        public double Hours { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }
    }
}