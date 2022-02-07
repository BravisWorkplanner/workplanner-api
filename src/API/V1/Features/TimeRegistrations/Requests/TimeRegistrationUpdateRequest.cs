using System;
using System.Text.Json.Serialization;

namespace API.V1.Features.TimeRegistrations.Requests;

public class TimeRegistrationUpdateRequest
{
    [JsonPropertyName("timeRegistrationId")]
    public int TimeRegistrationId { get; set; }

    [JsonPropertyName("hours")]
    public double Hours { get; set; }

    [JsonPropertyName("week")]
    public string Week { get; set; }

    [JsonPropertyName("day")]
    public DateTime Day { get; set; }
}