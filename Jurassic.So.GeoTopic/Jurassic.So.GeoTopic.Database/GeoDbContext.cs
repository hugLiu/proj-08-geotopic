using Jurassic.So.GeoTopic.Database.Models;

namespace Jurassic.So.GeoTopic.Database
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Diagnostics;

    public partial class GeoDbContext : DbContext
    {
        public GeoDbContext() : base("name=DefaultConnection") { }
        public virtual DbSet<GT_IndexDefinition> GT_IndexDefinition { get; set; }
        public virtual DbSet<GT_TopicIndex> GT_TopicIndex { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GT_TopicCard>();
            modelBuilder.Entity<GT_UrlTemplate>();
            modelBuilder.Entity<webpages_Membership>();
            modelBuilder.Entity<GT_UserBehavior>();
            modelBuilder.Entity<GT_Code>();
            modelBuilder.Entity<webpages_Membership>();

            modelBuilder.Entity<GT_IndexDefinition>()
                .HasMany(e => e.GT_TopicIndex)
                .WithRequired(e => e.GT_IndexDefinition)
                .HasForeignKey(e => e.IndexDefinitionId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GT_Remark>()
                .HasMany(e => e.UserProfile1)
                .WithMany(e => e.GT_Remark1)
                .Map(m => m.ToTable("GT_RemarkPraise").MapLeftKey("RemarkId").MapRightKey("UserId"));

            modelBuilder.Entity<GT_RenderType>()
                .HasMany(e => e.GT_RenderUrl)
                .WithRequired(e => e.GT_RenderType)
                .HasForeignKey(e => e.RenderTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GT_RenderUrl>()
                .HasMany(e => e.GT_UrlTemplate)
                .WithRequired(e => e.GT_RenderUrl)
                .HasForeignKey(e => e.RenderUrlId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GT_Topic>()
                .HasMany(e => e.GT_TopicCard)
                .WithRequired(e => e.GT_Topic)
                .HasForeignKey(e => e.TopicId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GT_Topic>()
                .HasMany(e => e.GT_TopicIndex)
                .WithRequired(e => e.GT_Topic)
                .HasForeignKey(e => e.TopicId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GT_Topic>()
                .HasMany(e => e.GT_Topic1)
                .WithOptional(e => e.GT_Topic2)
                .HasForeignKey(e => e.PId);

            modelBuilder.Entity<GT_Topic>()
                .HasMany(e => e.webpages_Roles)
                .WithMany(e => e.GT_Topic)
                .Map(m => m.ToTable("GT_TopicInRoles").MapLeftKey("TopicId").MapRightKey("RoleId"));

            modelBuilder.Entity<UserProfile>()
                .ToTable("UserProfile")
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<UserProfile>()
                .Property(e => e.PhoneNumber)
                .IsUnicode(false);

            modelBuilder.Entity<UserProfile>()
                .HasMany(e => e.GT_Remark)
                .WithOptional(e => e.UserProfile)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<UserProfile>()
               .HasMany(e => e.GT_UserBehavior)
               .WithRequired(e => e.UserProfile)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<webpages_Roles>()
                .HasMany(e => e.UserProfile)
                .WithMany(e => e.webpages_Roles)
                .Map(m => m.ToTable("webpages_UsersInRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<GT_CodeType>()
                .HasMany(e => e.GT_Code)
                .WithRequired(e => e.GT_CodeType)
                .HasForeignKey(e => e.CodeTypeId)
                .WillCascadeOnDelete(false);
        }
    }
}
