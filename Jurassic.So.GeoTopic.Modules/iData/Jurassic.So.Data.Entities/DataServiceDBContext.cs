using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Jurassic.So.Data.Entities
{
    public partial class DataServiceDBContext : DbContext
    {
        static DataServiceDBContext()
        {
            Database.SetInitializer<DataServiceDBContext>(null);
        }

        public DataServiceDBContext()
            : base("Name=DataServiceDBContext")
        {
        }

        public virtual DbSet<GT_AdapterInfo> GT_AdapterInfo { get; set; }
        public virtual DbSet<GT_PlanInfo> GT_PlanInfo { get; set; }
        public virtual DbSet<GT_SpiderScope> GT_SpiderScope { get; set; }
        public virtual DbSet<GT_TaskLogInfo> GT_TaskLogInfo { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GT_AdapterInfo>()
                .Property(e => e.SDomain)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<GT_AdapterInfo>()
                .HasMany(e => e.GT_PlanInfo)
                .WithOptional(e => e.GT_AdapterInfo)
                .HasForeignKey(e => e.AdapterId);

            modelBuilder.Entity<GT_AdapterInfo>()
                .HasMany(e => e.GT_SpiderScope)
                .WithRequired(e => e.GT_AdapterInfo)
                .HasForeignKey(e => e.AdapterId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GT_SpiderScope>()
                .Property(e => e.Desc)
                .IsUnicode(false);

            modelBuilder.Entity<GT_TaskLogInfo>()
                .Property(e => e.FailturReason)
                .IsUnicode(false);
        }
    }
}
