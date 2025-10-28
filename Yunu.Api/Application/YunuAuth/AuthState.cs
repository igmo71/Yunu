using System.Text.Json.Serialization;

namespace Yunu.Api.Application.YunuAuth;

public class AuthState
{
    [JsonPropertyName("status")]
    public required string Status { get; set; }


    [JsonPropertyName("result")]
    public required Result Result { get; set; }
}

public class Result
{
    [JsonPropertyName("token")]
    public required string Token { get; set; }


    [JsonPropertyName("lifeTime")]
    public DateTime LifeTime { get; set; }


    [JsonPropertyName("refresh_token")]
    public required string RefreshToken { get; set; }


    [JsonPropertyName("mp_auth_scopes")]
    public required MpAuthScopes[] MpAuthScopes { get; set; }


    [JsonPropertyName("active_warehouses")]
    public required ActiveWarehouses[] ActiveWarehouses { get; set; }
}

public class MpAuthScopes
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public required string Title { get; set; }
}

public class ActiveWarehouses
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }
}


public class RefreshTokenResponse
{
    [JsonPropertyName("token")]
    public required string Token { get; set; }

    [JsonPropertyName("lifeTime")]
    public DateTime LifeTime { get; set; }

    [JsonPropertyName("refresh_token")]
    public required string RefreshToken { get; set; }
}