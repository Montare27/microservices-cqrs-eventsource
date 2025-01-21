namespace Post.Query.Infrastructure.DataAccess;

using Microsoft.EntityFrameworkCore;

public class DatabaseContextFactory(Action<DbContextOptionsBuilder> configureDbContext)
{
	public DatabaseContext CreateDbContext()
	{
		DbContextOptionsBuilder<DatabaseContext> optionsBuilder = new();
		configureDbContext(optionsBuilder);

		return new DatabaseContext(optionsBuilder.Options);
	}
}
