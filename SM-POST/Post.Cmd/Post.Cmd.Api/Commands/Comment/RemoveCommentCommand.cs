namespace Post.Cmd.Api.Commands.Comment;

using CQRS.Core.Commands;

/// <summary>
/// The remove comment command class
/// </summary>
/// <seealso cref="BaseCommand"/>
public class RemoveCommentCommand : BaseCommand
{
	/// <summary>
	/// Gets or sets the value of the comment id
	/// </summary>
	public Guid CommentId { get; set; }
	/// <summary>
	/// Gets or sets the value of the username
	/// </summary>
	public string Username { get; set; } = default!;
}
