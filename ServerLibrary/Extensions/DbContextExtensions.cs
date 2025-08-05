using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServerLibrary.Data;

namespace ServerLibrary.Extensions
{
    public static class DbContextExtensions
    {
        /// <summary>
        /// Clase de extensión que configura el acceso a la base de datos para la aplicación.
        /// <para>Este método abstrae la lógica de configuración de los distintos proveedores de base de datos soportados (SQLite, MySQL, SQL Server, etc),
        /// permitiendo registrar el <see cref="ApplicationDbContext"/> en el contenedor de servicios según el proveedor configurado.</para>
        /// </summary>
        /// <param name="services">La colección de servicios de la aplicación donde se registrarán las configuraciones del DbContext.</param>
        /// <param name="configuration">La configuración de la aplicación, desde donde se obtendrán los detalles de conexión y el proveedor de base de datos.</param>
        /// <returns>Una instancia de <see cref="IServiceCollection"/> con el <see cref="ApplicationDbContext"/> configurado.</returns>
        /// <exception cref="Exception">Se lanza si el proveedor de base de datos especificado en la configuración no es soportado.</exception>
        public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            // Leer el proveedor de base de datos desde el archivo de configuración
            var databaseProvider = configuration.GetValue<string>("DatabaseProvider");

            // Configurar las cadenas de conexión
            var sqlServerConnection = configuration.GetConnectionString("SqlServerConnection");
            var mySqlConnection = configuration.GetConnectionString("DefaultConnection");

            // Registrar el Contexto de BD especifico según el proveedor seleccionado
            switch (databaseProvider)
            {
                case "sqlserver":
                    services.AddDbContext<AppDbContextSqlServer>(options =>
                        options.UseSqlServer(sqlServerConnection, sql => {
                            sql.MigrationsAssembly("ServerLibrary");
                            sql.MigrationsHistoryTable("__EFMigrationsHistory", "dbo");
                        }));

                    // Registra el contexto base para que pueda ser inyectado en otros servicios sin importar el proveedor de base de datos activo
                    services.AddScoped<AppDbContext, AppDbContextSqlServer>();
                    break;

                case "mysql":
                    services.AddDbContext<AppDbContextMySql>(options =>
                        options.UseMySql(mySqlConnection, ServerVersion.AutoDetect(mySqlConnection), mysql => {
                            mysql.MigrationsAssembly("ServerLibrary");
                            mysql.MigrationsHistoryTable("__EFMigrationsHistory");
                        }));

                    services.AddScoped<AppDbContext, AppDbContextMySql>();
                    break;

                default:
                    throw new Exception($"Proveedor de base de datos no soportado: {databaseProvider}");
            }

            return services;
        }
    }
}
