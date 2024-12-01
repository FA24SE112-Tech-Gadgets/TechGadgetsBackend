using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class NaturalLanguageKeywordAndCriteria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NaturalLanguageKeywordGroup",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NaturalLanguageKeywordGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Criteria",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NaturalLanguageKeywordGroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    SpecificationKeyId = table.Column<Guid>(type: "uuid", nullable: true),
                    Contains = table.Column<string>(type: "text", nullable: true),
                    MinPrice = table.Column<int>(type: "integer", nullable: true),
                    MaxPrice = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Criteria", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Criteria_NaturalLanguageKeywordGroup_NaturalLanguageKeyword~",
                        column: x => x.NaturalLanguageKeywordGroupId,
                        principalTable: "NaturalLanguageKeywordGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Criteria_SpecificationKeys_SpecificationKeyId",
                        column: x => x.SpecificationKeyId,
                        principalTable: "SpecificationKeys",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "NaturalLanguageKeyword",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NaturalLanguageKeywordGroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    Keyword = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NaturalLanguageKeyword", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NaturalLanguageKeyword_NaturalLanguageKeywordGroup_NaturalL~",
                        column: x => x.NaturalLanguageKeywordGroupId,
                        principalTable: "NaturalLanguageKeywordGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CriteriaCategory",
                columns: table => new
                {
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    CriteriaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CriteriaCategory", x => new { x.CategoryId, x.CriteriaId });
                    table.ForeignKey(
                        name: "FK_CriteriaCategory_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CriteriaCategory_Criteria_CriteriaId",
                        column: x => x.CriteriaId,
                        principalTable: "Criteria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Criteria_NaturalLanguageKeywordGroupId",
                table: "Criteria",
                column: "NaturalLanguageKeywordGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Criteria_SpecificationKeyId",
                table: "Criteria",
                column: "SpecificationKeyId");

            migrationBuilder.CreateIndex(
                name: "IX_CriteriaCategory_CriteriaId",
                table: "CriteriaCategory",
                column: "CriteriaId");

            migrationBuilder.CreateIndex(
                name: "IX_NaturalLanguageKeyword_NaturalLanguageKeywordGroupId",
                table: "NaturalLanguageKeyword",
                column: "NaturalLanguageKeywordGroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CriteriaCategory");

            migrationBuilder.DropTable(
                name: "NaturalLanguageKeyword");

            migrationBuilder.DropTable(
                name: "Criteria");

            migrationBuilder.DropTable(
                name: "NaturalLanguageKeywordGroup");
        }
    }
}
