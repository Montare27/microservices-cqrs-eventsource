namespace Post.Cmd.Api.Commands.Post;

using CQRS.Core.Commands;

/// <summary>
///     The delete post command class
/// </summary>
/// <seealso cref="BaseCommand" />
public class DeletePostCommand : BaseCommand
{
	/// <summary>
	///     Gets or sets the value of the username
	/// </summary>
	public string Username { get; set; } = default!;
}
