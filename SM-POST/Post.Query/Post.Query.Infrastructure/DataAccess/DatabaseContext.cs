namespace Post.Query.Infrastructure.DataAccess;

using Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class DatabaseContext(DbContextOptions options) : DbContext(options)
{
	public DbSet<PostEntity> Posts { get; set; } = default!;
	public DbSet<CommentEntity> Comments { get; set; } = default!;
}
