using System.Text.Json;

namespace eathappy.order.api.Extensions
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static class DefaultJsonSerializerOptions
    {
        public static JsonSerializerOptions Options => new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            IgnoreNullValues = true
        };
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
