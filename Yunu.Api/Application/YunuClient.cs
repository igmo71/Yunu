using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Text.Json;

namespace Yunu.Api.Application
{
    public interface IYunuClient
    {
        Task<TResult?> GetAsync<TResult>(string uri);
    }

    public class YunuClient : IYunuClient
    {
        private readonly HttpClient _httpClient;
        private readonly YunuConfig _yunuConfig;
        private readonly ILogger<YunuClient> _logger;

        public YunuClient(HttpClient httpClient, IOptions<YunuConfig> options, ILogger<YunuClient> logger)
        {
            _logger = logger;
            _yunuConfig = options.Value;

            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(_yunuConfig.BaseAddress ?? throw new InvalidOperationException("Yunu Base Address not found"));
            _httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
        }

        public async Task<TResult?> GetAsync<TResult>(string uri)
        {
            var source = nameof(GetAsync);
            try
            {
                var response = await _httpClient.GetAsync(uri);

                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("{Source} {ResponseContent}", source, responseContent);
                    return default;
                }

                var result = JsonSerializer.Deserialize<TResult>(responseContent);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Source}", source);
                throw;
            }
        }
    }
}
