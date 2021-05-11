using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace PlantTrade.Migrations
{
    public partial class Identity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<string>(maxLength: 45, nullable: false),
                    userName = table.Column<string>(maxLength: 45, nullable: false),
                    NormalizedUserName = table.Column<string>(nullable: true),
                    email = table.Column<string>(maxLength: 45, nullable: false),
                    NormalizedEmail = table.Column<string>(nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    postcode = table.Column<string>(maxLength: 15, nullable: false),
                    specialisme = table.Column<string>(maxLength: 45, nullable: true),
                    about = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "item",
                columns: table => new
                {
                    id = table.Column<int>(type: "int unsigned", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    kind = table.Column<string>(maxLength: 15, nullable: false),
                    name = table.Column<string>(maxLength: 45, nullable: false),
                    latinName = table.Column<string>(maxLength: 45, nullable: true),
                    sunHours = table.Column<int>(nullable: false),
                    transactionKind = table.Column<string>(maxLength: 15, nullable: false),
                    picture = table.Column<string>(maxLength: 250, nullable: false),
                    description = table.Column<string>(maxLength: 500, nullable: false),
                    userId = table.Column<string>(maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item", x => x.id);
                    table.ForeignKey(
                        name: "itemUser",
                        column: x => x.userId,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tutorial",
                columns: table => new
                {
                    id = table.Column<int>(type: "int unsigned", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    title = table.Column<string>(maxLength: 120, nullable: false),
                    link = table.Column<string>(maxLength: 120, nullable: false),
                    userId = table.Column<string>(maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tutorial", x => x.id);
                    table.ForeignKey(
                        name: "tutorialUser",
                        column: x => x.userId,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "conversation",
                columns: table => new
                {
                    id = table.Column<int>(type: "int unsigned", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    userId = table.Column<string>(maxLength: 45, nullable: true),
                    traderId = table.Column<string>(maxLength: 45, nullable: true),
                    itemId = table.Column<int>(type: "int unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_conversation", x => x.id);
                    table.ForeignKey(
                        name: "conversationItem",
                        column: x => x.itemId,
                        principalTable: "item",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "conversationTrader",
                        column: x => x.traderId,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "conversationUser",
                        column: x => x.userId,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "message",
                columns: table => new
                {
                    id = table.Column<int>(type: "int unsigned", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    message = table.Column<string>(maxLength: 500, nullable: false),
                    timeStamp = table.Column<string>(maxLength: 45, nullable: false),
                    userId = table.Column<string>(maxLength: 45, nullable: true),
                    conversationId = table.Column<int>(type: "int unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_message", x => x.id);
                    table.ForeignKey(
                        name: "messageConversation",
                        column: x => x.conversationId,
                        principalTable: "conversation",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "messageUser",
                        column: x => x.userId,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "id_UNIQUE",
                table: "conversation",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "conversationItem_idx",
                table: "conversation",
                column: "itemId");

            migrationBuilder.CreateIndex(
                name: "conversationTrader_idx",
                table: "conversation",
                column: "traderId");

            migrationBuilder.CreateIndex(
                name: "conversationUser_idx",
                table: "conversation",
                columns: new[] { "userId", "traderId" });

            migrationBuilder.CreateIndex(
                name: "id_UNIQUE",
                table: "item",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "itemUser_idx",
                table: "item",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "messageConversation_idx",
                table: "message",
                column: "conversationId");

            migrationBuilder.CreateIndex(
                name: "id_UNIQUE",
                table: "message",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "messageUser_idx",
                table: "message",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "id_UNIQUE",
                table: "tutorial",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "tutorialUser_idx",
                table: "tutorial",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "email_UNIQUE",
                table: "user",
                column: "email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "message");

            migrationBuilder.DropTable(
                name: "tutorial");

            migrationBuilder.DropTable(
                name: "conversation");

            migrationBuilder.DropTable(
                name: "item");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
