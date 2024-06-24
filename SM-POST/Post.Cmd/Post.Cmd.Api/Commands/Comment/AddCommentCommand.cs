namespace Post.Cmd.Api.Commands.Comment;

using CQRS.Core.Commands;

/// <summary>
///     The add comment command class
/// </summary>
/// <seealso cref="BaseCommand" />
public class AddCommentCommand : BaseCommand
{
	/// <summary>
	///     Gets or sets the value of the comment
	/// </summary>
	public string Comment { get; set; } = default!;
	/// <summary>
	///     Gets or sets the value of the username
	/// </summary>
	public string Username { get; set; } = default!;
}
