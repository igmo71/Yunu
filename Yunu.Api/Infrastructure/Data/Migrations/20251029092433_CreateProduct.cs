using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yunu.Api.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    parentId = table.Column<int>(type: "int", nullable: false),
                    isProductsExists = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    type = table.Column<int>(type: "int", nullable: false),
                    article = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    categoryId = table.Column<int>(type: "int", nullable: false),
                    category_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    price = table.Column<int>(type: "int", nullable: false),
                    oldPrice = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    photo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    reserve = table.Column<int>(type: "int", nullable: false),
                    marriage = table.Column<int>(type: "int", nullable: false),
                    is_fragile = table.Column<bool>(type: "bit", nullable: false),
                    is_with_video_record = table.Column<bool>(type: "bit", nullable: false),
                    is_required_cis = table.Column<bool>(type: "bit", nullable: false),
                    instruction_link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    instruction_text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_box = table.Column<bool>(type: "bit", nullable: false),
                    is_pack_in_bubble_wrap = table.Column<bool>(type: "bit", nullable: false),
                    is_pack_in_cardboard = table.Column<bool>(type: "bit", nullable: false),
                    is_hidden = table.Column<bool>(type: "bit", nullable: false),
                    is_fifo = table.Column<bool>(type: "bit", nullable: false),
                    is_expirable = table.Column<bool>(type: "bit", nullable: false),
                    is_dimensions_diff = table.Column<bool>(type: "bit", nullable: false),
                    is_bad_dimensions_from_mp = table.Column<bool>(type: "bit", nullable: false),
                    country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    measuredInWarehouse = table.Column<bool>(type: "bit", nullable: false),
                    height = table.Column<int>(type: "int", nullable: false),
                    width = table.Column<int>(type: "int", nullable: false),
                    length = table.Column<int>(type: "int", nullable: false),
                    volume = table.Column<float>(type: "real", nullable: false),
                    commission_acceptance = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    commission_pickup_from_warehouse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    commission_fbo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    comission = table.Column<int>(type: "int", nullable: false),
                    weight = table.Column<float>(type: "real", nullable: false),
                    foreign_purchase_price = table.Column<float>(type: "real", nullable: true),
                    foreign_purchase_price_currency_symbol = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    foreign_delivery_price = table.Column<float>(type: "real", nullable: true),
                    foreign_delivery_price_currency_symbol = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    calculated_purchase_price = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Product_Fbo_Stocks",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    total = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product_Fbo_Stocks", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Product_Fbo_Stocks_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Product_Marketplaces",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    YANDEX_MARKET_FBS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YANDEX_MARKET_FBS_FAST = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WILDBERRIES_FBS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OZON_FBS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OZON_REAL_FBS_EXPRESS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SBER_FBS = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product_Marketplaces", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Product_Marketplaces_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Product_Fbo_Stocks_By_Delivery_Type",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    delivery_type_name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    delivery_type_color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product_Fbo_Stocks_By_Delivery_Type", x => new { x.ProductId, x.delivery_type_name });
                    table.ForeignKey(
                        name: "FK_Product_Fbo_Stocks_By_Delivery_Type_Product_Fbo_Stocks_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product_Fbo_Stocks",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Product_Fbo_Stocks_By_Delivery_Type");

            migrationBuilder.DropTable(
                name: "Product_Marketplaces");

            migrationBuilder.DropTable(
                name: "Product_Fbo_Stocks");

            migrationBuilder.DropTable(
                name: "Product");
        }
    }
}
