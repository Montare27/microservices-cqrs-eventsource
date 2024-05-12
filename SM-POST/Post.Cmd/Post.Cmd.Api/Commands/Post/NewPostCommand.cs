namespace Post.Cmd.Api.Commands.Post;

using CQRS.Core.Commands;

/// <summary>
/// The new post command class
/// </summary>
/// <seealso cref="BaseCommand"/>
public class NewPostCommand : BaseCommand
{
	/// <summary>
	/// Gets or sets the value of the author
	/// </summary>
	public string Author { get; set; } = default!;
	/// <summary>
	/// Gets or sets the value of the message
	/// </summary>
	public string Message { get; set; } = default!;
}
