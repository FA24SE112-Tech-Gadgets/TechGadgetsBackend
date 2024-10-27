using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Pgvector;

#nullable disable

namespace WebApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveGadgetFilterOption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GadgetFilters_Categories_CategoryId",
                table: "GadgetFilters");

            migrationBuilder.DropTable(
                name: "GadgetFilterOptions");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "GadgetFilters",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "GadgetFilters",
                newName: "SpecificationKeyId");

            migrationBuilder.RenameIndex(
                name: "IX_GadgetFilters_CategoryId",
                table: "GadgetFilters",
                newName: "IX_GadgetFilters_SpecificationKeyId");

            migrationBuilder.AddColumn<Guid>(
                name: "SpecificationUnitId",
                table: "GadgetFilters",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Vector>(
                name: "Vector",
                table: "GadgetFilters",
                type: "vector",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_GadgetFilters_SpecificationUnitId",
                table: "GadgetFilters",
                column: "SpecificationUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_GadgetFilters_SpecificationKeys_SpecificationKeyId",
                table: "GadgetFilters",
                column: "SpecificationKeyId",
                principalTable: "SpecificationKeys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GadgetFilters_SpecificationUnits_SpecificationUnitId",
                table: "GadgetFilters",
                column: "SpecificationUnitId",
                principalTable: "SpecificationUnits",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GadgetFilters_SpecificationKeys_SpecificationKeyId",
                table: "GadgetFilters");

            migrationBuilder.DropForeignKey(
                name: "FK_GadgetFilters_SpecificationUnits_SpecificationUnitId",
                table: "GadgetFilters");

            migrationBuilder.DropIndex(
                name: "IX_GadgetFilters_SpecificationUnitId",
                table: "GadgetFilters");

            migrationBuilder.DropColumn(
                name: "SpecificationUnitId",
                table: "GadgetFilters");

            migrationBuilder.DropColumn(
                name: "Vector",
                table: "GadgetFilters");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "GadgetFilters",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "SpecificationKeyId",
                table: "GadgetFilters",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_GadgetFilters_SpecificationKeyId",
                table: "GadgetFilters",
                newName: "IX_GadgetFilters_CategoryId");

            migrationBuilder.CreateTable(
                name: "GadgetFilterOptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GadgetFilterId = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GadgetFilterOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GadgetFilterOptions_GadgetFilters_GadgetFilterId",
                        column: x => x.GadgetFilterId,
                        principalTable: "GadgetFilters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GadgetFilterOptions_GadgetFilterId",
                table: "GadgetFilterOptions",
                column: "GadgetFilterId");

            migrationBuilder.AddForeignKey(
                name: "FK_GadgetFilters_Categories_CategoryId",
                table: "GadgetFilters",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
