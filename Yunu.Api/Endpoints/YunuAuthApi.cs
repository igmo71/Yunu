using Microsoft.AspNetCore.Mvc;
using Yunu.Api.Application.YunuAuth;
using LoginRequest = Yunu.Api.Application.YunuAuth.LoginRequest;

namespace Yunu.Api.Endpoints
{
    public static class YunuAuthApi
    {

        public static IEndpointRouteBuilder MapYunuAuthApi(this IEndpointRouteBuilder builder)
        {
            var api = builder.MapGroup(AuthService.Prefix);

            api.MapGet(AuthService.LoginUri, Login);
            api.MapGet(AuthService.RefreshTokenUriUri, RefreshToken);

            return builder;
        }

        private static async Task<bool> Login(
            [FromBody] LoginRequest? authParams,
            [FromServices] IYunuAuthService authService)
        {
            var result = await authService.LoginAsync(authParams);

            return result;
        }

        private static async Task<bool> RefreshToken(
            [FromServices] IYunuAuthService authService)
        {
            var result = await authService.RefreshTockenAsync();

            return result;
        }

    }
}
