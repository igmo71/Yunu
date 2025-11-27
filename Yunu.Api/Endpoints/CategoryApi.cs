using Microsoft.AspNetCore.Mvc;
using Scalar.AspNetCore;
using Yunu.Api.Application;
using Yunu.Api.Common;
using Yunu.Api.Domain;

namespace Yunu.Api.Endpoints
{
    public static class CategoryApi
    {
        public static IEndpointRouteBuilder MapCategoryApi(this IEndpointRouteBuilder builder)
        {
            var api = builder.MapGroup(AppRouting.Prefix);

            api.MapGet(AppRouting.CategoryTreeUri, LoadCategoryTree)
                .WithName(nameof(LoadCategoryTree))
                .WithDescription("Load Category Tree")
                .WithTags(nameof(Category));
            api.MapDelete(AppRouting.CategoryTreeUri, ClearCategoryTree)
                .WithName(nameof(ClearCategoryTree))
                .WithDescription("Clear Category Tree")
                .WithTags(nameof(Category));

            return builder;
        }

        private static async Task<string> LoadCategoryTree([FromServices] ICategoryService yunuService)
        {
            var result = await yunuService.LoadCategoryTreeAsync();

            return $"Loaded: {result}";
        }

        private static async Task<string> ClearCategoryTree([FromServices] ICategoryService service)
        {
            var result = await service.ClearCategoryTreeAsync();

            return $"Deleted: {result}";
        }
    }
}
