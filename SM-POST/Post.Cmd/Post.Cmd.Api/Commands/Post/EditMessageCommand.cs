namespace Post.Cmd.Api.Commands.Post;

using CQRS.Core.Commands;

/// <summary>
/// The edit message command class
/// </summary>
/// <seealso cref="BaseCommand"/>
public class EditMessageCommand : BaseCommand
{
	/// <summary>
	/// Gets or sets the value of the message
	/// </summary>
	public string Message { get; set; } = default!;
}
