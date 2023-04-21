using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Omaha_market.Migrations
{
    /// <inheritdoc />
    public partial class deleteRecentP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecentPurchases");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RecentPurchases",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdOfCustomer = table.Column<int>(type: "int", nullable: false),
                    IdOfProduct = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecentPurchases", x => x.ID);
                });
        }
    }
}
