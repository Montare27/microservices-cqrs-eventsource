namespace Post.Query.Infrastructure.Repositories;

using DataAccess;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

public class CommentRepository(DatabaseContextFactory contextFactory) : ICommentRepository
{
	public async Task CreateAsync(CommentEntity comment)
	{
		await using var context = contextFactory.CreateDbContext();
		await context.Comments.AddAsync(comment);
		await context.SaveChangesAsync();
	}

	public async Task UpdateAsync(CommentEntity comment)
	{
		await using var context = contextFactory.CreateDbContext();
		context.Comments.Update(comment);
		await context.SaveChangesAsync();
	}

	public async Task DeleteAsync(Guid id)
	{
		await using var context = contextFactory.CreateDbContext();
		var comment = await GetByIdAsync(id);
		if (comment == null)
		{
			return;
		}
		context.Comments.Remove(comment);
		await context.SaveChangesAsync();
	}

	public async Task<CommentEntity?> GetByIdAsync(Guid id)
	{
		await using var context = contextFactory.CreateDbContext();
		return await context.Comments
			.FirstOrDefaultAsync(x => x.CommentId.Equals(id));
	}
}
