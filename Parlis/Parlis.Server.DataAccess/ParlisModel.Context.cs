namespace Parlis.Server.DataAccess
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    public partial class ParlisContext : DbContext
    {
        public ParlisContext()
            : base("name=ParlisContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Match> Matches { get; set; }
        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<PlayerProfile> PlayerProfiles { get; set; }
    }
}