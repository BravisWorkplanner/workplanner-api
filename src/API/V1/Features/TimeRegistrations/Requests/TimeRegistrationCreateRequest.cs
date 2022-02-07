using System;
using System.Text.Json.Serialization;

namespace API.V1.Features.TimeRegistrations.Requests;

public class TimeRegistrationCreateRequest
{
    [JsonPropertyName("OrderId")]
    public int OrderId { get; set; }

    [JsonPropertyName("WorkerId")]
    public int WorkerId { get; set; }

    [JsonPropertyName("hours")]
    public double Hours { get; set; }

    [JsonPropertyName("week")]
    public string Week { get; set; }

    [JsonPropertyName("day")]
    public DateTime Day { get; set; }
}