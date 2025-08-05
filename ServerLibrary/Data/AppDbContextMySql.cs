using Microsoft.EntityFrameworkCore;

namespace ServerLibrary.Data
{
    /// <summary>
    /// This class is used to configure the MySQL database context for the application.
    /// </summary>
    /// <param name="options"></param>
    public class AppDbContextMySql(DbContextOptions<AppDbContextMySql> options) : AppDbContext(options)
    {

    }
}
