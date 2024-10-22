using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerAndSellerInformation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PickUpAddress",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "ShippingAddress",
                table: "OrderDetails");

            migrationBuilder.CreateTable(
                name: "CustomerInformation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderDetailId = table.Column<Guid>(type: "uuid", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerInformation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerInformation_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerInformation_OrderDetails_OrderDetailId",
                        column: x => x.OrderDetailId,
                        principalTable: "OrderDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SellerInformation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SellerId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderDetailId = table.Column<Guid>(type: "uuid", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellerInformation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SellerInformation_OrderDetails_OrderDetailId",
                        column: x => x.OrderDetailId,
                        principalTable: "OrderDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SellerInformation_Sellers_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Sellers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerInformation_CustomerId",
                table: "CustomerInformation",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerInformation_OrderDetailId",
                table: "CustomerInformation",
                column: "OrderDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_SellerInformation_OrderDetailId",
                table: "SellerInformation",
                column: "OrderDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_SellerInformation_SellerId",
                table: "SellerInformation",
                column: "SellerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerInformation");

            migrationBuilder.DropTable(
                name: "SellerInformation");

            migrationBuilder.AddColumn<string>(
                name: "PickUpAddress",
                table: "OrderDetails",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress",
                table: "OrderDetails",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
