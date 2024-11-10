using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSellerOrderIdToNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SellerOrderId",
                table: "Notifications",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_SellerOrderId",
                table: "Notifications",
                column: "SellerOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_SellerOrders_SellerOrderId",
                table: "Notifications",
                column: "SellerOrderId",
                principalTable: "SellerOrders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_SellerOrders_SellerOrderId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_SellerOrderId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "SellerOrderId",
                table: "Notifications");
        }
    }
}
