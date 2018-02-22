namespace Jurassic.So.Submission.Mock.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class MockGBContent : DbContext
    {
        public MockGBContent()
            : base("name=MockGBContent")
        {
        }

        public virtual DbSet<GB_File> GB_File { get; set; }
        public virtual DbSet<GB_Submission> GB_Submission { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GB_Submission>()
                .HasMany(e => e.GB_File)
                .WithOptional(e => e.GB_Submission)
                .HasForeignKey(e => e.SubmissiomId);
        }
    }
}
