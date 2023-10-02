using EmersonDB.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace EmersonDB
{
    public partial class EmersonContext : DbContext
    {
        public EmersonContext()
        {
        }

        public EmersonContext(DbContextOptions<EmersonContext> options)
            : base(options)
        {

        }

        public virtual DbSet<Variable> Variables { get; set; }
        public virtual DbSet<City> Cities { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(configuration.GetConnectionString("EmersonDB"));
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer("Server=tcp:emerson-testdb.database.windows.net,1433;Initial Catalog=EmersonTest;Persist Security Info=False;User ID=EmersonAdmin;Password=emerson123!.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
