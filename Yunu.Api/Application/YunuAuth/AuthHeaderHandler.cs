using System.Net.Http.Headers;

namespace Yunu.Api.Application.YunuAuth;

public class AuthHeaderHandler(IYunuAuthService authService) : DelegatingHandler
{
    private readonly IYunuAuthService _authService = authService;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _authService.GetTokenAsync();

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return await base.SendAsync(request, cancellationToken);
    }
}
