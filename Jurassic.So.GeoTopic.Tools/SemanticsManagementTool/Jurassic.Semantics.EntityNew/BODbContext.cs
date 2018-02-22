namespace Jurassic.Semantics.EntityNew
{
    using System.Data.Entity;

    public partial class BODbContext : DbContext
    {
        public BODbContext()
            : base("name=BODbContext")
        {
        }

        public virtual DbSet<BO_BaseInfo> BO_BaseInfo { get; set; }
        public virtual DbSet<BO_BOAlias> BO_BOAlias { get; set; }
        public virtual DbSet<BO_Relation> BO_Relation { get; set; }
        public virtual DbSet<Dict_BOView> Dict_BOView { get; set; }
        public virtual DbSet<ES_BORelationView> ES_BORelationView { get; set; }
        public virtual DbSet<ES_BOTermView> ES_BOTermView { get; set; }

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
        }
    }
}
