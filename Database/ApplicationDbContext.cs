using Database.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Database;

public partial class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{	
	public DbSet<GameInstance> GameInstances { get; set; }
	public DbSet<GameInstancePlayer> GameInstancePlayers { get; set; }
	public DbSet<ReadyPlayer> ReadyPlayers { get; set; }

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);
		builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
	}
}

public class AppDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
	private static IConfiguration Config => new ConfigurationBuilder()
		.AddJsonFile("appsettings.json", optional: false)
		.AddJsonFile("appsettings.Development.json", optional: false)
		.Build();

	public ApplicationDbContext CreateDbContext(string[] args)
	{
		var connectionName = (!args?.Any() ?? true) ? "DefaultConnection" : args![0];

		var connectionString = Config.GetConnectionString(connectionName);

		Console.WriteLine($"args = {string.Join(", ", args!)}");
		Console.WriteLine($"connection name = {connectionName}");
		Console.WriteLine($"connection string = {connectionString}");

		var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
		builder.UseNpgsql(connectionString);
		return new ApplicationDbContext(builder.Options);
	}
}