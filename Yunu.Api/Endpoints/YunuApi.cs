using Microsoft.AspNetCore.Mvc;
using Yunu.Api.Application;
using static Yunu.Api.Application.IYunuService;

namespace Yunu.Api.Endpoints
{
    public static class YunuApi
    {
        public static IEndpointRouteBuilder MapYunuApi(this IEndpointRouteBuilder builder)
        {
            var api = builder.MapGroup(YunuClient.Prefix);

            api.MapGet(YunuClient.CategoryTreeUri, GetCategoryTree);
            api.MapGet(YunuClient.ProductListUri, GetProductList);

            return builder;
        }

        private static async Task<string> GetCategoryTree([FromServices] IYunuService yunuService)
        {
            var result = await yunuService.LoadCategoryTreeAsync();

            return $"Loaded: {result}";
        }

        private static async Task<string?> GetProductList(
            [FromServices] IYunuService yunuService,
            [FromQuery] int page = 0,
            [FromQuery] int perPage = 500,
            [FromQuery] int? scopeId = null)
        {
            var result = await yunuService.LoadProductListAsync(new ProductListParameters(page, perPage, scopeId));

            return $"Loaded: {result}";
        }
    }
}
