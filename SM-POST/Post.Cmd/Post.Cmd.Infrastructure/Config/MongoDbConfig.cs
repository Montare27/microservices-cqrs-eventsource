namespace Post.Cmd.Infrastructure.Config;

public class MongoDbConfig
{
	public string ConnectionString { get; set; } = default!;
	public string Database { get; set; } = default!;
	public string Collection { get; set; } = default!;
}
