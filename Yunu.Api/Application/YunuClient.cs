using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Text.Json;
using Yunu.Api.Domain;

namespace Yunu.Api.Application
{
    public interface IYunuClient
    {
        Task<CategoryTree?> GetCategoryTreeAsync();
        Task<ProductList?> GetProductListAsync(int page, int perPage, int? scopeId);
    }

    public class YunuClient : IYunuClient
    {
        private readonly HttpClient _httpClient;
        private readonly YunuConfig _yunuConfig;
        private readonly ILogger<YunuClient> _logger;

        public const string Prefix = "api";
        public const string ProductListUri = "/v1.0/product/list";
        public const string CategoryTreeUri = "/v1.0/category/tree";

        public YunuClient(IOptions<YunuConfig> options, HttpClient httpClient, ILogger<YunuClient> logger)
        {
            _logger = logger;
            _yunuConfig = options.Value;

            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(_yunuConfig.BaseAddress ?? throw new InvalidOperationException("Yunu Base Address not found"));
            _httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
        }

        public async Task<CategoryTree?> GetCategoryTreeAsync()
        {
            var source = nameof(GetCategoryTreeAsync);
            try
            {
                var uri = $"{Prefix}{CategoryTreeUri}";

                var response = await _httpClient.GetAsync(uri);

                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    _logger.LogError("{Source} {ResponseContent}", source, responseContent);

                var result = JsonSerializer.Deserialize<CategoryTree>(responseContent);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Source}", source);
                throw;
            }
        }

        public async Task<ProductList?> GetProductListAsync(int page, int perPage, int? scopeId)
        {
            var source = nameof(GetProductListAsync);
            try
            {
                var queryParams = new Dictionary<string, string?>
                {
                    ["page"] = page.ToString(),
                    ["perPage"] = perPage.ToString(),
                    ["scopeId"] = scopeId is not null ? scopeId.ToString() : _yunuConfig.ScopeId.ToString(),
                };
                var uri = QueryHelpers.AddQueryString($"{Prefix}{ProductListUri}", queryParams);

                var response = await _httpClient.GetAsync(uri);

                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    _logger.LogError("{Source} {ResponseContent}", source, responseContent);

                var result = JsonSerializer.Deserialize<ProductList>(responseContent);

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
