using Microsoft.AspNetCore.Mvc;
using Yunu.Api.Application;
using Yunu.Api.Domain;

namespace Yunu.Api.Endpoints
{
    public static class YunuApi
    {
        public static IEndpointRouteBuilder MapYunuApi(this IEndpointRouteBuilder builder)
        {
            var api = builder.MapGroup(YunuClient.Prefix);

            api.MapGet(YunuClient.ProductListUri, GetProductList);

            return builder;
        }

        private static async Task<string?> GetProductList(
            [FromServices] IYunuService yunuService,
            [FromQuery] int page = 0,
            [FromQuery] int perPage = 500,
            [FromQuery] int? scopeId = null)
        {
            var result = await yunuService.LoadProductListAsync(page, perPage, scopeId);

            return $"Loaded: {result}";
        }
    }
}
