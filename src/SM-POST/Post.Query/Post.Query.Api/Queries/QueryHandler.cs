﻿namespace Post.Query.Api.Queries;

using Domain.Entities;
using Domain.Repositories;
using Post;

public class QueryHandler(IPostRepository repository) : IQueryHandler
{

	public Task<List<PostEntity>> HandleAsync(FindAllPostsQuery query)
	{
		return repository.ListAllAsync();
	}

	public async Task<List<PostEntity>> HandleAsync(FindPostByIdQuery query)
	{
		var post = await repository.GetByIdAsync(query.Id);
		return post != null
			?[post]
			:[];
	}

	public Task<List<PostEntity>> HandleAsync(FindPostsByAuthorQuery query)
	{
		return repository.ListByAuthorAsync(query.Author);
	}

	public Task<List<PostEntity>> HandleAsync(FindPostsWithCommentsQuery query)
	{
		return repository.ListWithCommentsAsync();
	}

	public Task<List<PostEntity>> HandleAsync(FindPostsWithLikesQuery query)
	{
		return repository.ListWithLikesAsync(query.NumberOfLikes);
	}
}
