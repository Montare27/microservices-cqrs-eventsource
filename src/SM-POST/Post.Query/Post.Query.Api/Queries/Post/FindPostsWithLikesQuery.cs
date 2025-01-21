namespace Post.Query.Api.Queries.Post;

using CQRS.Core.Queries;

public class FindPostsWithLikesQuery : BaseQuery
{
	public int NumberOfLikes { get; set; }
}
