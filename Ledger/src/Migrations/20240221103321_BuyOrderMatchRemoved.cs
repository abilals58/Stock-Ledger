using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ledger.Migrations
{
    public partial class BuyOrderMatchRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BuyOrderMatches");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BuyOrderMatches",
                columns: table => new
                {
                    BuyOrderId = table.Column<int>(type: "integer", nullable: false),
                    SellOrderId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuyOrderMatches", x => new { x.BuyOrderId, x.SellOrderId });
                });
        }
    }
}
