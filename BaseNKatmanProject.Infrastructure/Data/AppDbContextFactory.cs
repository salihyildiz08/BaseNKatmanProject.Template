using BaseNKatmanProject.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Text.Json;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var basePath = Directory.GetCurrentDirectory();
        var appSettingsPath = Path.Combine(basePath, "appsettings.json");
        var json = File.ReadAllText(appSettingsPath);
        using var doc = JsonDocument.Parse(json);
        var connectionString = doc.RootElement.GetProperty("ConnectionStrings").GetProperty("DefaultConnection").GetString();

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new AppDbContext(optionsBuilder.Options, null);
    }
}
