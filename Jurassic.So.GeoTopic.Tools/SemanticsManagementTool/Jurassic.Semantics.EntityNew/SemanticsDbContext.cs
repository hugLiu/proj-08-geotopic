using Jurassic.Semantics.Entity.EntityModel;

namespace Jurassic.Semantics.EntityNew
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class SemanticsDbContext : DbContext
    {
        public SemanticsDbContext()
            : base("name=SemanticsDbContext")
        {
        }

        public virtual DbSet<SD_CCTerm> SD_CCTerm { get; set; }
        public virtual DbSet<SD_ConceptClass> SD_ConceptClass { get; set; }
        public virtual DbSet<SD_Semantics> SD_Semantics { get; set; }
        public virtual DbSet<SD_SemanticsType> SD_SemanticsType { get; set; }
        public virtual DbSet<SD_TermKeyword> SD_TermKeyword { get; set; }
        public virtual DbSet<SD_TermSource> SD_TermSource { get; set; }
        public virtual DbSet<SD_TermTranslation> SD_TermTranslation { get; set; }
        public virtual DbSet<Dict_ProfessionalView> Dict_ProfessionalView { get; set; }
        public virtual DbSet<ES_EPsOfPTView> ES_EPsOfPTView { get; set; }
        public virtual DbSet<ES_SemanticsTermView> ES_SemanticsTermView { get; set; }
        public virtual DbSet<SMT_BFTreeView> SMT_BFTreeView { get; set; }
        public virtual DbSet<SMT_GNTreeView> SMT_GNTreeView { get; set; }
        public virtual DbSet<SMT_BOTTreeView> SMT_BOTTreeView { get; set; }
        public virtual DbSet<SMT_PTContextView> SMT_PTContextView { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SD_CCTerm>()
                .Property(e => e.CCCode)
                .IsUnicode(false);

            modelBuilder.Entity<SD_CCTerm>()
                .Property(e => e.LangCode)
                .IsUnicode(false);

            modelBuilder.Entity<SD_CCTerm>()
                .HasMany(e => e.SD_Semantics)
                .WithRequired(e => e.SD_CCTerm)
                .HasForeignKey(e => e.FTermClassId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SD_CCTerm>()
                .HasMany(e => e.SD_Semantics1)
                .WithRequired(e => e.SD_CCTerm1)
                .HasForeignKey(e => e.LTermClassId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SD_CCTerm>()
                .HasMany(e => e.SD_TermKeyword)
                .WithRequired(e => e.SD_CCTerm)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SD_CCTerm>()
                .HasMany(e => e.SD_TermTranslation)
                .WithRequired(e => e.SD_CCTerm)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SD_ConceptClass>()
                .Property(e => e.CCCode)
                .IsUnicode(false);

            modelBuilder.Entity<SD_ConceptClass>()
                .Property(e => e.Tag)
                .IsUnicode(false);

            modelBuilder.Entity<SD_Semantics>()
                .Property(e => e.SR)
                .IsUnicode(false);

            modelBuilder.Entity<SD_SemanticsType>()
                .Property(e => e.SR)
                .IsUnicode(false);

            modelBuilder.Entity<SD_SemanticsType>()
                .Property(e => e.CCCode1)
                .IsUnicode(false);

            modelBuilder.Entity<SD_SemanticsType>()
                .Property(e => e.CCCode2)
                .IsUnicode(false);

            modelBuilder.Entity<SD_SemanticsType>()
                .HasMany(e => e.SD_Semantics)
                .WithRequired(e => e.SD_SemanticsType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SD_TermSource>()
                .Property(e => e.CCCode)
                .IsUnicode(false);

            modelBuilder.Entity<SD_TermTranslation>()
                .Property(e => e.LangCode)
                .IsUnicode(false);
        }
    }
}
