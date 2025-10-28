using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Yunu.Api.Domain
{
    [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    public class CategoryTree
    {
        public List<Category>? tree { get; set; }
    }

    [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    public class Category
    {
        public int id { get; set; }

        public string? name { get; set; }

        public int parentId { get; set; }

        [NotMapped]
        public List<Category>? child { get; set; }

        public bool isProductsExists { get; set; }
    }
}
