using System.Text.Json.Serialization;

namespace API.V1.Base
{
    public abstract class PagedRequest
    {
        [JsonPropertyName("perPage")]
        public int PerPage { get; set; } = 20;

        [JsonPropertyName("page")]
        public int Page { get; set; } = 1;
    }
}