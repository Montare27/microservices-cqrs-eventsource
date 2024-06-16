namespace Post.Query.Infrastructure.Repositories;

using DataAccess;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

public class PostRepository(DatabaseContextFactory contextFactory) : IPostRepository
{
	public async Task CreateAsync(PostEntity post)
	{
		await using var context = contextFactory.CreateDbContext();
		await context.Posts.AddAsync(post);
		await context.SaveChangesAsync();
	}

	public async Task UpdateAsync(PostEntity post)
	{
		await using var context = contextFactory.CreateDbContext();
		context.Posts.Update(post);
		await context.SaveChangesAsync();
	}

	public async Task DeleteAsync(Guid id)
	{
		await using var context = contextFactory.CreateDbContext();
		var post = await GetByIdAsync(id);
		if(post == null) return;

		context.Posts.Remove(post);
		await context.SaveChangesAsync();
	}

	public async Task<PostEntity?> GetByIdAsync(Guid id)
	{
		await using var context = contextFactory.CreateDbContext();
		return await context.Posts
			.Include(p => p.Comments)
			.FirstOrDefaultAsync(x => x.Id.Equals(id));
	}

	public async Task<List<PostEntity>> ListAllAsync()
	{
		await using var context = contextFactory.CreateDbContext();
		return await context.Posts.AsNoTracking()
			.Include(p => p.Comments).AsNoTracking()
			.ToListAsync();
	}

	public async Task<List<PostEntity>> ListByAuthorAsync(string author)
	{
		await using var context = contextFactory.CreateDbContext();
		return await context.Posts.AsNoTracking()
			.Include(p => p.Comments).AsNoTracking()
			.Where(p => p.Author.Equals(author))
			.ToListAsync();
	}

	public async Task<List<PostEntity>> ListWithLikesAsync(int numberOfLikes)
	{
		await using var context = contextFactory.CreateDbContext();
		return await context.Posts.AsNoTracking()
			.Include(p => p.Comments).AsNoTracking()
			.Where(p => p.Likes >= numberOfLikes)
			.ToListAsync();
	}

	public async Task<List<PostEntity>> ListWithCommentsAsync()
	{
		await using var context = contextFactory.CreateDbContext();
		return await context.Posts.AsNoTracking()
			.Include(p => p.Comments).AsNoTracking()
			.Where(p => p.Comments != null && p.Comments.Count != 0)
			.ToListAsync();
	}
}
