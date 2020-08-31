using Microsoft.EntityFrameworkCore;

namespace MangaQuotes.Data
{
    public class QuoteDbContext : DbContext
    {
        public QuoteDbContext(DbContextOptions<QuoteDbContext> options) : base(options)
        {

        }

        public DbSet<Character> Characters { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        
    }
}