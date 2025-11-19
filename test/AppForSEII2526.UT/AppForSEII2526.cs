using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

public class AppForSEII25264SqliteUT
{
    public AppForSEII25264SqliteUT()
    {
        // ⛔ IMPORTANTE: evita que SeedData se ejecute durante los tests
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");

        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        _contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new ApplicationDbContext(_contextOptions);
        _context.Database.EnsureCreated();
    }

    protected readonly DbConnection _connection;
    protected readonly ApplicationDbContext _context;
    protected readonly DbContextOptions<ApplicationDbContext> _contextOptions;

    protected ApplicationDbContext CreateContext() => new(_contextOptions);

    public void Dispose() => _connection.Dispose();
}
