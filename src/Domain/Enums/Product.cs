using System.Text.Json.Serialization;

namespace Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Product
    {
        Material = 0,
        Tools = 1,
        MachineHiring = 2,
        WorkerClothes = 3,
    }
}