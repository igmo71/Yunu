using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using Yunu.Api.Infrastructure.Data;
using static Yunu.Api.Application.IYunuService;

namespace Yunu.Api.Application
{
    public interface IYunuService
    {
        Task<int> LoadCategoryTreeAsync();

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        record ProductListParameters(int page, int perPage, int? scopeId);
        Task<int> LoadProductListAsync(ProductListParameters parameters);
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
    }
}
