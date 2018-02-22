namespace Jurassic.So.Data.Entities
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DataNodeDBContext : DbContext
    {
        public DataNodeDBContext()
            : base("name=DefaultConnection")
        {
        }

        public virtual DbSet<API_DATA_NODE_INFO> API_DATA_NODE_INFO { get; set; }
        public virtual DbSet<API_DATA_RELATION> API_DATA_RELATION { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
