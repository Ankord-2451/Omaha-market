using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Omaha_market.Migrations
{
    /// <inheritdoc />
    public partial class Lang : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "price",
                table: "Products",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Products",
                newName: "NameRu");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Products",
                newName: "NameRo");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Products",
                newName: "DescriptionRu");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Category",
                newName: "NameRu");

            migrationBuilder.AddColumn<string>(
                name: "CategoryRo",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CategoryRu",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionRo",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameRo",
                table: "Category",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryRo",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CategoryRu",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DescriptionRo",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "NameRo",
                table: "Category");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Products",
                newName: "price");

            migrationBuilder.RenameColumn(
                name: "NameRu",
                table: "Products",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "NameRo",
                table: "Products",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "DescriptionRu",
                table: "Products",
                newName: "Category");

            migrationBuilder.RenameColumn(
                name: "NameRu",
                table: "Category",
                newName: "Name");
        }
    }
}
