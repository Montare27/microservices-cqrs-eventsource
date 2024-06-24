namespace Post.Query.Api.Queries.Post;

using CQRS.Core.Queries;

public class FindPostByIdQuery : BaseQuery
{
	public Guid PostId { get; set; } = default!;
}
