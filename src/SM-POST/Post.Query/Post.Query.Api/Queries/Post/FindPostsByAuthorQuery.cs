namespace Post.Query.Api.Queries.Post;

using CQRS.Core.Queries;

public class FindPostsByAuthorQuery : BaseQuery
{
	public string Author { get; set; } = default!;
}
