using BaseLibrary.Entities;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Data.Seeders;
using ServerLibrary.Extensions;
using ServerLibrary.Helpers;
using ServerLibrary.Repositories.Contracts;
using ServerLibrary.Repositories.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<JwtSection>(builder.Configuration.GetSection("JwtSection"));

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    //options.UseSqlServer(connectionString ?? throw new InvalidOperationException("Sorry your connection is not found!"));
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

// Configurar JWT
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddScoped<IUserAccount, UserAccountRepository>();

builder.Services.AddScoped<IGenericRepository<GeneralDepartment>, GeneralDepartmentRepository>();
builder.Services.AddScoped<IGenericRepository<Department>, DepartmentRepository>();
builder.Services.AddScoped<IGenericRepository<Branch>, BranchRepository>();
builder.Services.AddScoped<IGenericRepository<Country>, CountryRepository>();
builder.Services.AddScoped<IGenericRepository<City>, CityRepository>();
builder.Services.AddScoped<IGenericRepository<Town>, TownRepository>();
builder.Services.AddScoped<IGenericRepository<Employee>, EmployeeRepository>();

builder.Services.AddHealthChecks().AddCheck<SystemResourcesHealthCheck>("System Resources");
builder.Services.AddTransient<AdminDataSeeder>();

builder.Services.AddCors(options => {
    options.AddPolicy("AllowBlazorWasm", 
        builder => builder
        //.WithOrigins("https://localhost:7268") // Allow specific origins
        .SetIsOriginAllowed(hosts => true) //Allow any origin
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
} else
{
    // Ejecutar migraciones autom�ticamente al iniciar la aplicaci�n
    using var scope = app.Services.CreateScope();
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        context.Database.Migrate();

        // Seed initial data
        var adminSeeder = scope.ServiceProvider.GetRequiredService<AdminDataSeeder>();
        adminSeeder.Seed();
    }
    catch {}
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("AllowBlazorWasm");
app.UseAuthentication();
app.UseAuthorization(); // Configured by Default 

app.MapControllers();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";

        var result = new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(entry => new
            {
                name = entry.Key,
                status = entry.Value.Status.ToString(),
                description = entry.Value.Description
            })
        };

        await context.Response.WriteAsJsonAsync(result);
    }
});


app.Run();
