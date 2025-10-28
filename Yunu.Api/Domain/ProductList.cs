using System.Diagnostics.CodeAnalysis;

namespace Yunu.Api.Domain;

[SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
public class ProductList
{
    public List<Product>? list { get; set; }
    public int total { get; set; }
    public string? exportLink { get; set; }
    public string? templateMassCreateLink { get; set; }
}

[SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
public class Product
{
    public int id { get; set; }
    public int type { get; set; }
    public string? article { get; set; }
    public int categoryId { get; set; }
    public string? category_name { get; set; }
    public string? name { get; set; }
    public int price { get; set; }
    public int oldPrice { get; set; }
    public int quantity { get; set; }
    public Product_Fbo_Stock? fbo_stock { get; set; }
    public Product_Fbo_Stocks? fbo_stocks { get; set; }
    public string? photo { get; set; }
    public int reserve { get; set; }
    public int marriage { get; set; }
    public bool is_fragile { get; set; }
    public bool is_with_video_record { get; set; }
    public bool is_required_cis { get; set; }
    public string? instruction_link { get; set; }
    public string? instruction_text { get; set; }
    public bool is_box { get; set; }
    public bool is_pack_in_bubble_wrap { get; set; }
    public bool is_pack_in_cardboard { get; set; }
    public bool is_hidden { get; set; }
    public bool is_fifo { get; set; }
    public bool is_expirable { get; set; }
    public bool is_dimensions_diff { get; set; }
    public bool is_bad_dimensions_from_mp { get; set; }
    public string? country { get; set; }
    public bool measuredInWarehouse { get; set; }
    public int height { get; set; }
    public int width { get; set; }
    public int length { get; set; }
    public float volume { get; set; }
    public string? commission_acceptance { get; set; }
    public string? commission_pickup_from_warehouse { get; set; }
    public string? commission_fbo { get; set; }
    public int comission { get; set; }
    public float weight { get; set; }
    public Product_Marketplaces? marketplaces { get; set; }
    public float? foreign_purchase_price { get; set; }
    public string? foreign_purchase_price_currency_symbol { get; set; }
    public float? foreign_delivery_price { get; set; }
    public string? foreign_delivery_price_currency_symbol { get; set; }
    public float calculated_purchase_price { get; set; }
}

[SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
public class Product_Fbo_Stock
{
    public int ProductId { get; set; } // My Key

    public int total { get; set; }
    public List<Product_Fbo_Stock_By_Delivery_Type>? by_delivery_type { get; set; }
}

[SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
public class Product_Fbo_Stock_By_Delivery_Type
{
    public int ProductId { get; set; } // My Key

    public string? delivery_type_name { get; set; }
    public string? delivery_type_color { get; set; }
    public int quantity { get; set; }
}

[SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
public class Product_Fbo_Stocks
{
    public int ProductId { get; set; } // My Key

    public int total { get; set; }
    public List<Product_Fbo_Stocks_By_Delivery_Type>? by_delivery_type { get; set; }
}

[SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
public class Product_Fbo_Stocks_By_Delivery_Type
{
    public int ProductId { get; set; } // My Key

    public string? delivery_type_name { get; set; }
    public string? delivery_type_color { get; set; }
    public int quantity { get; set; }
}

public class Product_Marketplaces
{
    public int ProductId { get; set; } // My Key

    public string? YANDEX_MARKET_FBS { get; set; }
    public string? YANDEX_MARKET_FBS_FAST { get; set; }
    public string? WILDBERRIES_FBS { get; set; }
    public string? OZON_FBS { get; set; }
    public string? OZON_REAL_FBS_EXPRESS { get; set; }
    public string? SBER_FBS { get; set; }
}
