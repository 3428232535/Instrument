using System.Text.Json;

namespace Instrument;

public static class JsonOption
{
    public static JsonSerializerOptions Options = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };
}