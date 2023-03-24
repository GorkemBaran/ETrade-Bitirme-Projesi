using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DataAccess.Contexts
{
    public class ETradeContextFactory : IDesignTimeDbContextFactory<ETradeContext>
    {
        public ETradeContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ETradeContext>();
            optionsBuilder.UseSqlServer("server=.\\SQLEXPRESS;database=ETrade Projem;user id=sa;password=sa;multipleactiveresultsets=true;trustservercertificate=true;");
            return new ETradeContext(optionsBuilder.Options);
        }
    }
}
