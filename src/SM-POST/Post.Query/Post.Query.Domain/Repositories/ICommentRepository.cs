namespace Post.Query.Domain.Repositories;

using Entities;

public interface ICommentRepository
{
	Task CreateAsync(CommentEntity comment);

	Task UpdateAsync(CommentEntity comment);

	Task DeleteAsync(Guid id);

	Task<CommentEntity?> GetByIdAsync(Guid id);
}
