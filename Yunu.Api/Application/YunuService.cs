using Microsoft.EntityFrameworkCore;
using Yunu.Api.Infrastructure.Data;

namespace Yunu.Api.Application
{
    public interface IYunuService
    {
        Task<int> LoadCategoryTreeAsync();
        Task<int> LoadProductListAsync(int page, int perPage, int? scopeId);
    }

    public class YunuService(IYunuClient yunuClient, AppDbContext dbContext, ILogger<YunuService> logger) : IYunuService
    {
        private readonly IYunuClient _yunuClient = yunuClient;
        private readonly AppDbContext _dbContext = dbContext;
        private readonly ILogger<YunuService> _logger = logger;

        public async Task<int> LoadCategoryTreeAsync()
        {
            var source = nameof(LoadCategoryTreeAsync);

            var categoryTree = await _yunuClient.GetCategoryTreeAsync();

            if (categoryTree is null || categoryTree.tree is null || categoryTree.tree.Count == 0)
            {
                _logger.LogError("{Source} Loading Category Tree Failed", source);
                return 0;
            }

            // TODO: Category CreateOrUpdate

            await _dbContext.Category.AddRangeAsync(categoryTree.tree);

            var result = await _dbContext.SaveChangesAsync();

            return result;

        }

        public async Task<int> LoadProductListAsync(int page, int perPage, int? scopeId)
        {
            var source = nameof(LoadProductListAsync);

            var productList = await _yunuClient.GetProductListAsync(page, perPage, scopeId);

            if (productList is null || productList.list is null || productList.list.Count == 0)
            {
                _logger.LogError("{Source} Loading Product List Failed", source);
                return 0;
            }

            // TODO: Product CreateOrUpdate

            foreach (var product in productList.list)
            {
                product.fbo_stock?.ProductId = product.id;

                if (product.fbo_stock?.by_delivery_type is not null)
                    foreach (var by_delivery_type in product.fbo_stock.by_delivery_type)
                        by_delivery_type.ProductId = product.id;

                product.fbo_stocks?.ProductId = product.id;

                if (product.fbo_stocks?.by_delivery_type is not null)
                    foreach (var by_delivery_type in product.fbo_stocks.by_delivery_type)
                        by_delivery_type.ProductId = product.id;
            }

            await _dbContext.Product.AddRangeAsync(productList.list);

            var result = await _dbContext.SaveChangesAsync();

            return result;
        }
    }
}
