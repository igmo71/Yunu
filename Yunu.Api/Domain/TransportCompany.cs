using System.Diagnostics.CodeAnalysis;

namespace Yunu.Api.Domain
{
    [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    public class TransportCompany
    {
        public int id { get; set; }
        public string? type { get; set; }
        public string? name { get; set; }
        public string? color { get; set; }
    }
}
