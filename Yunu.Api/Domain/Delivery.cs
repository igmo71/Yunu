using System.Diagnostics.CodeAnalysis;

namespace Yunu.Api.Domain
{
    [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    public class Delivery
    {
        public int? id { get; set; }
        public double? amount { get; set; }
        public Address? address { get; set; }
    }

    [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    public class Address
    {
        public string? full { get; set; }
        public string? city { get; set; }
    }
}
