namespace Jurassic.Semantics.Entity.EntityModel
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

        public virtual DbSet<BO_BaseInfo> BO_BaseInfo { get; set; }
        public virtual DbSet<BO_BOAlias> BO_BOAlias { get; set; }
        public virtual DbSet<BO_Relation> BO_Relation { get; set; }
        public virtual DbSet<CD_RelType> CD_RelType { get; set; }
        public virtual DbSet<CD_TypeCode> CD_TypeCode { get; set; }
        public virtual DbSet<SD_CCTerm> SD_CCTerm { get; set; }
        public virtual DbSet<SD_ConceptClass> SD_ConceptClass { get; set; }
        public virtual DbSet<SD_ProfessionalTerm> SD_ProfessionalTerm { get; set; }
        public virtual DbSet<SD_Semantics> SD_Semantics { get; set; }
        public virtual DbSet<SD_SemanticsType> SD_SemanticsType { get; set; }
        public virtual DbSet<SD_TermKeyword> SD_TermKeyword { get; set; }
        public virtual DbSet<SD_TermSource> SD_TermSource { get; set; }
        public virtual DbSet<SD_TermTranslation> SD_TermTranslation { get; set; }
        public virtual DbSet<tmp_Location> tmp_Location { get; set; }
        public virtual DbSet<Log> Log { get; set; }
        public virtual DbSet<Dict_BOView> Dict_BOView { get; set; }
        public virtual DbSet<Dict_ProfessionalView> Dict_ProfessionalView { get; set; }
        public virtual DbSet<ES_BORelationView> ES_BORelationView { get; set; }
        public virtual DbSet<ES_BOTermView> ES_BOTermView { get; set; }
        public virtual DbSet<ES_EPsOfPTView> ES_EPsOfPTView { get; set; }
        public virtual DbSet<ES_SemanticsTermView> ES_SemanticsTermView { get; set; }
        public virtual DbSet<SMT_BFTreeView> SMT_BFTreeView { get; set; }
        public virtual DbSet<SMT_BOTTreeView> SMT_BOTTreeView { get; set; }
        public virtual DbSet<SMT_GNTreeView> SMT_GNTreeView { get; set; }
        public virtual DbSet<USP_GetTermTree> USP_GetTermTree { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BO_BaseInfo>()
                .HasMany(e => e.BO_BOAlias)
                .WithRequired(e => e.BO_BaseInfo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BO_BaseInfo>()
                .HasMany(e => e.BO_Relation)
                .WithRequired(e => e.BO_BaseInfo)
                .HasForeignKey(e => e.ID1)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BO_BaseInfo>()
                .HasMany(e => e.BO_Relation1)
                .WithRequired(e => e.BO_BaseInfo1)
                .HasForeignKey(e => e.ID2)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CD_RelType>()
                .HasMany(e => e.BO_Relation)
                .WithRequired(e => e.CD_RelType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SD_CCTerm>()
                .Property(e => e.CCCode)
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

            modelBuilder.Entity<SD_ConceptClass>()
                .HasMany(e => e.SD_CCTerm)
                .WithRequired(e => e.SD_ConceptClass)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SD_ConceptClass>()
                .HasMany(e => e.SD_SemanticsType)
                .WithOptional(e => e.SD_ConceptClass)
                .HasForeignKey(e => e.CC1);

            modelBuilder.Entity<SD_ConceptClass>()
                .HasMany(e => e.SD_SemanticsType1)
                .WithOptional(e => e.SD_ConceptClass1)
                .HasForeignKey(e => e.CC2);

            modelBuilder.Entity<SD_Semantics>()
                .Property(e => e.SR)
                .IsUnicode(false);

            modelBuilder.Entity<SD_SemanticsType>()
                .Property(e => e.SR)
                .IsUnicode(false);

            modelBuilder.Entity<SD_SemanticsType>()
                .HasMany(e => e.SD_Semantics)
                .WithRequired(e => e.SD_SemanticsType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SD_TermTranslation>()
                .Property(e => e.LangCode)
                .IsUnicode(false);

            modelBuilder.Entity<Log>()
                .Property(e => e.Thread)
                .IsUnicode(false);

            modelBuilder.Entity<Log>()
                .Property(e => e.Level)
                .IsUnicode(false);

            modelBuilder.Entity<Log>()
                .Property(e => e.Logger)
                .IsUnicode(false);

            modelBuilder.Entity<Log>()
                .Property(e => e.Message)
                .IsUnicode(false);

            modelBuilder.Entity<Log>()
                .Property(e => e.Exception)
                .IsUnicode(false);

            modelBuilder.Entity<Log>()
                .Property(e => e.Request)
                .IsUnicode(false);

            modelBuilder.Entity<Log>()
                .Property(e => e.Response)
                .IsUnicode(false);

            modelBuilder.Entity<SMT_BFTreeView>()
                .Property(e => e.SR)
                .IsUnicode(false);

            modelBuilder.Entity<SMT_BOTTreeView>()
                .Property(e => e.SR)
                .IsUnicode(false);

            modelBuilder.Entity<SMT_GNTreeView>()
                .Property(e => e.SR)
                .IsUnicode(false);
        }
    }
}
