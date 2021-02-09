using AssetsAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AssetsAPI.Data
{
    public class CurrencyContext : DbContext
    {

        public CurrencyContext(DbContextOptions<CurrencyContext> options) : base(options) { }

        public DbSet<currency> currencies { get; set; }
        public DbSet<provider> providers { get; set; }
        public DbSet<asset> assets { get; set; }
        public DbSet<price> prices { get; set; }
    }
}
