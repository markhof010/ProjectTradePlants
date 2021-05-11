using Microsoft.EntityFrameworkCore.Migrations;

namespace PlantTrade.Migrations
{
    public partial class ReportUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "reporterId",
                table: "report",
                maxLength: 45,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "reportUser_idx",
                table: "report",
                column: "reporterId");

            migrationBuilder.AddForeignKey(
                name: "reportUser",
                table: "report",
                column: "reporterId",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "reportUser",
                table: "report");

            migrationBuilder.DropIndex(
                name: "reportUser_idx",
                table: "report");

            migrationBuilder.DropColumn(
                name: "reporterId",
                table: "report");
        }
    }
}
