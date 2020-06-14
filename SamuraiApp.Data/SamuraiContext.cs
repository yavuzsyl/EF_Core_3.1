using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SamuraiApp.Domain;


namespace SamuraiApp.Data
{
    //in efcore we have tot explicitly tell to context which sqlprovider and connection string we are using
    public class SamuraiContext : DbContext
    {
        public DbSet<Samurai> Samurais { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Clan> Clans { get; set; }
        public DbSet<Battle> Battles { get; set; }
        public DbSet<SamuraiBattle> SamuraiBattles { get; set; }


        public static readonly ILoggerFactory ConsoleLogFactory
            = LoggerFactory.Create(builder =>
            {
                builder.
                    AddFilter((category, level) =>
                        category == DbLoggerCategory.Database.Command.Name
                        && level == LogLevel.Information)
                    .AddConsole();
                    
            });


        //the first time efcore instantiate samuraicontext class it will trigger this method
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLoggerFactory(ConsoleLogFactory)//LOGGER factory will log every time context called?
                .EnableSensitiveDataLogging(true)
                .UseSqlServer("Data Source =(localdb)\\MSSQLLocalDB; Initial Catalog = SamuraiAppData", options => options.MaxBatchSize(150));
        }


        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<SamuraiBattle>().HasKey(x => new { x.BattleId, x.SamuraiId });//composite key
            mb.Entity<Samurai>().HasOne(x => x.Horse).WithOne(x => x.Samurai).IsRequired(true);
            mb.Entity<Horse>().ToTable("Horses");
                            
        }
    }
}
