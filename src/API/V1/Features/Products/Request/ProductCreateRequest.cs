using System.Text.Json.Serialization;

namespace API.V1.Features.Products.Request;

public class ProductCreateRequest
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
}