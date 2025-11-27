namespace Yunu.Api.Endpoints
{
    public static partial class DiExtensions
    {
        public static IEndpointRouteBuilder MapAppApi(this IEndpointRouteBuilder builder)
        {
            builder.MapCategoryApi();
            builder.MapProductApi();
            builder.MapOrderApi();

            return builder;
        }
    }
}
