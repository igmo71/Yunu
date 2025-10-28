using Yunu.Api.Application.YunuAuth;

namespace Yunu.Api.Application
{
    public class YunuConfig
    {
        public const string Section = "Yunu";

        public string? BaseAddress { get; set; }
        public int ScopeId { get; set; }
        public LoginRequest? AuthParams { get; set; }

        public string? AccountBaseAddress { get; set; }
    }
}
