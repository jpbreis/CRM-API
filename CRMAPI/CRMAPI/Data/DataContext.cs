using Microsoft.EntityFrameworkCore;

namespace CRMAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<Operador> Operadores => Set<Operador>();
    }
}
