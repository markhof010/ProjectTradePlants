using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.AspNetCore.Identity;

namespace PlantTrade.Models
{
    public partial class planttradeContext : DbContext
    {
        public planttradeContext()
        {
        }

        public planttradeContext(DbContextOptions<planttradeContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Conversation> Conversation { get; set; }
        public virtual DbSet<Item> Item { get; set; }
        public virtual DbSet<Message> Message { get; set; }
        public virtual DbSet<Tutorial> Tutorial { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Report> Report { get; set; }
        public virtual DbSet<Feedback> Feedback { get; set; }

        public virtual DbSet<IdentityUserClaim<Guid>> IdentityUserClaims { get; set; }
        public virtual DbSet<IdentityUserClaim<string>> IdentityUserClaim { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Conversation>(entity =>
            {
                entity.ToTable("conversation");

                entity.HasIndex(e => e.Id)
                    .HasName("id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.ItemId)
                    .HasName("conversationItem_idx");

                entity.HasIndex(e => e.TraderId)
                    .HasName("conversationTrader_idx");

                entity.HasIndex(e => new { e.UserId, e.TraderId })
                    .HasName("conversationUser_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int unsigned");

                entity.Property(e => e.ItemId)
                    .HasColumnName("itemId")
                    .HasColumnType("int unsigned");

                entity.Property(e => e.TraderId)
                    .HasColumnName("traderId")
                    .HasMaxLength(45);

                entity.Property(e => e.UserId)
                    .HasColumnName("userId")
                    .HasMaxLength(45);

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.Conversation)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.NoAction)
                    .HasConstraintName("conversationItem");

                entity.HasOne(d => d.Trader)
                    .WithMany(p => p.ConversationTrader)
                    .HasForeignKey(d => d.TraderId)
                    .OnDelete(DeleteBehavior.NoAction)
                    .HasConstraintName("conversationTrader");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ConversationUser)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.NoAction)
                    .HasConstraintName("conversationUser");
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.ToTable("item");

                entity.HasIndex(e => e.Id)
                    .HasName("id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.UserId)
                    .HasName("itemUser_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int unsigned");

                entity.Property(e => e.Offer)
                    .HasColumnName("offer")
                    .HasMaxLength(45);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasMaxLength(500);

                entity.Property(e => e.Water)
                   .IsRequired()
                   .HasColumnName("water")
                   .HasMaxLength(45);

                entity.Property(e => e.Ground)
                   .IsRequired()
                   .HasColumnName("ground")
                   .HasMaxLength(45);

                entity.Property(e => e.Kind)
                    .IsRequired()
                    .HasColumnName("kind")
                    .HasMaxLength(15);

                entity.Property(e => e.LatinName)
                    .HasColumnName("latinName")
                    .HasMaxLength(45);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(45);

                entity.Property(e => e.Picture)
                    .IsRequired()
                    .HasColumnName("picture")
                    .HasMaxLength(250);

                entity.Property(e => e.SunHours).HasColumnName("sunHours");

                entity.Property(e => e.TransactionKind)
                    .IsRequired()
                    .HasColumnName("transactionKind")
                    .HasMaxLength(15);

                entity.Property(e => e.UserId)
                    .HasColumnName("userId")
                    .HasMaxLength(45);

                entity.Property(e => e.Available)
                    .HasColumnName("available")
                    .HasColumnType("int");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Item)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.NoAction)
                    .HasConstraintName("itemUser");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.ToTable("message");

                entity.HasIndex(e => e.ConversationId)
                    .HasName("messageConversation_idx");

                entity.HasIndex(e => e.Id)
                    .HasName("id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.UserId)
                    .HasName("messageUser_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int unsigned");

                entity.Property(e => e.ConversationId)
                    .HasColumnName("conversationId")
                    .HasColumnType("int unsigned");

                entity.Property(e => e.Message1)
                    .IsRequired()
                    .HasColumnName("message")
                    .HasMaxLength(500);

                entity.Property(e => e.TimeStamp)
                    .IsRequired()
                    .HasColumnName("timeStamp")
                    .HasMaxLength(45);

                entity.Property(e => e.UserId)
                    .HasColumnName("userId")
                    .HasMaxLength(45);

                entity.HasOne(d => d.Conversation)
                    .WithMany(p => p.Message)
                    .HasForeignKey(d => d.ConversationId)
                    .OnDelete(DeleteBehavior.NoAction)
                    .HasConstraintName("messageConversation");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Message)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.NoAction)
                    .HasConstraintName("messageUser");
            });

            modelBuilder.Entity<Tutorial>(entity =>
            {
                entity.ToTable("tutorial");

                entity.HasIndex(e => e.Id)
                    .HasName("id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.UserId)
                    .HasName("tutorialUser_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int unsigned");

                entity.Property(e => e.Link)
                    .IsRequired()
                    .HasColumnName("link")
                    .HasMaxLength(120);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(120);

                entity.Property(e => e.UserId)
                    .HasColumnName("userId")
                    .HasMaxLength(45);

                entity.Property(e => e.Available)
                    .HasColumnName("available")
                    .HasColumnType("int");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Tutorial)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.NoAction)
                    .HasConstraintName("tutorialUser");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.HasIndex(e => e.Email)
                    .HasName("email_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(45);

                entity.Property(e => e.About)
                    .HasColumnName("about")
                    .HasMaxLength(500);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(45);

                entity.Property(e => e.Postcode)
                    .IsRequired()
                    .HasColumnName("postcode")
                    .HasMaxLength(15);

                entity.Property(e => e.Specialisme)
                    .HasColumnName("specialisme")
                    .HasMaxLength(45);

                entity.Property(e => e.Role)
                    .HasColumnName("role")
                    .HasMaxLength(15);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnName("userName")
                    .HasMaxLength(45);
            });

            modelBuilder.Entity<Report>(entity =>
            {
                entity.ToTable("report");

                entity.HasIndex(e => e.Id)
                    .HasName("id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.ReporterId)
                    .HasName("reportUser_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int unsigned");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(500);

                entity.Property(e => e.Kind)
                    .HasColumnName("kind")
                    .HasMaxLength(15);

                entity.Property(e => e.ReportId)
                    .HasColumnName("reportId")
                    .HasMaxLength(50);

                entity.Property(e => e.ReporterId)
                    .HasColumnName("reporterId")
                    .HasMaxLength(45);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Report)
                    .HasForeignKey(d => d.ReporterId)
                    .OnDelete(DeleteBehavior.NoAction)
                    .HasConstraintName("reportUser");
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.ToTable("feedback");

                entity.HasIndex(e => e.Id)
                    .HasName("id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int unsigned");

                entity.Property(e => e.Page)
                    .HasColumnName("page")
                    .HasMaxLength(25);

                entity.Property(e => e.FeedbackText)
                    .HasColumnName("feedbackText")
                    .HasMaxLength(500);

                entity.Property(e => e.UserId)
                    .HasColumnName("userId")
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);

        }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
