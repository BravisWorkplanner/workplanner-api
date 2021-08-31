using System;
using System.Text.Json.Serialization;
using Domain.Entities;

namespace API.V1.Features.Orders.Response
{
    public class TimeRegistrationResult
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("dateTime")]
        public DateTime DateTime { get; set; }

        [JsonPropertyName("week")]
        public string Week { get; set; }

        [JsonPropertyName("worker")]
        public Worker Worker { get; set; }

        [JsonPropertyName("hours")]
        public double Hours { get; set; }
    }
}