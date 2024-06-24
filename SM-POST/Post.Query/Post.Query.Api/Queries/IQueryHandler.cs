namespace Post.Query.Api.Queries;

using Domain.Entities;
using Post;

public interface IQueryHandler
{
	Task<List<PostEntity>> HandleAsync(FindAllPostsQuery query);
	Task<PostEntity?> HandleAsync(FindPostByIdQuery query);
	Task<List<PostEntity>> HandleAsync(FindPostsByAuthorQuery query);
	Task<List<PostEntity>> HandleAsync(FindPostsWithCommentsQuery query);
	Task<List<PostEntity>> HandleAsync(FindPostsWithLikesQuery query);
}
