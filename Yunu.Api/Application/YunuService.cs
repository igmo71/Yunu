using Microsoft.EntityFrameworkCore;
using Yunu.Api.Infrastructure.Data;

namespace Yunu.Api.Application
{
    public interface IYunuService
    {
        Task<int> LoadProductListAsync(int page, int perPage, int? scopeId);
    }

    public class YunuService(IYunuClient yunuClient, AppDbContext dbContext, ILogger<YunuService> logger) : IYunuService
    {
        private readonly IYunuClient _yunuClient = yunuClient;
        private readonly AppDbContext _dbContext = dbContext;
        private readonly ILogger<YunuService> _logger = logger;

        public async Task<int> LoadProductListAsync(int page, int perPage, int? scopeId)
        {
            var source = nameof(LoadProductListAsync);

            var productList = await _yunuClient.GetProductListAsync(page, perPage, scopeId);

            if (productList is null || productList.list is null || productList.list.Count == 0)
            {
                _logger.LogError("{Source} Loading Product List Failed", source);
                return 0;
            }

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

            //using var transaction = await _dbContext.Database.BeginTransactionAsync();
            //await _dbContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Product ON;");
            await _dbContext.Product.AddRangeAsync(productList.list);
            var result = await _dbContext.SaveChangesAsync();
            //await _dbContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Product OFF;");
            //await transaction.CommitAsync();

            return result;
        }
    }
}
