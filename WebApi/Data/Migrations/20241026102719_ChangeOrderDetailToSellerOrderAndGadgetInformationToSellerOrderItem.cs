using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeOrderDetailToSellerOrderAndGadgetInformationToSellerOrderItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerInformation_OrderDetails_OrderDetailId",
                table: "CustomerInformation");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Gadgets_GadgetId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_OrderDetails_OrderDetailId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_SellerInformation_OrderDetails_OrderDetailId",
                table: "SellerInformation");

            migrationBuilder.DropForeignKey(
                name: "FK_WalletTrackings_OrderDetails_OrderDetailId",
                table: "WalletTrackings");

            migrationBuilder.DropTable(
                name: "GadgetInformation");

            migrationBuilder.DropTable(
                name: "SystemOrderDetailTrackings");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_SellerInformation_OrderDetailId",
                table: "SellerInformation");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_GadgetId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_OrderDetailId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_CustomerInformation_OrderDetailId",
                table: "CustomerInformation");

            migrationBuilder.DropColumn(
                name: "OrderDetailId",
                table: "SellerInformation");

            migrationBuilder.DropColumn(
                name: "GadgetId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "OrderDetailId",
                table: "CustomerInformation");

            migrationBuilder.RenameColumn(
                name: "OrderDetailId",
                table: "WalletTrackings",
                newName: "SellerOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_WalletTrackings_OrderDetailId",
                table: "WalletTrackings",
                newName: "IX_WalletTrackings_SellerOrderId");

            migrationBuilder.RenameColumn(
                name: "OrderDetailId",
                table: "Reviews",
                newName: "SellerOrderItemId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "SellerInformation",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "CustomerInformation",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "SellerOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    SellerId = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerInformationId = table.Column<Guid>(type: "uuid", nullable: false),
                    SellerInformationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellerOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SellerOrders_CustomerInformation_CustomerInformationId",
                        column: x => x.CustomerInformationId,
                        principalTable: "CustomerInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SellerOrders_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SellerOrders_SellerInformation_SellerInformationId",
                        column: x => x.SellerInformationId,
                        principalTable: "SellerInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SellerOrders_Sellers_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Sellers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SellerOrderItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SellerOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    GadgetPrice = table.Column<int>(type: "integer", nullable: false),
                    GadgetQuantity = table.Column<int>(type: "integer", nullable: false),
                    GadgetId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellerOrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SellerOrderItems_Gadgets_GadgetId",
                        column: x => x.GadgetId,
                        principalTable: "Gadgets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SellerOrderItems_SellerOrders_SellerOrderId",
                        column: x => x.SellerOrderId,
                        principalTable: "SellerOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SystemSellerOrderTrackings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SystemWalletId = table.Column<Guid>(type: "uuid", nullable: false),
                    SellerOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    FromUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ToUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemSellerOrderTrackings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SystemSellerOrderTrackings_SellerOrders_SellerOrderId",
                        column: x => x.SellerOrderId,
                        principalTable: "SellerOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SystemSellerOrderTrackings_SystemWallets_SystemWalletId",
                        column: x => x.SystemWalletId,
                        principalTable: "SystemWallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SystemSellerOrderTrackings_Users_FromUserId",
                        column: x => x.FromUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SystemSellerOrderTrackings_Users_ToUserId",
                        column: x => x.ToUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_SellerOrderItemId",
                table: "Reviews",
                column: "SellerOrderItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SellerOrderItems_GadgetId",
                table: "SellerOrderItems",
                column: "GadgetId");

            migrationBuilder.CreateIndex(
                name: "IX_SellerOrderItems_SellerOrderId",
                table: "SellerOrderItems",
                column: "SellerOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_SellerOrders_CustomerInformationId",
                table: "SellerOrders",
                column: "CustomerInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_SellerOrders_OrderId",
                table: "SellerOrders",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_SellerOrders_SellerId",
                table: "SellerOrders",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_SellerOrders_SellerInformationId",
                table: "SellerOrders",
                column: "SellerInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemSellerOrderTrackings_FromUserId",
                table: "SystemSellerOrderTrackings",
                column: "FromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemSellerOrderTrackings_SellerOrderId",
                table: "SystemSellerOrderTrackings",
                column: "SellerOrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SystemSellerOrderTrackings_SystemWalletId",
                table: "SystemSellerOrderTrackings",
                column: "SystemWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemSellerOrderTrackings_ToUserId",
                table: "SystemSellerOrderTrackings",
                column: "ToUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_SellerOrderItems_SellerOrderItemId",
                table: "Reviews",
                column: "SellerOrderItemId",
                principalTable: "SellerOrderItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WalletTrackings_SellerOrders_SellerOrderId",
                table: "WalletTrackings",
                column: "SellerOrderId",
                principalTable: "SellerOrders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_SellerOrderItems_SellerOrderItemId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_WalletTrackings_SellerOrders_SellerOrderId",
                table: "WalletTrackings");

            migrationBuilder.DropTable(
                name: "SellerOrderItems");

            migrationBuilder.DropTable(
                name: "SystemSellerOrderTrackings");

            migrationBuilder.DropTable(
                name: "SellerOrders");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_SellerOrderItemId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "SellerInformation");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "CustomerInformation");

            migrationBuilder.RenameColumn(
                name: "SellerOrderId",
                table: "WalletTrackings",
                newName: "OrderDetailId");

            migrationBuilder.RenameIndex(
                name: "IX_WalletTrackings_SellerOrderId",
                table: "WalletTrackings",
                newName: "IX_WalletTrackings_OrderDetailId");

            migrationBuilder.RenameColumn(
                name: "SellerOrderItemId",
                table: "Reviews",
                newName: "OrderDetailId");

            migrationBuilder.AddColumn<Guid>(
                name: "OrderDetailId",
                table: "SellerInformation",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "GadgetId",
                table: "Reviews",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "OrderDetailId",
                table: "CustomerInformation",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    SellerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Sellers_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Sellers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GadgetInformation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GadgetId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderDetailId = table.Column<Guid>(type: "uuid", nullable: false),
                    GadgetName = table.Column<string>(type: "text", nullable: false),
                    GadgetPrice = table.Column<int>(type: "integer", nullable: false),
                    GadgetQuantity = table.Column<int>(type: "integer", nullable: false),
                    GadgetThumbnailUrl = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GadgetInformation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GadgetInformation_Gadgets_GadgetId",
                        column: x => x.GadgetId,
                        principalTable: "Gadgets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GadgetInformation_OrderDetails_OrderDetailId",
                        column: x => x.OrderDetailId,
                        principalTable: "OrderDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SystemOrderDetailTrackings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FromUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderDetailId = table.Column<Guid>(type: "uuid", nullable: false),
                    SystemWalletId = table.Column<Guid>(type: "uuid", nullable: false),
                    ToUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemOrderDetailTrackings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SystemOrderDetailTrackings_OrderDetails_OrderDetailId",
                        column: x => x.OrderDetailId,
                        principalTable: "OrderDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SystemOrderDetailTrackings_SystemWallets_SystemWalletId",
                        column: x => x.SystemWalletId,
                        principalTable: "SystemWallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SystemOrderDetailTrackings_Users_FromUserId",
                        column: x => x.FromUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SystemOrderDetailTrackings_Users_ToUserId",
                        column: x => x.ToUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SellerInformation_OrderDetailId",
                table: "SellerInformation",
                column: "OrderDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_GadgetId",
                table: "Reviews",
                column: "GadgetId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_OrderDetailId",
                table: "Reviews",
                column: "OrderDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerInformation_OrderDetailId",
                table: "CustomerInformation",
                column: "OrderDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_GadgetInformation_GadgetId",
                table: "GadgetInformation",
                column: "GadgetId");

            migrationBuilder.CreateIndex(
                name: "IX_GadgetInformation_OrderDetailId",
                table: "GadgetInformation",
                column: "OrderDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_SellerId",
                table: "OrderDetails",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemOrderDetailTrackings_FromUserId",
                table: "SystemOrderDetailTrackings",
                column: "FromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemOrderDetailTrackings_OrderDetailId",
                table: "SystemOrderDetailTrackings",
                column: "OrderDetailId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SystemOrderDetailTrackings_SystemWalletId",
                table: "SystemOrderDetailTrackings",
                column: "SystemWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemOrderDetailTrackings_ToUserId",
                table: "SystemOrderDetailTrackings",
                column: "ToUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerInformation_OrderDetails_OrderDetailId",
                table: "CustomerInformation",
                column: "OrderDetailId",
                principalTable: "OrderDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Gadgets_GadgetId",
                table: "Reviews",
                column: "GadgetId",
                principalTable: "Gadgets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_OrderDetails_OrderDetailId",
                table: "Reviews",
                column: "OrderDetailId",
                principalTable: "OrderDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SellerInformation_OrderDetails_OrderDetailId",
                table: "SellerInformation",
                column: "OrderDetailId",
                principalTable: "OrderDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WalletTrackings_OrderDetails_OrderDetailId",
                table: "WalletTrackings",
                column: "OrderDetailId",
                principalTable: "OrderDetails",
                principalColumn: "Id");
        }
    }
}
