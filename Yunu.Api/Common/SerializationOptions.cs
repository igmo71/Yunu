using System.Text.Json;

namespace Yunu.Api.Common
{
    public static class SerializationOptions
    {
        public static readonly JsonSerializerOptions Auth = new()
        {
            WriteIndented = false,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }
}
