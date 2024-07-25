using System.Text.Json.Serialization;

namespace Library.Domain;

public class Response
{
    [JsonPropertyName("status")] public string StatusText { get; set; } = null!;

    public string Data { get; set; } = default!;
}