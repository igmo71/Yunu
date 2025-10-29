using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
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

    public class ProductService(IYunuClient yunuClient, AppDbContext dbContext, ILogger<ProductService> logger) : IProductService
    {
        private readonly IYunuClient _yunuClient = yunuClient;
        private readonly AppDbContext _dbContext = dbContext;
        private readonly ILogger<ProductService> _logger = logger;        

        public async Task<int> LoadProductListAsync(ProductListParameters parameters) // TODO: ProductList Parameters
        {
            var source = nameof(LoadProductListAsync);

            var productList = await _yunuClient.GetProductListAsync(parameters.page, parameters.perPage, parameters.scopeId);

            if (productList is null || productList.list is null || productList.list.Count == 0)
            {
                _logger.LogError("{Source} Loading Product List Failed", source);
                return 0;
            }

            // TODO: Product CreateOrUpdate

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
