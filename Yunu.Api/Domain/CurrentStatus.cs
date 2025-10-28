using System.Diagnostics.CodeAnalysis;

namespace Yunu.Api.Domain
{
    [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    public class CurrentStatus
    {
        public int id { get; set; }
        public string? label { get; set; }
    }
}
