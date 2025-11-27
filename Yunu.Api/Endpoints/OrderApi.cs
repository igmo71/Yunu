using Microsoft.AspNetCore.Mvc;
using Yunu.Api.Application;
using Yunu.Api.Common;
using Yunu.Api.Domain;

namespace Yunu.Api.Endpoints;

public static class OrderApi
{
    public static IEndpointRouteBuilder MapOrderApi(this IEndpointRouteBuilder builder)
    {
        var api = builder.MapGroup(AppRouting.Prefix);

        api.MapGet(AppRouting.OrderListUri, LoadOrderList)
            .WithName(nameof(LoadOrderList))
            .WithDescription("Load Order List")
            .WithTags(nameof(Order));
        api.MapDelete(AppRouting.OrderListUri, ClearOrderList)
            .WithName(nameof(ClearOrderList))
            .WithDescription("Clear Order List")
            .WithTags(nameof(Order));

        return builder;
    }

    private static async Task<string> LoadOrderList([FromServices] IOrderService service)
    {
        var result = await service.LoadOrderListAsync();

        return $"Loaded: {result}";
    }

    private static async Task<string> ClearOrderList([FromServices] IOrderService service)
    {
        var result = await service.ClearOrderListAsync();

        return $"Deleted: {result}";
    }
}
