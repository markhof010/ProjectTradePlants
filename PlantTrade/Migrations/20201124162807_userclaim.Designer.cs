// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PlantTrade.Models;

namespace PlantTrade.Migrations
{
    [DbContext(typeof(planttradeContext))]
    [Migration("20201124162807_userclaim")]
    partial class userclaim
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("IdentityUserClaims");
                });

            modelBuilder.Entity("PlantTrade.Models.Conversation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int unsigned");

                    b.Property<int>("ItemId")
                        .HasColumnName("itemId")
                        .HasColumnType("int unsigned");

                    b.Property<string>("TraderId")
                        .HasColumnName("traderId")
                        .HasColumnType("varchar(45)")
                        .HasMaxLength(45);

                    b.Property<string>("UserId")
                        .HasColumnName("userId")
                        .HasColumnType("varchar(45)")
                        .HasMaxLength(45);

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasName("id_UNIQUE");

                    b.HasIndex("ItemId")
                        .HasName("conversationItem_idx");

                    b.HasIndex("TraderId")
                        .HasName("conversationTrader_idx");

                    b.HasIndex("UserId", "TraderId")
                        .HasName("conversationUser_idx");

                    b.ToTable("conversation");
                });

            modelBuilder.Entity("PlantTrade.Models.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int unsigned");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnName("description")
                        .HasColumnType("varchar(500)")
                        .HasMaxLength(500);

                    b.Property<string>("Kind")
                        .IsRequired()
                        .HasColumnName("kind")
                        .HasColumnType("varchar(15)")
                        .HasMaxLength(15);

                    b.Property<string>("LatinName")
                        .HasColumnName("latinName")
                        .HasColumnType("varchar(45)")
                        .HasMaxLength(45);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name")
                        .HasColumnType("varchar(45)")
                        .HasMaxLength(45);

                    b.Property<string>("Picture")
                        .IsRequired()
                        .HasColumnName("picture")
                        .HasColumnType("varchar(250)")
                        .HasMaxLength(250);

                    b.Property<int>("SunHours")
                        .HasColumnName("sunHours")
                        .HasColumnType("int");

                    b.Property<string>("TransactionKind")
                        .IsRequired()
                        .HasColumnName("transactionKind")
                        .HasColumnType("varchar(15)")
                        .HasMaxLength(15);

                    b.Property<string>("UserId")
                        .HasColumnName("userId")
                        .HasColumnType("varchar(45)")
                        .HasMaxLength(45);

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasName("id_UNIQUE");

                    b.HasIndex("UserId")
                        .HasName("itemUser_idx");

                    b.ToTable("item");
                });

            modelBuilder.Entity("PlantTrade.Models.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int unsigned");

                    b.Property<int>("ConversationId")
                        .HasColumnName("conversationId")
                        .HasColumnType("int unsigned");

                    b.Property<string>("Message1")
                        .IsRequired()
                        .HasColumnName("message")
                        .HasColumnType("varchar(500)")
                        .HasMaxLength(500);

                    b.Property<string>("TimeStamp")
                        .IsRequired()
                        .HasColumnName("timeStamp")
                        .HasColumnType("varchar(45)")
                        .HasMaxLength(45);

                    b.Property<string>("UserId")
                        .HasColumnName("userId")
                        .HasColumnType("varchar(45)")
                        .HasMaxLength(45);

                    b.HasKey("Id");

                    b.HasIndex("ConversationId")
                        .HasName("messageConversation_idx");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasName("id_UNIQUE");

                    b.HasIndex("UserId")
                        .HasName("messageUser_idx");

                    b.ToTable("message");
                });

            modelBuilder.Entity("PlantTrade.Models.Tutorial", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int unsigned");

                    b.Property<string>("Link")
                        .IsRequired()
                        .HasColumnName("link")
                        .HasColumnType("varchar(120)")
                        .HasMaxLength(120);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnName("title")
                        .HasColumnType("varchar(120)")
                        .HasMaxLength(120);

                    b.Property<string>("UserId")
                        .HasColumnName("userId")
                        .HasColumnType("varchar(45)")
                        .HasMaxLength(45);

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasName("id_UNIQUE");

                    b.HasIndex("UserId")
                        .HasName("tutorialUser_idx");

                    b.ToTable("tutorial");
                });

            modelBuilder.Entity("PlantTrade.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnName("id")
                        .HasColumnType("varchar(45)")
                        .HasMaxLength(45);

                    b.Property<string>("About")
                        .HasColumnName("about")
                        .HasColumnType("varchar(500)")
                        .HasMaxLength(500);

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnName("email")
                        .HasColumnType("varchar(45)")
                        .HasMaxLength(45);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("text");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Postcode")
                        .IsRequired()
                        .HasColumnName("postcode")
                        .HasColumnType("varchar(15)")
                        .HasMaxLength(15);

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<string>("Specialisme")
                        .HasColumnName("specialisme")
                        .HasColumnType("varchar(45)")
                        .HasMaxLength(45);

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnName("userName")
                        .HasColumnType("varchar(45)")
                        .HasMaxLength(45);

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasName("email_UNIQUE");

                    b.ToTable("user");
                });

            modelBuilder.Entity("PlantTrade.Models.Conversation", b =>
                {
                    b.HasOne("PlantTrade.Models.Item", "Item")
                        .WithMany("Conversation")
                        .HasForeignKey("ItemId")
                        .HasConstraintName("conversationItem")
                        .IsRequired();

                    b.HasOne("PlantTrade.Models.User", "Trader")
                        .WithMany("ConversationTrader")
                        .HasForeignKey("TraderId")
                        .HasConstraintName("conversationTrader");

                    b.HasOne("PlantTrade.Models.User", "User")
                        .WithMany("ConversationUser")
                        .HasForeignKey("UserId")
                        .HasConstraintName("conversationUser");
                });

            modelBuilder.Entity("PlantTrade.Models.Item", b =>
                {
                    b.HasOne("PlantTrade.Models.User", "User")
                        .WithMany("Item")
                        .HasForeignKey("UserId")
                        .HasConstraintName("itemUser");
                });

            modelBuilder.Entity("PlantTrade.Models.Message", b =>
                {
                    b.HasOne("PlantTrade.Models.Conversation", "Conversation")
                        .WithMany("Message")
                        .HasForeignKey("ConversationId")
                        .HasConstraintName("messageConversation")
                        .IsRequired();

                    b.HasOne("PlantTrade.Models.User", "User")
                        .WithMany("Message")
                        .HasForeignKey("UserId")
                        .HasConstraintName("messageUser");
                });

            modelBuilder.Entity("PlantTrade.Models.Tutorial", b =>
                {
                    b.HasOne("PlantTrade.Models.User", "User")
                        .WithMany("Tutorial")
                        .HasForeignKey("UserId")
                        .HasConstraintName("tutorialUser");
                });
#pragma warning restore 612, 618
        }
    }
}
