using Microsoft.Extensions.Options;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Yunu.Api.Common;

namespace Yunu.Api.Application.YunuAuth
{
    public interface IYunuAuthService
    {
        Task<bool> LoginAsync(LoginRequest? authParams = null, CancellationToken cancellationToken = default);
        Task<bool> RefreshTockenAsync(CancellationToken cancellationToken = default);

        Task<string> GetTokenAsync();
    }

    public class AuthService : IYunuAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly YunuConfig _yunuConfig;
        private readonly ILogger<AuthService> _logger;

        private static AuthState? _authState = default;

        public const string Prefix = "api";
        public const string LoginUri = "/v1.0/login";
        public const string RefreshTokenUriUri = "/v1.0/refreshToken";

        public AuthService(IOptions<YunuConfig> options, HttpClient httpClient, ILogger<AuthService> logger)
        {
            _logger = logger;
            _yunuConfig = options.Value;

            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(_yunuConfig.BaseAddress ?? throw new InvalidOperationException("Yunu Base Address not found"));
        }

        public async Task<bool> LoginAsync(LoginRequest? loginRequest = null, CancellationToken cancellationToken = default)
        {
            var source = nameof(LoginAsync);
            try
            {
                var authRequest = loginRequest is not null
                ? new LoginRequest(loginRequest.Login, loginRequest.Password)
                : _yunuConfig.AuthParams ?? throw new InvalidOperationException("Yunu Auth Params not found");

                var requestBody = JsonSerializer.Serialize(authRequest, SerializationOptions.Auth);

                var request = new HttpRequestMessage(HttpMethod.Post, $"{Prefix}{LoginUri}")
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, mediaType: MediaTypeNames.Application.Json)
                };
                request.Headers.Add("Origin", _yunuConfig.AccountBaseAddress);
                request.Headers.Add("Referer", _yunuConfig.AccountBaseAddress);

                var authResponse = await _httpClient.SendAsync(request, cancellationToken);

                var responseContent = await authResponse.Content.ReadAsStringAsync(cancellationToken);

                if (!authResponse.IsSuccessStatusCode)
                {
                    _logger.LogError("{Source} {ResponseContent}", source, responseContent);
                    return false;
                }
                _authState = JsonSerializer.Deserialize<AuthState>(responseContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Source}", source);
                return false;
            }

            return true;
        }

        public async Task<bool> RefreshTockenAsync(CancellationToken cancellationToken = default)
        {
            var source = nameof(RefreshTockenAsync);
            try
            {
                if (_authState is null)
                {
                    _logger.LogError("{Source} AuthState Is Null", source);
                    return false;
                }

                var authRequest = new RefreshTokenRequest()
                {
                    RefreshToken = _authState.Result.RefreshToken,
                    ScopeId = _yunuConfig.ScopeId
                };

                var requestBody = JsonSerializer.Serialize(authRequest, SerializationOptions.Auth);

                var request = new HttpRequestMessage(HttpMethod.Post, $"{Prefix}{RefreshTokenUriUri}")
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, mediaType: MediaTypeNames.Application.Json)
                };
                request.Headers.Add("Origin", _yunuConfig.AccountBaseAddress);
                request.Headers.Add("Referer", _yunuConfig.AccountBaseAddress);

                var authResponse = await _httpClient.SendAsync(request, cancellationToken);

                var responseContent = await authResponse.Content.ReadAsStringAsync(cancellationToken);

                if (!authResponse.IsSuccessStatusCode)
                {
                    _logger.LogError("{Source} {ResponseContent}", source, responseContent);
                    return false;
                }

                var refreshTokenResponse = JsonSerializer.Deserialize<RefreshTokenResponse>(responseContent);

                if (refreshTokenResponse is null)
                {
                    _logger.LogError("{Source} RefreshTokenResponse Deserialize Failed", source);
                    return false;
                }
                _authState.Result.Token = refreshTokenResponse.Token;
                _authState.Result.RefreshToken = refreshTokenResponse.RefreshToken;
                _authState.Result.LifeTime = refreshTokenResponse.LifeTime;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Source}", nameof(RefreshTockenAsync));
                return false;
            }

            return true;
        }

        public async Task<string> GetTokenAsync()
        {
            var source = nameof(GetTokenAsync);

            if (_authState is null)
                if (!await LoginAsync())
                    _logger.LogError("{Source} Login Failed", source);

            if (_authState is not null && DateTime.Now > _authState.Result.LifeTime)
                if (!await RefreshTockenAsync())
                    _logger.LogError("{Source} Refresh Tocken Failed", source);

            if (_authState is null)
            {
                _logger.LogError("{Source} Login and Refresh Tocken Failed", source);
                throw new InvalidOperationException("Yunu Login and Refresh Tocken Failed");
            }
            return _authState.Result.Token;
        }
    }
}
