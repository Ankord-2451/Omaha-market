using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Omaha_market.Migrations
{
    /// <inheritdoc />
    public partial class Add_dateOfLastChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfLastChange",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfLastChange",
                table: "Products");
        }
    }
}
