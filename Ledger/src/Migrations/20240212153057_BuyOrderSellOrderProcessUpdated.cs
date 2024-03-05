using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ledger.Migrations
{
    public partial class BuyOrderSellOrderProcessUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AskPrice",
                table: "SellOrderJobs",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "StockId",
                table: "SellOrderJobs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "BidPrice",
                table: "BuyOrderJobs",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "StockId",
                table: "BuyOrderJobs",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AskPrice",
                table: "SellOrderJobs");

            migrationBuilder.DropColumn(
                name: "StockId",
                table: "SellOrderJobs");

            migrationBuilder.DropColumn(
                name: "BidPrice",
                table: "BuyOrderJobs");

            migrationBuilder.DropColumn(
                name: "StockId",
                table: "BuyOrderJobs");
        }
    }
}
