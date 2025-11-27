using Microsoft.AspNetCore.Mvc;
using Scalar.AspNetCore;
using Yunu.Api.Application;
using Yunu.Api.Common;
using Yunu.Api.Domain;

namespace Yunu.Api.Endpoints
{
    public static class ProductApi
    {
        public static IEndpointRouteBuilder MapProductApi(this IEndpointRouteBuilder builder)
        {
            var api = builder.MapGroup(AppRouting.Prefix);

            api.MapGet(AppRouting.ProductListUri, LoadProductList)
                .WithName(nameof(LoadProductList))
                .WithDescription("Load Product List")
                .WithTags(nameof(Product));
            api.MapDelete(AppRouting.ProductListUri, ClearProductList)
                .WithName(nameof(ClearProductList))
                .WithDescription("Clear Product List")
                .WithTags(nameof(Product));

            return builder;
        }
        
        private static async Task<string> LoadProductList([FromServices] IProductService service,
            [FromQuery] int page = 0,
            [FromQuery] int perPage = 500,
            [FromQuery] int? scopeId = null)
        {
            var result = await service.LoadProductListAsync(new ProductListParameters(page, perPage, scopeId));

            return $"Loaded: {result}";
        }

        private static async Task<string> ClearProductList([FromServices] IProductService service)
        {
            var result = await service.ClearProductListAsync();

            return $"Deleted: {result}";
        }
    }
}
