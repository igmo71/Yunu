using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yunu.Api.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cabinet",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cabinet", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "CurrentStatus",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    label = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrentStatus", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Delivery",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    amount = table.Column<int>(type: "int", nullable: false),
                    address_full = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    address_city = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Delivery", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TransportCompany",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    color = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportCompany", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Warehouse",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouse", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    uid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentStatusId = table.Column<int>(type: "int", nullable: false),
                    updateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    addedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    paymentStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    amount = table.Column<int>(type: "int", nullable: false),
                    isFake = table.Column<bool>(type: "bit", nullable: false),
                    consumer_firstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    consumer_lastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    consumer_patronymic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    consumer_phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransportCompanyId = table.Column<int>(type: "int", nullable: false),
                    departureNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    tracking_number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeliveryId = table.Column<int>(type: "int", nullable: false),
                    WarehouseId = table.Column<int>(type: "int", nullable: false),
                    CabinetId = table.Column<int>(type: "int", nullable: false),
                    fromMarketplace = table.Column<bool>(type: "bit", nullable: false),
                    is_allowed_accept_as_defective = table.Column<bool>(type: "bit", nullable: false),
                    serviceCommission = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.id);
                    table.ForeignKey(
                        name: "FK_Order_Cabinet_CabinetId",
                        column: x => x.CabinetId,
                        principalTable: "Cabinet",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_CurrentStatus_CurrentStatusId",
                        column: x => x.CurrentStatusId,
                        principalTable: "CurrentStatus",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_Delivery_DeliveryId",
                        column: x => x.DeliveryId,
                        principalTable: "Delivery",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_TransportCompany_TransportCompanyId",
                        column: x => x.TransportCompanyId,
                        principalTable: "TransportCompany",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_Warehouse_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouse",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_CabinetId",
                table: "Order",
                column: "CabinetId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_CurrentStatusId",
                table: "Order",
                column: "CurrentStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_DeliveryId",
                table: "Order",
                column: "DeliveryId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_TransportCompanyId",
                table: "Order",
                column: "TransportCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_WarehouseId",
                table: "Order",
                column: "WarehouseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Cabinet");

            migrationBuilder.DropTable(
                name: "CurrentStatus");

            migrationBuilder.DropTable(
                name: "Delivery");

            migrationBuilder.DropTable(
                name: "TransportCompany");

            migrationBuilder.DropTable(
                name: "Warehouse");
        }
    }
}
