namespace Post.Cmd.Api.Commands;

using Comment;
using Post;

public interface ICommandHandler
{
	Task HandleAsync(NewPostCommand command);

	Task HandleAsync(LikePostCommand command);

	Task HandleAsync(DeletePostCommand command);

	Task HandleAsync(EditMessageCommand command);

	Task HandleAsync(AddCommentCommand command);

	Task HandleAsync(EditCommentCommand command);

	Task HandleAsync(RemoveCommentCommand command);
}
