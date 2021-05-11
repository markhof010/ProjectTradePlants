using Microsoft.EntityFrameworkCore.Migrations;

namespace PlantTrade.Migrations
{
    public partial class changeItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "available",
                table: "item",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ground",
                table: "item",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "offer",
                table: "item",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "water",
                table: "item",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "available",
                table: "item");

            migrationBuilder.DropColumn(
                name: "ground",
                table: "item");

            migrationBuilder.DropColumn(
                name: "offer",
                table: "item");

            migrationBuilder.DropColumn(
                name: "water",
                table: "item");
        }
    }
}
