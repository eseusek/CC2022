using Microsoft.EntityFrameworkCore;
namespace Produktverwaltung.Models
{
    public class ProduktContext : DbContext
    {
        public ProduktContext(DbContextOptions<ProduktContext> options)
            : base(options)
        {

        }
        public DbSet<Produkt> Produkte { get; set; }
    }
}
