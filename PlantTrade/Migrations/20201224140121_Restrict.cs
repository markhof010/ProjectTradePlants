using Microsoft.EntityFrameworkCore.Migrations;

namespace PlantTrade.Migrations
{
    public partial class Restrict : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "conversationItem",
                table: "conversation");

            migrationBuilder.DropForeignKey(
                name: "conversationTrader",
                table: "conversation");

            migrationBuilder.DropForeignKey(
                name: "conversationUser",
                table: "conversation");

            migrationBuilder.DropForeignKey(
                name: "itemUser",
                table: "item");

            migrationBuilder.DropForeignKey(
                name: "messageConversation",
                table: "message");

            migrationBuilder.DropForeignKey(
                name: "messageUser",
                table: "message");

            migrationBuilder.DropForeignKey(
                name: "reportUser",
                table: "report");

            migrationBuilder.DropForeignKey(
                name: "tutorialUser",
                table: "tutorial");

            migrationBuilder.AddForeignKey(
                name: "conversationItem",
                table: "conversation",
                column: "itemId",
                principalTable: "item",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "conversationTrader",
                table: "conversation",
                column: "traderId",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "conversationUser",
                table: "conversation",
                column: "userId",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "itemUser",
                table: "item",
                column: "userId",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "messageConversation",
                table: "message",
                column: "conversationId",
                principalTable: "conversation",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "messageUser",
                table: "message",
                column: "userId",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "reportUser",
                table: "report",
                column: "reporterId",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "tutorialUser",
                table: "tutorial",
                column: "userId",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "conversationItem",
                table: "conversation");

            migrationBuilder.DropForeignKey(
                name: "conversationTrader",
                table: "conversation");

            migrationBuilder.DropForeignKey(
                name: "conversationUser",
                table: "conversation");

            migrationBuilder.DropForeignKey(
                name: "itemUser",
                table: "item");

            migrationBuilder.DropForeignKey(
                name: "messageConversation",
                table: "message");

            migrationBuilder.DropForeignKey(
                name: "messageUser",
                table: "message");

            migrationBuilder.DropForeignKey(
                name: "reportUser",
                table: "report");

            migrationBuilder.DropForeignKey(
                name: "tutorialUser",
                table: "tutorial");

            migrationBuilder.AddForeignKey(
                name: "conversationItem",
                table: "conversation",
                column: "itemId",
                principalTable: "item",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "conversationTrader",
                table: "conversation",
                column: "traderId",
                principalTable: "user",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "conversationUser",
                table: "conversation",
                column: "userId",
                principalTable: "user",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "itemUser",
                table: "item",
                column: "userId",
                principalTable: "user",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "messageConversation",
                table: "message",
                column: "conversationId",
                principalTable: "conversation",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "messageUser",
                table: "message",
                column: "userId",
                principalTable: "user",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "reportUser",
                table: "report",
                column: "reporterId",
                principalTable: "user",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "tutorialUser",
                table: "tutorial",
                column: "userId",
                principalTable: "user",
                principalColumn: "id");
        }
    }
}
