using Microsoft.EntityFrameworkCore.Migrations;

namespace PlantTrade.Migrations
{
    public partial class changeFeedback : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "feedbackUser",
                table: "feedback");

            migrationBuilder.DropIndex(
                name: "feedbackUser_idx",
                table: "feedback");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "feedbackUser_idx",
                table: "feedback",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "feedbackUser",
                table: "feedback",
                column: "userId",
                principalTable: "user",
                principalColumn: "id");
        }
    }
}
