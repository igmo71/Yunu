using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using Yunu.Api.Common;
using Yunu.Api.Domain;
using Yunu.Api.Infrastructure.Data;

namespace Yunu.Api.Application
{
    [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    public record ProductListParameters(int page, int perPage, int? scopeId);

    public interface IProductService
    {
        Task<int> LoadProductListAsync(ProductListParameters parameters);
        Task<int> ClearProductListAsync();        
    }

    public class ProductService(IYunuClient yunuClient, AppDbContext dbContext, IOptions<YunuConfig> options, ILogger<ProductService> logger) : IProductService
    {
        private readonly IYunuClient _yunuClient = yunuClient;
        private readonly AppDbContext _dbContext = dbContext;
        private readonly YunuConfig _yunuConfig = options.Value;
        private readonly ILogger<ProductService> _logger = logger;        

        public async Task<int> LoadProductListAsync(ProductListParameters parameters)
        {  // TODO: Доработать цикл постраничной загрузки
            var source = nameof(LoadProductListAsync);

            int scopeId = parameters.scopeId is not null ? (int)parameters.scopeId : _yunuConfig.ScopeId;

            var queryParams = new Dictionary<string, string?>
            {
                ["page"] = parameters.page.ToString(),
                ["perPage"] = parameters.perPage.ToString(),
                ["scopeId"] = parameters.scopeId.ToString(),
            };
            var uri = QueryHelpers.AddQueryString($"{AppRouting.Prefix}{AppRouting.ProductListUri}", queryParams);

            var productList = await _yunuClient.GetAsync<ProductList>(uri);

            if (productList is null || productList.list is null || productList.list.Count == 0)
            {
                _logger.LogError("{Source} Loading Product List Failed", source);
                return 0;
            }

            _ = await ClearProductListAsync();

            foreach (var product in productList.list)
            {
                product.fbo_stocks?.ProductId = product.id;

                if (product.fbo_stocks?.by_delivery_type is not null)
                    foreach (var by_delivery_type in product.fbo_stocks.by_delivery_type)
                        by_delivery_type.ProductId = product.id;
            }

            await _dbContext.Product.AddRangeAsync(productList.list);

            var result = await _dbContext.SaveChangesAsync();

            return result;
        }

        public async Task<int> ClearProductListAsync()
        {
            var result = await _dbContext.Product.ExecuteDeleteAsync();

            return result;
        }
    }
}
