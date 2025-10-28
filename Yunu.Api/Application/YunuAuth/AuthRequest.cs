using System.Text.Json.Serialization;

namespace Yunu.Api.Application.YunuAuth;

public record LoginRequest(string Login, string Password);

public class RefreshTokenRequest
{
    [JsonPropertyName("refresh_token")]
    public required string RefreshToken { get; set; }

    [JsonPropertyName("scopeId")]
    public int ScopeId { get; set; }
}

