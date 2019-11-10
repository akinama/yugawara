using System.Configuration;
using System.Numerics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Yugawara
{
    public class DefaultDbContext : DbContext
    {
        public DbSet<Commitment> Commitments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            var connectionString = "Server=tcp:yugawarasqlserver.database.windows.net,1433;Initial Catalog=yugawarasqldatabase;Persist Security Info=False;User ID=4dm1n157r470r;Password=H2DyzWQgrEZ>zcYfd;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            // if (ConfigurationManager.ConnectionStrings["DefaultConnection"] == null)
            // {
            //     connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            // }
            
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            var converter = new ValueConverter<BigInteger, long>(    
                model => (long)model,
                provider => new BigInteger(provider));

           builder.Entity<Commitment>()
                .Property(e => e.Deposit)
                .HasConversion(converter);
        }
    }
}
