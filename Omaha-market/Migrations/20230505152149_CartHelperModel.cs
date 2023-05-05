using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Omaha_market.Migrations
{
    /// <inheritdoc />
    public partial class CartHelperModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "ShoppingCart",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "ShoppingCart");
        }
    }
}
