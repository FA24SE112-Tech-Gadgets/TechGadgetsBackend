using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModifyKeywordHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KeywordHistories_Customers_CustomerId",
                table: "KeywordHistories");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "KeywordHistories",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_KeywordHistories_CustomerId",
                table: "KeywordHistories",
                newName: "IX_KeywordHistories_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_KeywordHistories_Users_UserId",
                table: "KeywordHistories",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KeywordHistories_Users_UserId",
                table: "KeywordHistories");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "KeywordHistories",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_KeywordHistories_UserId",
                table: "KeywordHistories",
                newName: "IX_KeywordHistories_CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_KeywordHistories_Customers_CustomerId",
                table: "KeywordHistories",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
