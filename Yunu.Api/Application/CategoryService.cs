using Microsoft.EntityFrameworkCore;
using Yunu.Api.Infrastructure.Data;

namespace Yunu.Api.Application
{
    public interface ICategoryService
    {
        Task<int> LoadCategoryTreeAsync();
        Task<int> ClearCategoryTreeAsync();
    }

    public class CategoryService(IYunuClient yunuClient, AppDbContext dbContext, ILogger<CategoryService> logger) : ICategoryService
    {
        private readonly IYunuClient _yunuClient = yunuClient;
        private readonly AppDbContext _dbContext = dbContext;
        private readonly ILogger<CategoryService> _logger = logger;        

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
        public async Task<int> ClearCategoryTreeAsync()
        {
            var result = await _dbContext.Category.ExecuteDeleteAsync();

            return result;
        }
    }
}
