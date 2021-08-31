using System.Text.Json.Serialization;

namespace Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OrderStatus
    {
        NotStarted = 0,
        OnHold = 1,
        OnGoing = 2,
        Finished = 3,
    }
}