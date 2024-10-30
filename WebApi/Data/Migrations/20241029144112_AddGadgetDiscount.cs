using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Pgvector;

#nullable disable

namespace WebApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGadgetDiscount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SearchAIVectors");

            migrationBuilder.DropTable(
                name: "SearchAIs");

            migrationBuilder.AddColumn<Guid>(
                name: "GadgetDiscountId",
                table: "SellerOrderItems",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Vector>(
                name: "Vector",
                table: "Gadgets",
                type: "vector(1536)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GadgetDiscounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GadgetId = table.Column<Guid>(type: "uuid", nullable: false),
                    DiscountPercentage = table.Column<int>(type: "integer", nullable: false),
                    ExpiredDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GadgetDiscounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GadgetDiscounts_Gadgets_GadgetId",
                        column: x => x.GadgetId,
                        principalTable: "Gadgets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SellerOrderItems_GadgetDiscountId",
                table: "SellerOrderItems",
                column: "GadgetDiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_GadgetDiscounts_GadgetId",
                table: "GadgetDiscounts",
                column: "GadgetId");

            migrationBuilder.AddForeignKey(
                name: "FK_SellerOrderItems_GadgetDiscounts_GadgetDiscountId",
                table: "SellerOrderItems",
                column: "GadgetDiscountId",
                principalTable: "GadgetDiscounts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SellerOrderItems_GadgetDiscounts_GadgetDiscountId",
                table: "SellerOrderItems");

            migrationBuilder.DropTable(
                name: "GadgetDiscounts");

            migrationBuilder.DropIndex(
                name: "IX_SellerOrderItems_GadgetDiscountId",
                table: "SellerOrderItems");

            migrationBuilder.DropColumn(
                name: "GadgetDiscountId",
                table: "SellerOrderItems");

            migrationBuilder.DropColumn(
                name: "Vector",
                table: "Gadgets");

            migrationBuilder.CreateTable(
                name: "SearchAIs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CanAddMore = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchAIs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SearchAIVectors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SearchAIId = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false),
                    Vector = table.Column<Vector>(type: "vector(384)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchAIVectors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SearchAIVectors_SearchAIs_SearchAIId",
                        column: x => x.SearchAIId,
                        principalTable: "SearchAIs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SearchAIVectors_SearchAIId",
                table: "SearchAIVectors",
                column: "SearchAIId");
        }
    }
}
