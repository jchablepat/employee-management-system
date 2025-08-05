using Microsoft.EntityFrameworkCore;

namespace ServerLibrary.Data
{
    /// <summary>
    /// This class is used to configure the SQL Server database context for the application.
    /// </summary>
    /// <param name="options"></param>
    public class AppDbContextSqlServer(DbContextOptions<AppDbContextSqlServer> options) : AppDbContext(options)
    {

    }
}
